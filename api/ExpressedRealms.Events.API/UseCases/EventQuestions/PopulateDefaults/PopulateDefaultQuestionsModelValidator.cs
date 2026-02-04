using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;

[UsedImplicitly]
internal sealed class PopulateDefaultQuestionsModelValidator
    : AbstractValidator<PopulateDefaultQuestionsModel>
{
    public PopulateDefaultQuestionsModelValidator(
        IEventRepository repository,
        IEventQuestionRepository questionRepository
    )
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
