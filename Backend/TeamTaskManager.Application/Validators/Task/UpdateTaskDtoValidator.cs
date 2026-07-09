using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.DTOs.Task;

namespace TeamTaskManager.Application.Validators.Task
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.Priority).IsInEnum();
            RuleFor(x => x.DueDate).GreaterThan(DateTime.UtcNow).When(x => x.DueDate.HasValue)
                .WithMessage("DueDate must be in the future");
        }
    }
}
