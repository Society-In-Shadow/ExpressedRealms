using HTMLQuestPDF;
using QuestPDF.Fluent;

namespace ExpressedRealms.Shared.Reports;

public static class SetStandardHtmlFormatting
{
    public static void StandardHtmlFormatting(this HTMLDescriptor html)
    {
        html.SetContainerStyleForHtmlElement("ul", x => x.Padding(0));
        html.SetContainerStyleForHtmlElement("ol", x => x.Padding(0));
        html.SetContainerStyleForHtmlElement("li", x => x.Padding(0));
        html.SetContainerStyleForHtmlElement("p", x => x.Padding(0).PaddingBottom(3));   
    }
}