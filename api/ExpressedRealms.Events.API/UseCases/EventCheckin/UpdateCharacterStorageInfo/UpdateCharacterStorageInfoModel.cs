namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCharacterStorageInfo;

public class UpdateCharacterStorageInfoModel
{
    public required string LookupId { get; set; }
    public bool OptedIn { get; set; }
}
