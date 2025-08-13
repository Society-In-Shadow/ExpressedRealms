// See https://aka.ms/new-console-template for more information

using Bogus;
using ExpressedRealms.Powers.Reporting.powerCards;
using QuestPDF.Fluent;

Console.WriteLine("Hello, World!");


var categories = new[] { "Offense", "Defense", "Utility", "Support", "Control", "Movement" };
var durations = new[] { "Instant", "Sustained", "Encounter", "Daily" };
var areas = new[] { "Self", "Single Target", "Cone", "Sphere", "Burst" };
var levels = new[] { "Basic", "Intermediate", "Advanced", "Supreme" };
var activationTypes = new[] { "Passive", "Active", "Triggered", "Channelled" };

// If you have a PrerequisiteData class, you can create a separate faker for it:
// var prereqFaker = new Faker<PrerequisiteData>()
//   // .RuleFor(x => x.SomeProp, f => ...)
//   ;

var powerCardFaker = new Faker<PowerCardData>()
    .RuleFor(p => p.ExpressionName, f => f.Hacker.Noun())
    .RuleFor(p => p.PathName, f => f.Commerce.Department())
    .RuleFor(p => p.Id, f => f.IndexFaker + 1)
    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
    .RuleFor(p => p.Category, f => f.PickRandom(categories, f.Random.Int(1, 3)).Distinct().ToList())
    .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
    .RuleFor(p => p.GameMechanicEffect, f => f.Lorem.Sentence())
    .RuleFor(p => p.Limitation, f => f.Random.Bool(0.3f) ? f.Lorem.Sentence() : null)
    .RuleFor(p => p.PowerDuration, f => f.PickRandom(durations))
    .RuleFor(p => p.AreaOfEffect, f => f.PickRandom(areas))
    .RuleFor(p => p.PowerLevel, f => f.PickRandom(levels))
    .RuleFor(p => p.PowerActivationType, f => f.PickRandom(activationTypes))
    .RuleFor(p => p.Other, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence() : null)
    .RuleFor(p => p.IsPowerUse, f => f.Random.Bool())
    .RuleFor(p => p.Cost, f => f.Random.Bool(0.5f) ? $"{f.Random.Int(1, 5)} FP" : null)
    .RuleFor(p => p.PrerequisitesNeeded, f => f.Random.Int(0, 2))
    // If you have a prereq faker, you could do:
    // .RuleFor(p => p.Prerequisites, f => f.Random.Bool() ? prereqFaker.Generate() : null)
    .RuleFor(p => p.Prerequisites, f => null);

// Generate a list of fake cards
var cards = powerCardFaker.Generate(25);



var report = PowerCardReport.GenerateReport(cards);



report.GeneratePdf("../test.pdf");