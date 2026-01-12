using FluentValidation;
using Ges_sco.API.DTOs.Requests;

namespace Ges_sco.API.Validators
{
    public class CreateGradeValidator : AbstractValidator<CreateGradeDto>
    {
        public CreateGradeValidator()
        {
            RuleFor(x => x.Value)
                .InclusiveBetween(0, 20).WithMessage("Grade must be between 0 and 20");

            RuleFor(x => x.Coefficient)
                .InclusiveBetween(0.1, 5).WithMessage("Coefficient must be between 0.1 and 5");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Grade type is required")
                .Must(type => new[] { "Exam", "CC", "TP", "TD", "Projet" }.Contains(type))
                .WithMessage("Type must be Exam, CC, TP, TD or Projet");

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Course is required");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Grade date cannot be in the future");
        }
    }

    public class UpdateGradeValidator : AbstractValidator<UpdateGradeDto>
    {
        public UpdateGradeValidator()
        {
            RuleFor(x => x.Value)
                .InclusiveBetween(0, 20).WithMessage("Grade must be between 0 and 20")
                .When(x => x.Value.HasValue);

            RuleFor(x => x.Coefficient)
                .InclusiveBetween(0.1, 5).WithMessage("Coefficient must be between 0.1 and 5")
                .When(x => x.Coefficient.HasValue);

            RuleFor(x => x.Type)
                .Must(type => new[] { "Exam", "CC", "TP", "TD", "Projet" }.Contains(type))
                .WithMessage("Type must be Exam, CC, TP, TD or Projet")
                .When(x => !string.IsNullOrEmpty(x.Type));

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Grade date cannot be in the future")
                .When(x => x.Date.HasValue);
        }
    }
}
