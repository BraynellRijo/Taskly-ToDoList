using Domain.Entities;
using FluentValidation;

namespace BLL.Validations
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(t => t.Id)
                .NotNull();

            RuleFor(t => t.Title)
                .NotNull().WithMessage("El título no puede ser null.")
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El título no puede superar 100 caracteres.");

            RuleFor(t => t.Description)
                .NotNull().WithMessage("La descripción no puede ser null.")
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.");

            RuleFor(t => t.CreatedAt)
                .LessThanOrEqualTo(t => DateTime.Now).WithMessage("La fecha de creación no puede ser futura.");

            RuleFor(t => t.DueDate)
                .GreaterThan(t => DateTime.Now).WithMessage("La fecha límite debe ser futura.")
                .GreaterThan(t => t.CreatedAt).WithMessage("La fecha límite debe ser mayor a la fecha de creación.");


            RuleFor(t => t.IsCompleted)
                .Equal(false).WithMessage("Una tarea nueva no puede estar completada.");
        }
    }
}
