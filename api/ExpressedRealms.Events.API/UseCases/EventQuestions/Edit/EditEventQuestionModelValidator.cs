using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Edit;

[UsedImplicitly]
internal sealed class EditEventQuestionModelValidator : AbstractValidator<EditEventQuestionModel>
{
    public EditEventQuestionModelValidator(
        IEventRepository repository,
        IEventQuestionRepository questionRepository
    )
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("Question is required.")
            .MaximumLength(500)
            .WithMessage("Question must be between 1 and 500 characters.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) =>
                    !await questionRepository.IsDuplicateEventQuestionQuestion(
                        x.EventId,
                        x.Id,
                        x.Question
                    )
            )
            .WithMessage("Question already exists.")
            .WithName(nameof(EditEventQuestionModel.Question));

        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) => await questionRepository.IsExistingEventQuestion(x.EventId, x.Id)
            )
            .WithErrorCode("NotFound")
            .WithMessage("Question does not exist.")
            .WithName(nameof(EditEventQuestionModel.Id));

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
