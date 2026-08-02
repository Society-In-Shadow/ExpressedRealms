using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;

namespace ExpressedRealms.Characters.Reports.CRB.CrbPages;

internal static class StatPage
{
    internal static void FillInProficiencies(
        PdfAcroField.PdfAcroFieldCollection fields,
        ProficiencyData dataProficiencyInfo,
        PdfDocument document,
        string expression
    )
    {
        var page = document.Pages[5];

        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Vitality.ToString(),
            XUnitPt.FromInch(2.25),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Health.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Blood.ToString(),
            XUnitPt.FromInch(2.75),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.RWP.ToString(),
            XUnitPt.FromInch(3.00),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Reaction.ToString(),
            XUnitPt.FromInch(3.26),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Psyche.ToString(),
            XUnitPt.FromInch(3.53),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Mortis.ToString(),
            XUnitPt.FromInch(3.80),
            XUnitPt.FromInch(4.60)
        );

        Helpers.MergeField(fields, "Vitality", dataProficiencyInfo.Vitality.ToString());
        Helpers.MergeField(fields, "Health", dataProficiencyInfo.Health.ToString());
        Helpers.MergeField(fields, "Blood", dataProficiencyInfo.Blood.ToString());
        Helpers.MergeField(fields, "Reaction", dataProficiencyInfo.Reaction.ToString());
        Helpers.MergeField(fields, "Psyche", dataProficiencyInfo.Psyche.ToString());
        Helpers.MergeField(fields, "RWP", dataProficiencyInfo.RWP.ToString());
        Helpers.MergeField(fields, "Mortis", dataProficiencyInfo.Mortis.ToString());

        switch (expression)
        {
            case "Adepts":
                TextPrintUtilities.PrintStatLabelInfo(
                    page,
                    "Chi",
                    XUnitPt.FromInch(4.12),
                    XUnitPt.FromInch(5.48)
                );
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(4.06),
                    XUnitPt.FromInch(5.68)
                );
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Chi.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Shammas":
                TextPrintUtilities.PrintStatLabelInfo(
                    page,
                    "Noumenon",
                    XUnitPt.FromInch(4.12),
                    XUnitPt.FromInch(5.48)
                );
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(4.06),
                    XUnitPt.FromInch(5.68)
                );
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Noumenon.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Sorcerers":
                TextPrintUtilities.PrintStatLabelInfo(
                    page,
                    "Mana",
                    XUnitPt.FromInch(4.12),
                    XUnitPt.FromInch(5.48)
                );
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(4.06),
                    XUnitPt.FromInch(5.68)
                );
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Mana.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Sidhe":
                TextPrintUtilities.PrintStatLabelInfo(
                    page,
                    "Essence",
                    XUnitPt.FromInch(4.12),
                    XUnitPt.FromInch(5.48)
                );
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(4.06),
                    XUnitPt.FromInch(5.68)
                );
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Essence.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Aeternari":
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(2.25),
                    XUnitPt.FromInch(5.68)
                );
                break;
            case "Vampyres":
                TextPrintUtilities.PrintPPIdentifier(
                    page,
                    XUnitPt.FromInch(2.76),
                    XUnitPt.FromInch(5.68)
                );
                break;
        }

        var powerPoints = new List<int>()
        {
            dataProficiencyInfo.Chi,
            dataProficiencyInfo.Essence,
            dataProficiencyInfo.Mana,
            dataProficiencyInfo.Noumenon,
        };

        Helpers.MergeField(fields, "PowerPoints", powerPoints.Max().ToString());

        var offensiveCenterOffset = XUnitPt.FromInch(8.72);
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Strike.ToString(),
            XUnitPt.FromInch(2.14),
            offensiveCenterOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Thrust.ToString(),
            XUnitPt.FromInch(2.34),
            offensiveCenterOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Throw.ToString(),
            XUnitPt.FromInch(2.54),
            offensiveCenterOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Shoot.ToString(),
            XUnitPt.FromInch(2.75),
            offensiveCenterOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Cast.ToString(),
            XUnitPt.FromInch(2.95),
            offensiveCenterOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Project.ToString(),
            XUnitPt.FromInch(3.14),
            offensiveCenterOffset
        );

        Helpers.MergeField(fields, "Strike", dataProficiencyInfo.Strike.ToString());
        Helpers.MergeField(fields, "Thrust", dataProficiencyInfo.Thrust.ToString());
        Helpers.MergeField(fields, "Throw", dataProficiencyInfo.Throw.ToString());
        Helpers.MergeField(fields, "Shoot", dataProficiencyInfo.Shoot.ToString());
        Helpers.MergeField(fields, "Cast", dataProficiencyInfo.Cast.ToString());
        Helpers.MergeField(fields, "Project", dataProficiencyInfo.Project.ToString());

        var defensiveOffset = XUnitPt.FromInch(7.55);
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Dodge.ToString(),
            XUnitPt.FromInch(2.14),
            defensiveOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Parry.ToString(),
            XUnitPt.FromInch(2.34),
            defensiveOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.EvadeThrow.ToString(),
            XUnitPt.FromInch(2.54),
            defensiveOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.EvadeShoot.ToString(),
            XUnitPt.FromInch(2.75),
            defensiveOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Ward.ToString(),
            XUnitPt.FromInch(2.95),
            defensiveOffset
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Deflect.ToString(),
            XUnitPt.FromInch(3.14),
            defensiveOffset
        );

        Helpers.MergeField(fields, "Dodge", dataProficiencyInfo.Dodge.ToString());
        Helpers.MergeField(fields, "Parry", dataProficiencyInfo.Parry.ToString());
        Helpers.MergeField(fields, "ThrowEvade", dataProficiencyInfo.EvadeThrow.ToString());
        Helpers.MergeField(fields, "ShootEvade", dataProficiencyInfo.EvadeShoot.ToString());
        Helpers.MergeField(fields, "Ward", dataProficiencyInfo.Ward.ToString());
        Helpers.MergeField(fields, "Deflect", dataProficiencyInfo.Deflect.ToString());
    }
}