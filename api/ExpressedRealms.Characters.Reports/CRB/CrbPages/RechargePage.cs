using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using QRCoder;

namespace ExpressedRealms.Characters.Reports.CRB.CrbPages;

internal static class RechargePage
{
    public static void FillInRechargePage(ReportData reportData, PdfDocument document)
    {
        var page = document.Pages[0];
        var characterLevel = int.Parse(reportData.BasicInfo.CharacterLevel);

        var recharges = GetRechargesAndPool(reportData, characterLevel, page, out var pool);

        var powerType = GetPowerPointTypeForExpressionName(reportData.BasicInfo);

        TextPrintUtilities.PrintStatInfo(
            page,
            powerType,
            XUnitPt.FromInch(0.51),
            XUnitPt.FromInch(9.00)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            recharges.ToString(),
            XUnitPt.FromInch(0.51),
            XUnitPt.FromInch(7.82)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            pool.ToString(),
            XUnitPt.FromInch(0.51),
            XUnitPt.FromInch(6.15)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            reportData.WealthInfo.WealthLevel.ToString(),
            XUnitPt.FromInch(3.02),
            XUnitPt.FromInch(5.38)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            reportData.WealthInfo.WealthIncome.ToString("C0"),
            XUnitPt.FromInch(3.02),
            XUnitPt.FromInch(4.68)
        );

        // Day 1
        if (reportData.BasicInfo.CurrentDay > 1)
        {
            GenerateRechargeBoxes(page, 0, XUnitPt.FromInch(0.975), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, 0, XUnitPt.FromInch(0.975), XUnitPt.FromInch(6.505));
        }
        else
        {
            GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(0.975), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(0.975), XUnitPt.FromInch(6.505));
        }

