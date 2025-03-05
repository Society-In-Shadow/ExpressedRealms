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

                case nameof(User.AccessFailedCount):
                    // This catches when the user is initially created
                    if (
                        changedRecord.NewValue == "0"
                        && string.IsNullOrWhiteSpace(changedRecord.OriginalValue)
                    )
                        break;
                    changedRecord.FriendlyName = "Invalid Password Attempt";
                    changedRecord.Message = "Player entered an incorrect password";
                    changedRecordsToReturn.Add(changedRecord);
                    break;

                case nameof(User.LockoutEnd):
                    changedRecord.FriendlyName = "Player Status Update";

                    var successful = DateTimeOffset.TryParse(changedRecord.NewValue, out var date);

                    // User was more than likely just added / doesn't have issues
                    if (!successful)
                    {
                        break;
                    }

                    // Interesting side note, converting max value to a string and converting back
                    // will make it lose it's microsecond and millisecond value, thus making it not
                    // equal to Max Value after conversion.  Comparing year should be a good work around
                    if (date.Year == DateTimeOffset.MaxValue.Year)
                        changedRecord.Message = "Player was Disabled";
                    else if (date <= DateTimeOffset.UtcNow)
                        changedRecord.Message = "Player was Enabled";
                    else if (date > DateTimeOffset.UtcNow)
                        changedRecord.Message = "Player was Locked Out";

                    changedRecordsToReturn.Add(changedRecord);
                    break;
            }
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddUserAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<User, UserAuditTrail>(
            (user, audit) =>
            {
                audit.UserId = user.Id;
                return true;
            }
        );
    }
}
