namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;

public record CommonDeleteHierarchyDto(int Id, int? ParentId, bool IsDeleted);
