using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AnswerQuestion;

[UsedImplicitly]
internal sealed class AnswerQuestionModelValidator : AbstractValidator<AnswerQuestionModel>
{
    public AnswerQuestionModelValidator(IEventCheckinRepository repository)
    {
        RuleFor(x => x.LookupId)
            .NotEmpty()
            .WithMessage("Lookup Id is required.")
            .Length(8)
            .WithMessage("Lookup Id must be 8 characters long.")
            .MustAsync(async (x, y) => await repository.CheckinIdExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Lookup Id does not exist.");

        RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question Id is required.");

        RuleFor(x => x.Response).NotEmpty().WithMessage("Question Id is required.");
    }
}
