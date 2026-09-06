namespace ExpressedRealms.Characters.UseCases.Reports.GetCRB
{
    public class GetCharacterBookletModel
    {
        public int CharacterId { get; set; }
        public bool UseLatestApproved { get; set; }
    }
}
