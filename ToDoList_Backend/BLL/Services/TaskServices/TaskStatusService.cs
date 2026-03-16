using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.TaskServices;
using Domain.Interfaces.Validations;

namespace BLL.Services.TaskServices
{
    public class TaskStatusService : ITaskStatusService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskValidation _taskValidation;

        public TaskStatusService(ITaskRepository taskRepository,
            ITaskValidation taskValidation)
        {
            _taskRepository = taskRepository;
            _taskValidation = taskValidation;
        }

        public async Task MarkAsCompleted(int id)
        {
            await _taskValidation.ValidateIdForSearch(id);
            await _taskRepository.MarkTaskAsCompleted(id);
        }
    }
}
