using Domain.Entities;
using FluentValidation;

public class TaskIdValidation : AbstractValidator<int>
{
    private readonly IEnumerable<TaskItem> _existingItems;
    private readonly bool _checkDuplicate;
    private readonly bool _checkExists;  

    public TaskIdValidation()
    {
        ApplyRules();
    }

    public static TaskIdValidation ForCreate(IEnumerable<TaskItem> existingItems)
        => new TaskIdValidation(existingItems, checkDuplicate: true);

    public static TaskIdValidation ForSearch(IEnumerable<TaskItem> existingItems)
        => new TaskIdValidation(existingItems, checkExists: true);

    private TaskIdValidation(
        IEnumerable<TaskItem> existingItems,
        bool checkDuplicate = false,
        bool checkExists = false)
    {
        _existingItems = existingItems;
        _checkDuplicate = checkDuplicate;
        _checkExists = checkExists;
        ApplyRules();
    }

    private void ApplyRules()
    {
        RuleFor(id => id)
            .GreaterThan(0).WithMessage("El ID debe ser mayor a 0.")
            .NotEqual(0).WithMessage("El ID no puede ser 0.");

        When(_ => _checkDuplicate, () =>
        {
            RuleFor(id => id)
                .Must(id => !_existingItems.Any(t => t.Id == id))
                .WithMessage(id => $"El ID {id} ya existe.");
        });

        When(_ => _checkExists, () =>
        {
            RuleFor(id => id)
                .Must(id => _existingItems.Any(t => t.Id == id))
                .WithMessage(id => $"El ID {id} no fue encontrado.");
        });
    }
}