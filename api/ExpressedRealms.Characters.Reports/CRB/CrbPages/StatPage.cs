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

        HandleAdminFields(fields, dataProficiencyInfo);

        PrintProficiencyStats(dataProficiencyInfo, expression, page);
        PrintOffensiveStats(dataProficiencyInfo, page);
        PrintDefensiveStats(dataProficiencyInfo, page);
        PrintMovement(dataProficiencyInfo, page);
    }

    private static void PrintMovement(ProficiencyData dataProficiencyInfo, PdfPage page)
    {
        var walkingOffset = XUnitPt.FromInch(3.52);

        var offensiveOffset = XUnitPt.FromInch(8.72);
        var pacesOffset = XUnitPt.FromInch(8.15);
        var defensiveOffset = XUnitPt.FromInch(7.56);

        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(-1 + dataProficiencyInfo.WalkingOffensiveProficiencies, 0).ToString(),
            walkingOffset,
            offensiveOffset
        );

        var minWalkingPaces = 1;
        var maxWalkingPaces = minWalkingPaces + dataProficiencyInfo.WalkingPaces + 1;
        var walkingPaceString =
            minWalkingPaces == maxWalkingPaces
                ? minWalkingPaces.ToString()
                : $"{minWalkingPaces} - {maxWalkingPaces}";

        TextPrintUtilities.PrintStatInfo(page, walkingPaceString, walkingOffset, pacesOffset);

        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(0 + dataProficiencyInfo.WalkingDefensiveProficiencies, 0).ToString(),
            walkingOffset,
            defensiveOffset
        );

        var runningOffset = XUnitPt.FromInch(3.69);
        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(-3 + dataProficiencyInfo.RunningOffensiveProficiencies, 0).ToString(),
            runningOffset,
            offensiveOffset
        );

        var minRunningPaces = maxWalkingPaces + 1;
        var maxRunningPaces = minRunningPaces + dataProficiencyInfo.RunningPaces + 1;
        var runningPaceString =
            minRunningPaces == maxRunningPaces
                ? minRunningPaces.ToString()
                : $"{minRunningPaces} - {maxRunningPaces}";

        TextPrintUtilities.PrintStatInfo(page, runningPaceString, runningOffset, pacesOffset);

        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(0 + dataProficiencyInfo.RunningDefensiveProficiencies, 0).ToString(),
            runningOffset,
            defensiveOffset
        );

        var sprintingOffset = XUnitPt.FromInch(3.85);
        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(-6 + dataProficiencyInfo.SprintingOffensiveProficiencies, 0).ToString(),
            sprintingOffset,
            offensiveOffset
        );

        var minSprintingPaces = maxRunningPaces + 1;
        var maxSprintingPaces = minSprintingPaces + dataProficiencyInfo.SprintingPaces;

        var sprintingPaceString =
            minSprintingPaces == maxSprintingPaces
                ? minSprintingPaces.ToString()
                : $"{minSprintingPaces} - {maxSprintingPaces}";

        TextPrintUtilities.PrintStatInfo(page, sprintingPaceString, sprintingOffset, pacesOffset);

        TextPrintUtilities.PrintStatInfo(
            page,
            Math.Min(-3 + dataProficiencyInfo.SprintingDefensiveProficiencies, 0).ToString(),
            sprintingOffset,
            defensiveOffset
        );
    }

    private static void PrintDefensiveStats(ProficiencyData dataProficiencyInfo, PdfPage page)
    {
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
    }

    private static void PrintOffensiveStats(ProficiencyData dataProficiencyInfo, PdfPage page)
    {
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
    }

    private static void PrintProficiencyStats(
        ProficiencyData dataProficiencyInfo,
        string expression,
        PdfPage page
    )
    {
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
    }

    private static void FillInStatInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        StatModifierInfo dataStatInfo,
        PdfDocument document
    )
    {
        var page = document.Pages[5];
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Agility.Stat.ToString(),
            XUnitPt.FromInch(2.20),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Agility.Bonus.ToString(),
            XUnitPt.FromInch(2.20),
            XUnitPt.FromInch(9.8)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Constitution.Stat.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Constitution.Bonus.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(9.8)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Dexterity.Stat.ToString(),
            XUnitPt.FromInch(2.80),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Dexterity.Bonus.ToString(),
            XUnitPt.FromInch(2.80),
            XUnitPt.FromInch(9.8)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Intelligence.Stat.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Intelligence.Bonus.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(9.8)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Strength.Stat.ToString(),
            XUnitPt.FromInch(3.40),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Strength.Bonus.ToString(),
            XUnitPt.FromInch(3.40),
            XUnitPt.FromInch(9.8)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Willpower.Stat.ToString(),
            XUnitPt.FromInch(3.70),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Willpower.Bonus.ShowPlusMinusSigns(),
            XUnitPt.FromInch(3.70),
            XUnitPt.FromInch(9.8)
        );

        Helpers.MergeField(fields, "AglStat", dataStatInfo.Agility.Stat.ToString());
        Helpers.MergeField(fields, "StrStat", dataStatInfo.Strength.Stat.ToString());
        Helpers.MergeField(fields, "ConStat", dataStatInfo.Constitution.Stat.ToString());
        Helpers.MergeField(fields, "DexStat", dataStatInfo.Dexterity.Stat.ToString());
        Helpers.MergeField(fields, "IntStat", dataStatInfo.Intelligence.Stat.ToString());
        Helpers.MergeField(fields, "WilStat", dataStatInfo.Willpower.Stat.ToString());

        Helpers.MergeField(fields, "AglBonus", dataStatInfo.Agility.Bonus.ToString());
        Helpers.MergeField(fields, "StrBonus", dataStatInfo.Strength.Bonus.ToString());
        Helpers.MergeField(fields, "ConBonus", dataStatInfo.Constitution.Bonus.ToString());
        Helpers.MergeField(fields, "DexBonus", dataStatInfo.Dexterity.Bonus.ToString());
        Helpers.MergeField(fields, "IntBonus", dataStatInfo.Intelligence.Bonus.ToString());
        Helpers.MergeField(fields, "WilBonus", dataStatInfo.Willpower.Bonus.ToString());
    }

    private static void HandleAdminFields(
        PdfAcroField.PdfAcroFieldCollection fields,
        ProficiencyData dataProficiencyInfo
    )
    {
        var powerPoints = new List<int>()
        {
            dataProficiencyInfo.Chi,
            dataProficiencyInfo.Essence,
            dataProficiencyInfo.Mana,
            dataProficiencyInfo.Noumenon,
        };

        // Secondary
        Helpers.MergeField(fields, "Vitality", dataProficiencyInfo.Vitality.ToString());
        Helpers.MergeField(fields, "Health", dataProficiencyInfo.Health.ToString());
        Helpers.MergeField(fields, "Blood", dataProficiencyInfo.Blood.ToString());
        Helpers.MergeField(fields, "Reaction", dataProficiencyInfo.Reaction.ToString());
        Helpers.MergeField(fields, "Psyche", dataProficiencyInfo.Psyche.ToString());
        Helpers.MergeField(fields, "RWP", dataProficiencyInfo.RWP.ToString());
        Helpers.MergeField(fields, "Mortis", dataProficiencyInfo.Mortis.ToString());

        Helpers.MergeField(fields, "PowerPoints", powerPoints.Max().ToString());

        // Offensive
        Helpers.MergeField(fields, "Strike", dataProficiencyInfo.Strike.ToString());
        Helpers.MergeField(fields, "Thrust", dataProficiencyInfo.Thrust.ToString());
        Helpers.MergeField(fields, "Throw", dataProficiencyInfo.Throw.ToString());
        Helpers.MergeField(fields, "Shoot", dataProficiencyInfo.Shoot.ToString());
        Helpers.MergeField(fields, "Cast", dataProficiencyInfo.Cast.ToString());
        Helpers.MergeField(fields, "Project", dataProficiencyInfo.Project.ToString());

        // Defensive
        Helpers.MergeField(fields, "Dodge", dataProficiencyInfo.Dodge.ToString());
        Helpers.MergeField(fields, "Parry", dataProficiencyInfo.Parry.ToString());
        Helpers.MergeField(fields, "ThrowEvade", dataProficiencyInfo.EvadeThrow.ToString());
        Helpers.MergeField(fields, "ShootEvade", dataProficiencyInfo.EvadeShoot.ToString());
        Helpers.MergeField(fields, "Ward", dataProficiencyInfo.Ward.ToString());
        Helpers.MergeField(fields, "Deflect", dataProficiencyInfo.Deflect.ToString());
    }
}
