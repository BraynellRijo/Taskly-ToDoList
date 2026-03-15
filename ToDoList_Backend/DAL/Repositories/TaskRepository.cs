using Domain.Entities;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Repositories;

namespace DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IFileHandler _fileHandler;
        private const string FILE_NAME = "Task";

        public TaskRepository(IFileHandler fileHandler) 
        {
            _fileHandler = fileHandler;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _fileHandler.ReadData<TaskItem>(FILE_NAME);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _fileHandler.ReadDataAsync<TaskItem>(FILE_NAME); 
        }

        public async Task<TaskItem> GetValueAsync(int id)
        {
            var tasks = await _fileHandler.ReadDataAsync<TaskItem>(FILE_NAME);
            return tasks.FirstOrDefault(t => t.Id == id);
        }
        public async Task<IEnumerable<TaskItem>> GetCompletedTasks()
        {
            var tasks = await _fileHandler.ReadDataAsync<TaskItem>(FILE_NAME);
            return tasks.Where(t => t.IsCompleted);
        }
        public async Task<IEnumerable<TaskItem>> GetDueTasks()
        {
            var tasks = await _fileHandler.ReadDataAsync<TaskItem>(FILE_NAME);
            return tasks.Where(t => !t.IsCompleted);
        }

        public async Task CreateAsync(TaskItem entity)
        {
            _fileHandler.SaveData(FILE_NAME, entity);
        }
        public async Task MarkTaskAsCompleted(int id)
        {
            var tasks = (await GetAllAsync()).ToList();
            var index = tasks.FindIndex(t => t.Id == id);

            if (index < 0) return;
            tasks[index].IsCompleted = true;

            await _fileHandler.OverwriteDataAsync(FILE_NAME, tasks);
        }
        public async Task UpdateAsync(int id, TaskItem newEntity)
        {
            var tasks = (await GetAllAsync()).ToList();
            var index = tasks.FindIndex(t => t.Id == id);
            if (index < 0) return;

            newEntity.Id = id;
            newEntity.CreatedAt = tasks[index].CreatedAt;
            newEntity.IsCompleted = tasks[index].IsCompleted;
            tasks[index] = newEntity;

            await _fileHandler.OverwriteDataAsync(FILE_NAME, tasks);
        }
        public async Task DeleteAsync(int id)
        {
            var tasks = (await GetAllAsync()).ToList();

            int removed = tasks.RemoveAll(t => t.Id == id);
            if (removed == 0) return;

            await _fileHandler.OverwriteDataAsync(FILE_NAME, tasks);
        }

    }
}
