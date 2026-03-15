using Domain.Entities;

namespace Domain.Interfaces.Services.TaskServices
{
    public interface ITaskFilterService
    {
        Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(bool isCompleted);
        Task<IEnumerable<TaskItem>> GetCompletedTasksAsync();
        Task<IEnumerable<TaskItem>> GetDueTasksAsync();
    }
}
