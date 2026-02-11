using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup.Audit;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public ICollection<Checkin> Checkins = new HashSet<Checkin>();
    public ICollection<CheckinAuditTrail> CheckinAuditTrails = new HashSet<CheckinAuditTrail>();
}
