using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathLIst;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
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
    public async Task<Result<List<PowerPathInformation>>> GetPowerPathsAsync(int expressionId)
    {
        var items = await context
            .PowerPaths.Where(x => x.ExpressionId == expressionId)
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

    public async Task<Result<int>> CreatePowerPathAsync(CreatePowerPathModel createPowerModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            createPowerModelValidator,
            createPowerModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var newPowerPath = new PowerPath()
        {
            Name = createPowerModel.Name,
            Description = createPowerModel.Description,
            ExpressionId = createPowerModel.ExpressionId,
        };

        context.PowerPaths.Add(newPowerPath);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(newPowerPath.Id);
    }

    public async Task<Result> EditPowerPathAsync(EditPowerPathModel editPowerModel)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            editPowerModelValidator,
            editPowerModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var power = await context.PowerPaths.FirstAsync(
            x => x.Id == editPowerModel.Id,
            cancellationToken
        );

        power.Name = editPowerModel.Name;
        power.Description = editPowerModel.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeletePowerPathAsync(int id)
    {
        var section = await context
            .PowerPaths.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (section is null)
            return Result.Fail(new NotFoundFailure("Power Path"));

        if (section.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure("Power Path"));

        section.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
