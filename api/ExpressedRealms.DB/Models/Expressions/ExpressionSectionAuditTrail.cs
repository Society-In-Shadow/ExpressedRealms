namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSectionAuditTrail
{
    public int Id { get; set; }
    public int SectionId { get; set; }
    public int ExpressionId { get; set; }
    public string Action { get; set; }
    public string PropertyUpdated { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserName { get; set; }
    
    public virtual Expression Expression { get; set; }
    public virtual ExpressionSection ExpressionSection { get; set; }
    
}