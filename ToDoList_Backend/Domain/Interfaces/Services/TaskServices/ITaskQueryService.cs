using Domain.Entities;

namespace Domain.Interfaces.Services.TaskServices
{
    public interface ITaskQueryService
    {
        Task<TaskItem> GetTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllAsync();
    }
}
