using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.TaskServices;
using Domain.Interfaces.Validations;

namespace BLL.Services.TaskServices
{
    public class TaskQueryService : ITaskQueryService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskValidation _taskValidator;

        public TaskQueryService(ITaskRepository taskRepository, ITaskValidation Validator)
        {
            _taskRepository = taskRepository;
            _taskValidator = Validator;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Any() ? tasks : [];
        }

        public async Task<TaskItem> GetTaskAsync(int id)
        {
            await _taskValidator.ValidateIdForSearch(id);

            var task = await _taskRepository.GetValueAsync(id);
            return task;
        }
    }
}
