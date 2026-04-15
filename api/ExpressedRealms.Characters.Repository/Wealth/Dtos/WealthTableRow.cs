namespace ExpressedRealms.Characters.Repository.Wealth.Dtos;

public class WealthTableRow
{
    public int Level { get; set; }
    public double SessionIncome { get; set; }
    public double CashToLevelUp { get; set; }
    public double LiquidationValue { get; set; }
    public bool IsCurrentLevel { get; set; }
}