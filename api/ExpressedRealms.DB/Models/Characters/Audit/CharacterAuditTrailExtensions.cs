using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Characters.Audit
{
    internal static class CharacterAuditTrailExtensions
    {
        public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
        {
            List<ChangedRecord> changedRecordsToReturn = new();
            foreach (var changedRecord in changedRecords)
            {
                switch (changedRecord.ColumnName)
                {
                    case "wealth_level":
                        changedRecord.FriendlyName = "Wealth Level";
                        break;

                    case "void_fragments":
                        changedRecord.FriendlyName = "Void Fragments";
                        break;

                    case "prima_fragments":
                        changedRecord.FriendlyName = "Prima Fragments";
                        break;
                    
                    case "motes":
                        changedRecord.FriendlyName = "Prima / Void";
                        break;
                    
                    // This is an opt in approach, not opt out - There's a lot of fields here
                    default:
                        continue;
                }

                changedRecordsToReturn.Add(changedRecord);
            }

            return changedRecordsToReturn;
        }

        public static IAuditEntityMapping AddCharacterAuditTrailMapping(this IAuditEntityMapping mapping)
        {
            return mapping.Map<Character, CharacterAuditTrail>(
                (blessing, audit) =>
                {
                    audit.CharacterId = blessing.Id;
                    return true;
                }
            );
        }
    }
}
