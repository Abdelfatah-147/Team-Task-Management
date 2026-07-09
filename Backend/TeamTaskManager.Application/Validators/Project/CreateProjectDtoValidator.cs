using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.DTOs.Project;

namespace TeamTaskManager.Application.Validators.Project
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Description).MaximumLength(700);
            RuleFor(x => x.TeamId).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("End-Date must be greater than Start-Date");
        }
    }
}
