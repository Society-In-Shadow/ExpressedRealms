using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ExpressedRealms.Characters.Reports.CRB.CrbPages;

internal static class RechargePage
{
    public static void FillInRechargePage(
        ReportData reportData,
        PdfDocument document
    )
    {
        var page = document.Pages[0];
        var characterLevel = int.Parse(reportData.BasicInfo.CharacterLevel);

        // Free PP / Recharges per day
        var recharges = characterLevel / 2 + 1;
        var pool = 12;

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
            TextPrintUtilities.PrintStatInfo(page, "x", XUnitPt.FromInch(0.953), XUnitPt.FromInch(5));
            if (energeticallyInfused.Cost == "4pt")
            {
                recharges += 1;
            }
            if (energeticallyInfused.Cost == "8pt")
            {
                recharges += 1;
                pool += characterLevel;
            }
        }

        // Notorious
        if (notorious is not null)
        {
            TextPrintUtilities.PrintStatInfo(page, "x", XUnitPt.FromInch(0.473), XUnitPt.FromInch(5));
        }

        // Disowned
        if (disowned is not null)
        {
            TextPrintUtilities.PrintStatInfo(page, "x", XUnitPt.FromInch(0.633), XUnitPt.FromInch(5));
            TextPrintUtilities.CrossStampInfo(page, "☥", XUnitPt.FromInch(1.80), XUnitPt.FromInch(4.71));
        }

        // Weakened
        if (weakenedConnection is not null)
        {
            TextPrintUtilities.PrintStatInfo(page, "x", XUnitPt.FromInch(0.793), XUnitPt.FromInch(5));
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

        var powerType = GetPowerPointTypeForExpressionName(reportData.BasicInfo);

        TextPrintUtilities.PrintStatInfo(page, powerType, XUnitPt.FromInch(0.47), XUnitPt.FromInch(9.00));

        TextPrintUtilities.PrintStatInfo(page, recharges.ToString(), XUnitPt.FromInch(0.47), XUnitPt.FromInch(7.79));
        TextPrintUtilities.PrintStatInfo(page, pool.ToString(), XUnitPt.FromInch(0.47), XUnitPt.FromInch(6.96));
        
        TextPrintUtilities.PrintStatInfo(page, reportData.WealthInfo.WealthLevel.ToString(), XUnitPt.FromInch(3.02), XUnitPt.FromInch(5.88));
        TextPrintUtilities.PrintStatInfo(page, reportData.WealthInfo.WealthIncome.ToString("C0"), XUnitPt.FromInch(3.02), XUnitPt.FromInch(5.01));

        // Day 1
        if (reportData.BasicInfo.CurrentDay > 1)
        {
            GenerateRechargeBoxes(page, 0, XUnitPt.FromInch(0.8), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, 0, XUnitPt.FromInch(0.8), XUnitPt.FromInch(7.32));
        }
        else
        {
            GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(0.8), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(0.8), XUnitPt.FromInch(7.32));
        }

        // Day 2
        if (reportData.BasicInfo.CurrentDay > 2)
        {
            GenerateRechargeBoxes(page, 0, XUnitPt.FromInch(1.65), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, 0, XUnitPt.FromInch(1.65), XUnitPt.FromInch(7.32));
        }
        else
        {
            GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(1.65), XUnitPt.FromInch(9.92));
            GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(1.65), XUnitPt.FromInch(7.32));
        }

        GenerateRechargeBoxes(page, recharges, XUnitPt.FromInch(2.51), XUnitPt.FromInch(9.92));
        GenerateRechargePoolBoxes(page, pool, XUnitPt.FromInch(2.51), XUnitPt.FromInch(7.32));
    }

    private static string GetPowerPointTypeForExpressionName(BasicInfo basicInfo)
    {
        var powerType = string.Empty;
        if (basicInfo.Expression == "Adepts")
        {
            powerType = "Chi";
        }
        else if(basicInfo.Expression == "Aeternari")
        {
            powerType = "Vitality";
        }
        else if(basicInfo.Expression == "Shammas")
        {
            powerType = "Noumenon";
        }
        else if(basicInfo.Expression == "Sidhe")
        {
            powerType = "Essence";
        }
        else if(basicInfo.Expression == "Sorcerers")
        {
            powerType = "Mana";
        }
        else if(basicInfo.Expression == "Vampyres")
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
        for (int y = 0; y < 4; y++)
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