using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup.Audit;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Checkin> Checkins { get; set; }
    public DbSet<CheckinAuditTrail> CheckinAuditTrails { get; set; }
    public DbSet<CheckinQuestionResponse> CheckinQuestionResponses { get; set; }
    public DbSet<CheckinStage> CheckinStages { get; set; }
}
