namespace Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public  DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem() { }

        public TaskItem(string title, string description, DateTime createAt, DateTime dueDate, bool isCompleted)
        {
            Title = title;
            Description = description;
            CreatedAt = createAt;
            DueDate = dueDate;
            IsCompleted = isCompleted;
        }
    }
}
