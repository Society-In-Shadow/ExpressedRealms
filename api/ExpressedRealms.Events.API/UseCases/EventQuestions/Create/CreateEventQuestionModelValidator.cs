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
            .WithMessage("Question is required.");
        
        RuleFor(x => x)
            .MustAsync(async (x, y) => !await questionRepository.IsExistingEventQuestion(x.EventId, x.Question))
            .WithMessage("Question already exists.");

        RuleFor(x => x.QuestionTypeId)
            .MustAsync(async (x, y) => await questionRepository.IsExistingCustomizableQuestionType(x))
            .WithMessage("Question Type does not exist.");
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingEvent(x))
            .WithMessage("Event does not exist.");
    }
}
