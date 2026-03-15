using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        IEnumerable<TaskItem> GetAll();
        Task MarkTaskAsCompleted(int id);
        Task<IEnumerable<TaskItem>> GetCompletedTasks();
        Task<IEnumerable<TaskItem>> GetDueTasks();
    }
}
