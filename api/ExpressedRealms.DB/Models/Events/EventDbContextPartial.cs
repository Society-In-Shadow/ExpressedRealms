using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<EventScheduleItem> EventScheduleItems { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<EventQuestion> EventQuestions { get; set; }
}
