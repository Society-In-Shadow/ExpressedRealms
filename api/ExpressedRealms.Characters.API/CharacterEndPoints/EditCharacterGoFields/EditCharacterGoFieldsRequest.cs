namespace ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacterGoFields;

internal record EditCharacterGoFieldsRequest
{
    public int WealthLevel { get; set; }
    public int VoidFragments { get; set; }
    public int Motes { get; set; }
    public int PrimaFragments { get; set; }
}
