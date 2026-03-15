using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.TaskServices;
using Domain.Interfaces.Validations;

namespace BLL.Services.TaskServices
{
    public class TaskFilterService : ITaskFilterService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskValidation _taskValidator;

        public TaskFilterService(ITaskRepository taskRepository, ITaskValidation Validator)
        {
            _taskRepository = taskRepository;
            _taskValidator = Validator;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(bool isCompleted)
        {
            await _taskValidator.ValidateNotEmptyAsync();

            return isCompleted ?
                await _taskRepository.GetCompletedTasks() :
                await _taskRepository.GetDueTasks();
        }

        public async Task<IEnumerable<TaskItem>> GetCompletedTasksAsync()
        {
            await _taskValidator.ValidateNotEmptyAsync();
            return await _taskRepository.GetCompletedTasks();
        }
        public async Task<IEnumerable<TaskItem>> GetDueTasksAsync()
        {
            await _taskValidator.ValidateNotEmptyAsync();
            return await _taskRepository.GetDueTasks();
        }
    }
}
