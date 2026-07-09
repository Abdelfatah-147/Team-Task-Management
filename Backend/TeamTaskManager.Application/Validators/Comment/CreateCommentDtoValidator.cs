using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Application.DTOs.Comment;

namespace TeamTaskManager.Application.Validators.Comment
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.TaskId).NotEmpty();
        }
    }
}
