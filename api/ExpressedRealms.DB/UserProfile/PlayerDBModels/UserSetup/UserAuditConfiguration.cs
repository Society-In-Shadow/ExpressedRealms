using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

internal static class UserAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case nameof(User.Email):
                    changedRecordsToReturn.Add(changedRecord);
                    break;
                case nameof(User.PasswordHash):
                    changedRecord.Message = "Changed their password.";
                    changedRecord.NewValue = null;
                    changedRecord.OriginalValue = null;
                    changedRecordsToReturn.Add(changedRecord);
                    break;
            }
            
        }
        
        return changedRecordsToReturn;
    }
    
    public static IAuditEntityMapping AddUserAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<User, UserAuditTrail>();
    }
}