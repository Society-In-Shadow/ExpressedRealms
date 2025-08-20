using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathLIst;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathSorting;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathToC;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.PowerPaths;

internal sealed class PowerPathRepository(
    ExpressedRealmsDbContext context,
    CreatePowerPathModelValidator createPowerModelValidator,
    EditPowerPathModelValidator editPowerModelValidator,
    CancellationToken cancellationToken
) : IPowerPathRepository
{
    public async Task<Result<List<PowerPathToc>>> GetPowerPathAndPowers(int expressionId)
    {
        var items = await context
            .PowerPaths.Where(x => x.ExpressionId == expressionId)
            .OrderBy(x => x.OrderIndex)
            .Select(x => new PowerPathToc()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Powers = x
                    .Powers.AsQueryable()
                    .OrderBy(x => x.OrderIndex)
                    .Select(PowerInformation.Selector)
                    .ToList(),
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(items);
    }

    public async Task<Result<List<PowerPathToc>>> GetPowerPathAndPowers(List<int> powerIds)
    {
        var items = await context
            .PowerPaths.Where(x => x.Powers.Any(y => powerIds.Contains(y.Id)))
            .OrderBy(x => x.OrderIndex)
            .Select(x => new PowerPathToc()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Powers = x
                    .Powers.Where(x => powerIds.Contains(x.Id))
                    .OrderBy(x => x.OrderIndex)
                    .AsQueryable()
                    .Select(PowerInformation.Selector)
                    .ToList(),
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(items);
    }

    public async Task<Result<List<PowerPathInformation>>> GetPowerPathsAsync(int expressionId)
    {
        var items = await context
            .PowerPaths.Where(x => x.ExpressionId == expressionId)
            .OrderBy(x => x.OrderIndex)
            .Select(x => new PowerPathInformation
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(items);
    }

    public async Task<Result<EditPowerPathInformation>> GetPowerPathAsync(int powerPathId)
    {
        var power = await context
            .PowerPaths.Where(x => x.Id == powerPathId)
            .Select(x => new EditPowerPathInformation
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (power is null)
            return Result.Fail(new NotFoundFailure(nameof(PowerPath)));

        return Result.Ok(power);
    }

    public async Task<Result<int>> CreatePowerPathAsync(CreatePowerPathModel createPowerPathModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            createPowerModelValidator,
            createPowerPathModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var nextPlaceOnList = await context
            .PowerPaths.AsNoTracking()
            .Where(x => x.ExpressionId == createPowerPathModel.ExpressionId)
            .CountAsync();

        var newPowerPath = new PowerPath()
        {
            Name = createPowerPathModel.Name,
            Description = createPowerPathModel.Description,
            ExpressionId = createPowerPathModel.ExpressionId,
            OrderIndex = nextPlaceOnList + 1,
        };

        context.PowerPaths.Add(newPowerPath);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(newPowerPath.Id);
    }

    public async Task<Result> EditPowerPathAsync(EditPowerPathModel editPowerPathModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            editPowerModelValidator,
            editPowerPathModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var power = await context.PowerPaths.FirstAsync(
            x => x.Id == editPowerPathModel.Id,
            cancellationToken
        );

        power.Name = editPowerPathModel.Name;
        power.Description = editPowerPathModel.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeletePowerPathAsync(int powerPathId)
    {
        var section = await context
            .PowerPaths.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == powerPathId);

        if (section is null)
            return Result.Fail(new NotFoundFailure("Power Path"));

        if (section.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure("Power Path"));

        section.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> UpdatePowerPathSortOrder(EditPowerPathSortModel dto)
    {
        var sections = await context
            .PowerPaths.Where(x => x.ExpressionId == dto.ExpressionId)
            .ToListAsync();

        foreach (var item in dto.Items)
        {
            var section = sections.First(x => x.Id == item.Id);
            section.OrderIndex = item.SortOrder;
        }

        await context.SaveChangesAsync();
        return Result.Ok();
    }
}
