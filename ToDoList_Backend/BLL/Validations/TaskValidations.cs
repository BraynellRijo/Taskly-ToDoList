using Domain.DTOs;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validations;
using FluentValidation;

namespace BLL.Validations
{
    public class TaskValidations : ITaskValidation
    {
        private readonly IValidator<TaskItemDTO> _taskDTOValidator;
        private readonly ITaskRepository _taskRepository;

        public TaskValidations(IValidator<TaskItemDTO> validatorDTO,
            IValidator<int> idValidator,
            ITaskRepository taskRepository)
        {
            _taskDTOValidator = validatorDTO;
            _taskRepository = taskRepository;
        }

        public async Task ValidateIdForCreate(int id)
        {
            var tasks = await _taskRepository.GetAllAsync();
            var result = TaskIdValidation.ForCreate(tasks).Validate(id);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public async Task ValidateIdForSearch(int id)
        {
            var tasks = await _taskRepository.GetAllAsync();
            var result = TaskIdValidation.ForSearch(tasks).Validate(id);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public async Task ValidateDTOAsync(TaskItemDTO dto)
        {
            var result = await _taskDTOValidator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public async Task ValidateIdAndDTOAsync(int id, TaskItemDTO dto)
        {
            await ValidateIdForSearch(id);
            await ValidateDTOAsync(dto);
        }

        public async Task ValidateNotEmptyAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            if (!tasks.Any())
                throw new InvalidOperationException("There are no tasks registered.");
        }
    }
}
