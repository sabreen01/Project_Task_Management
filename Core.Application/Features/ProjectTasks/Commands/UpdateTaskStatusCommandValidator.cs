using FluentValidation;

namespace Core.Application.Features.ProjectTasks.Commands;

public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task Id is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid task status value.");
    }
}