        // Day 2
        if (reportData.BasicInfo.CurrentDay > 2)
        {
            GenerateRechargeBoxes(page, 0, XUnitPt.FromInch(1.74), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, 0, XUnitPt.FromInch(1.74), XUnitPt.FromInch(6.505));
        }
        else
        {
            GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(1.74), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(1.74), XUnitPt.FromInch(6.505));
        }

        GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(2.51), XUnitPt.FromInch(9.92));
        GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(2.51), XUnitPt.FromInch(6.505));
        
        DrawQrCode(reportData.BasicInfo.LookupId, document, page);
    }

    
    private static void DrawQrCode(
        string content,
        PdfDocument document,
        PdfPage page
    )
    {
        
        var anchor = document.AcroForm.Fields["QrAnchor"] as PdfTextField;

        if (anchor == null)
            throw new InvalidOperationException("QrAnchor field not found.");

        PdfDictionary? widget;

        // Try /Kids first
        var kids = anchor.Elements["/Kids"] as PdfArray;

        if (kids != null && kids.Elements.Count > 0)
        {
            widget = kids.Elements[0] as PdfDictionary;
        }
        else
        {
            // Field itself is the widget
            widget = anchor;
        }

        if (widget == null)
            throw new InvalidOperationException("QrAnchor field not found.");

        var rect = widget.Elements.GetRectangle("/Rect");

        var x = XUnitPt.FromPoint(rect.X1);
        var y = XUnitPt.FromPoint(page.Height.Point - rect.Y2);
        var width = XUnitPt.FromPoint(rect.Width);
        var height = XUnitPt.FromPoint(rect.Height);

        using var gfx = XGraphics.FromPdfPage(page);

        
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);

        var matrix = data.ModuleMatrix;
        var modules = matrix.Count;

        double moduleSize = XUnitPt.FromPoint(Math.Min(width, height) / modules);

        // Center inside bounds
        var offsetX = x + (width - modules * moduleSize) / 2;
        var offsetY = y + (height - modules * moduleSize) / 2;

        for (int row = 0; row < modules; row++)
        {
            for (int col = 0; col < modules; col++)
            {
                if (matrix[row][col])
                {
                    gfx.DrawRectangle(
                        XBrushes.Black,
                        XUnitPt.FromPoint(offsetX + col * moduleSize),
                        XUnitPt.FromPoint(offsetY + row * moduleSize),
                        XUnitPt.FromPoint(moduleSize),
                        XUnitPt.FromPoint(moduleSize)
                    );
                }
            }
        }
    }
    
    private static int GetRechargesAndPool(
        ReportData reportData,
        int characterLevel,
        PdfPage page,
        out int pool
    )
    {
        // Free PP / Recharges per day
        var recharges = characterLevel / 2 + 1;
        pool = 12;

        var energeticallyInfused = reportData.Traits.Advantages.FirstOrDefault(x =>
            x.Name == "Energetically Infused"
        );
        var weakenedConnection = reportData.Traits.Disadvantages.FirstOrDefault(x =>
            x.Name == "Weakened Connection to Pryma"
        );
        var notorious = reportData.Traits.Disadvantages.FirstOrDefault(x =>
            x.Name == "Notorious / Gossip Magnet"
        );
        var disowned = reportData.Traits.Disadvantages.FirstOrDefault(x =>
            x.Name == "Disowned / Disfavored"
        );

        // Energetically Infused
        if (energeticallyInfused is not null)
        {
            TextPrintUtilities.PrintStatInfo(
                page,
                "x",
                XUnitPt.FromInch(0.77),
                XUnitPt.FromInch(8.30)
            );
            if (energeticallyInfused.Cost == "4pt")
            {
                recharges += 1;
            }
            if (energeticallyInfused.Cost == "8pt")
            {
                // 1 PP Bonus
                TextPrintUtilities.PrintStatInfo(
                    page,
                    "x",
                    XUnitPt.FromInch(0.77),
                    XUnitPt.FromInch(7.57)
                );
                recharges += 1;
                pool += characterLevel;
            }
        }

        // Notorious
        if (notorious is not null)
        {
            TextPrintUtilities.PrintStatInfo(
                page,
                "x",
                XUnitPt.FromInch(0.77),
                XUnitPt.FromInch(10.50)
            );
        }

        // Disowned
        if (disowned is not null)
        {
            TextPrintUtilities.PrintStatInfo(
                page,
                "x",
                XUnitPt.FromInch(0.77),
                XUnitPt.FromInch(9.775)
            );
            TextPrintUtilities.CrossStampInfo(
                page,
                "☥",
                XUnitPt.FromInch(2.155),
                XUnitPt.FromInch(4.98)
            );
        }

        // Weakened
        if (weakenedConnection is not null)
        {
            TextPrintUtilities.PrintStatInfo(
                page,
                "x",
                XUnitPt.FromInch(0.77),
                XUnitPt.FromInch(9.045)
            );
            if (weakenedConnection.Cost == "4pt")
            {
                pool = 8;
            }

            if (weakenedConnection.Cost == "8pt")
            {
                recharges = 1;
                pool = 8;
            }
        }

        return recharges;
    }

    private static string GetPowerPointTypeForExpressionName(BasicInfo basicInfo)
    {
        var powerType = string.Empty;
        if (basicInfo.Expression == "Adepts")
        {
            powerType = "Chi";
        }
        else if (basicInfo.Expression == "Aeternari")
        {
            powerType = "Vitality";
        }
        else if (basicInfo.Expression == "Shammas")
        {
            powerType = "Noumenon";
        }
        else if (basicInfo.Expression == "Sidhe")
        {
            powerType = "Essence";
        }
        else if (basicInfo.Expression == "Sorcerers")
        {
            powerType = "Mana";
        }
        else if (basicInfo.Expression == "Vampyres")
        {
            powerType = "Blood";
        }

        return powerType;
    }

    private static void GenerateRechargeBoxes(
        PdfPage page,
        int recharges,
        double startX,
        double startY
    )
    {
        var boxCount = 1;
        for (int y = 0; y < 5; y++)
        {
            var coordinateX = startX;
            var coordinateY = startY - (y * XUnitPt.FromInch(0.78));
            if (boxCount > recharges)
            {
                TextPrintUtilities.CrossStampInfo(
                    page,
                    "x",
                    coordinateX + XUnitPt.FromInch(0.67) / 2,
                    coordinateY + XUnitPt.FromInch(0.67) / 2
                );
            }
            boxCount++;
        }
    }

    private static void GenerateRechargePoolBoxes(
        PdfPage page,
        int pool,
        double startX,
        double startY
    )
    {
        var boxCount = 1;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var coordinateX = startX + (x * XUnitPt.FromInch(0.17));
                var coordinateY = startY - (y * XUnitPt.FromInch(0.17));
                if (boxCount > pool)
                {
                    TextPrintUtilities.PrintStatInfo(
                        page,
                        "x",
                        coordinateX + XUnitPt.FromInch(0.14) / 2,
                        coordinateY + XUnitPt.FromInch(0.14) / 2
                    );
                }
                boxCount++;
            }
        }
    }
}
