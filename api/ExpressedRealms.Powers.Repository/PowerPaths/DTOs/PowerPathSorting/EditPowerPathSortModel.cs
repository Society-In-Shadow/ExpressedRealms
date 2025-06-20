namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathSorting;

public class EditPowerPathSortModel
{
    public int ExpressionId { get; set; }
    public List<EditPowerPathSortItemModel> Items { get; set; } = new();
}
