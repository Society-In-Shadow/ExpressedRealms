namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;

public class EditPowerSortModel
{
    public int PowerPathId { get; set; }
    public List<EditPowerSortItemModel> Items { get; set; } = new();
}
