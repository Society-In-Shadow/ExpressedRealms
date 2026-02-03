using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Create;

[UsedImplicitly]
internal sealed class CreateEventQuestionModelValidator
    : AbstractValidator<CreateEventQuestionModel>
{
    public CreateEventQuestionModelValidator(IEventRepository repository, IEventQuestionRepository questionRepository)
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("Question is required.")
            .MaximumLength(500)
            .WithMessage("Question must be between 1 and 500 characters.");
        
        RuleFor(x => x)
            .MustAsync(async (x, y) => !await questionRepository.IsExistingEventQuestion(x.EventId, x.Question))
            .WithMessage("Question already exists.")
            .WithName(nameof(CreateEventQuestionModel.Question));

        RuleFor(x => x.QuestionTypeId)
            .NotEmpty()
            .WithMessage("Question Type is required.")
            .MustAsync(async (x, y) => await questionRepository.IsExistingCustomizableQuestionType(x))
            .WithMessage("Question Type does not exist.")
            .WithName(nameof(CreateEventQuestionModel.QuestionTypeId));
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithErrorCode("NotFound")
            .WithMessage("Event does not exist.");
    }
}
