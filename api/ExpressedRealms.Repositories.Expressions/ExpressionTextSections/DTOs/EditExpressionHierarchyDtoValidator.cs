using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class EditExpressionHierarchyDtoValidator : AbstractValidator<EditExpressionHierarchyDto>
{
    public EditExpressionHierarchyDtoValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.ExpressionId)
            .MustAsync(
                async (expressionId, cancellationToken) =>
                {
                    return await dbContext.Expressions.AnyAsync(
                        x => x.Id == expressionId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Expression Id");
        RuleFor(x => x)
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    var expressionSections = await dbContext
                        .ExpressionSections.Where(x => x.ExpressionId == dto.ExpressionId)
                        .ToListAsync();

                    return dto
                        .Items.Select(x => x.Id)
                        .OrderBy(id => id)
                        .SequenceEqual(expressionSections.Select(x => x.Id).OrderBy(id => id));
                }
            )
            .WithMessage(
                "There has been changes made to the expression that are not reflected in the UI. Please refresh the page and try again."
            );
        RuleFor(x => x)
            .Must(
                (dto, cancellationToken) =>
                {
                    var newParentIds = dto.Items.Select(x => x.ParentId).ToList();
                    var availableParentIds = dto.Items.Select(x => (int?)x.Id).ToList();

                    return !availableParentIds.Except(newParentIds).Any();
                }
            )
            .WithMessage("One or more Parent Id's are not available.");
        RuleFor(x => x)
            .Must((dto, cancellationToken) => HasCycles(dto.Items))
            .WithMessage("There is an infinite loop in the hierarchy.");
        RuleFor(x => x)
            .Must(
                (dto, cancellationToken) =>
                {
                    var groups = dto.Items.GroupBy(x => x.ParentId).ToList();

                    foreach (var group in groups)
                    {
                        var startingCount = 0;
                        foreach (var item in group.OrderBy(x => x.SortOrder))
                        {
                            if (item.SortOrder == startingCount)
                                return true;
                            startingCount++;
                        }
                    }

                    return false;
                }
            )
            .WithMessage("The sort order has gaps or duplicate values in one of the sections.");
    }

    private bool HasCycles(IEnumerable<EditExpressionHierarchyItemDto> nodes)
    {
        // Create dictionary mapping of Id to ParentId
        var nodeLookup = nodes.ToDictionary(x => x.Id, x => x.ParentId);

        // Function to detect a cycle starting from a given node
        bool DetectCycle(int id, HashSet<int> visited, HashSet<int> path)
        {
            if (path.Contains(id))
                return true; // Cycle detected
            if (!nodeLookup.ContainsKey(id) || nodeLookup[id] == null)
                return false; // Reached root

            path.Add(id); // Add current node to the path
            var parentId = nodeLookup[id]; // Get parent
            if (parentId.HasValue && DetectCycle(parentId.Value, visited, path))
                return true;
            path.Remove(id); // Remove node from path after traversal

            visited.Add(id); // Mark as fully processed
            return false;
        }

        // Use a visited set to avoid reprocessing nodes
        var visited = new HashSet<int>();
        foreach (var node in nodes)
        {
            if (!visited.Contains(node.Id))
            {
                var path = new HashSet<int>(); // Path specific to this node traversal
                if (DetectCycle(node.Id, visited, path))
                    return true; // Cycle found
            }
        }

        return false; // No cycles detected
    }
}
