using Domain.DTOs;
using FluentValidation;

namespace BLL.Validations
{
    public class TaskItemDTOValidator : AbstractValidator<TaskItemDTO>
    {
        public TaskItemDTOValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(100).WithMessage("Máximo 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("Máximo 500 caracteres.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now).WithMessage("La fecha límite debe ser futura.");
        }
    }
}
