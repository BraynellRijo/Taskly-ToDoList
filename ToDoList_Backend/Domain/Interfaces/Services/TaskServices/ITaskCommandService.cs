using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces.Services.TaskServices
{
    public interface ITaskCommandService 
    {
        Task CreateTaskAsync(TaskItemDTO taskItemDTO);
        Task UpdateTaskAsync(int id, TaskItemDTO taskItemDTO);
        Task DeleteTaskAsync(int id);
    }
}
