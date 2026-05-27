namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetCharacterGoFields;

internal record GetCharacterGoFieldsResponse
{
    public int WealthLevel { get; set; }
    public int VoidFragments { get; set; }
    public int Motes { get; set; }
    public int PrimaFragments { get; set; }
}
