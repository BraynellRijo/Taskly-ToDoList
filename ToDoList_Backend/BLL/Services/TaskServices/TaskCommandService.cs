using AutoMapper;
using BLL.Validations;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.TaskServices;
using Domain.Interfaces.Validations;


namespace BLL.Services.TaskServices
{
    public class TaskCommandService : ITaskCommandService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ITaskValidation _taskValidation;

        public TaskCommandService(ITaskRepository taskRepository, 
            IMapper mapper,
            ITaskValidation taskValidation)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _taskValidation = taskValidation;
        }

        public async Task CreateTaskAsync(TaskItemDTO taskItemDTO)
        {
            await _taskValidation.ValidateDTOAsync(taskItemDTO);

            var item = _mapper.Map<TaskItem>(taskItemDTO);
            item.Id = await GetNextId();
            item.CreatedAt = DateTime.Now;

            await _taskRepository.CreateAsync(item);
        }

        public async Task UpdateTaskAsync(int id, TaskItemDTO taskItemDTO)
        {
            await _taskValidation.ValidateIdForSearch(id);
            await _taskValidation.ValidateDTOAsync(taskItemDTO);

            var item = _mapper.Map<TaskItem>(taskItemDTO);
            await _taskRepository.UpdateAsync(id, item);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskValidation.ValidateIdForSearch(id);
            await _taskRepository.DeleteAsync(id);
        }

        private async Task<int> GetNextId()
        {
            var tasks = (await _taskRepository.GetAllAsync()).ToList();
            return tasks.Any() ?
                tasks.Max(t => t.Id) + 1
                : 1;
        }
    }
}
