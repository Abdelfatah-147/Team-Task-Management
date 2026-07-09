using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.DTOs.Team;

namespace TeamTaskManager.Application.Validators.Team
{
    public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
    {
        public UpdateTeamDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Description).MaximumLength(700);
        }
    }
}
