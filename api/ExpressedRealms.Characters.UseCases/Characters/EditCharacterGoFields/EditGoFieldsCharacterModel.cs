namespace ExpressedRealms.Characters.UseCases.Characters.EditCharacterGoFields;

public sealed record EditCharacterGoFieldsModel
{
    public int Id { get; set; }

    public int WealthLevel { get; set; }
    public int VoidFragments { get; set; }
    public int Motes { get; set; }
    public int PrimaFragments { get; set; }
}
