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
                    changedRecord.ColumnName = "Password";
                    changedRecord.Message = "Changed their password.";
                    changedRecord.NewValue = null;
                    changedRecord.OriginalValue = null;
                    changedRecordsToReturn.Add(changedRecord);
                    break;
                case nameof(User.EmailConfirmed):
                    // This doesn't seem to reset itself on email change,
                    // Instead it does a whole update
                    changedRecord.FriendlyName = "Confirmed Email";
                    if (changedRecord.NewValue.ToLowerInvariant() == "true")
                        changedRecord.Message = "Confirmed their email.";
                    if (changedRecord.NewValue.ToLowerInvariant() == "false")
                        changedRecord.Message = "Need to reconfirm their new email address.";
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
