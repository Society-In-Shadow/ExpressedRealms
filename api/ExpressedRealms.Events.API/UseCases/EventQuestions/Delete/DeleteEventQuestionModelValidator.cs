using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Delete;

[UsedImplicitly]
internal sealed class DeleteEventQuestionModelValidator
    : AbstractValidator<DeleteEventQuestionModel>
{
    public DeleteEventQuestionModelValidator(
        IEventRepository repository,
        IEventQuestionRepository questionRepository
    )
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) => await questionRepository.IsExistingEventQuestion(x.EventId, x.Id)
            )
            .WithErrorCode("NotFound")
            .WithMessage("Question does not exist.")
            .WithName(nameof(DeleteEventQuestionModel.Id));

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
