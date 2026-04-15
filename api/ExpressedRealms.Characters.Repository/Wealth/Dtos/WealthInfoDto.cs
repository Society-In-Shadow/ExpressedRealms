namespace ExpressedRealms.Characters.Repository.Wealth.Dtos;

public class WealthInfoDto
{
    public int WealthLevel { get; set; }
    public double InitialBasicItemIncome { get; set; }
    public List<WealthTableRow> WealthTable { get; set; } = [];
    public List<KeyValuePair<string, string>> AppliedBlessings { get; set; } = [];
}
