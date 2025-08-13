// See https://aka.ms/new-console-template for more information

using ExpressedRealms.Powers.Reporting.powerCards;
using QuestPDF.Fluent;

Console.WriteLine("Hello, World!");


var report = PowerCardReport.GenerateReport(new List<PowerCardData>());

report.GeneratePdf("../test.pdf");