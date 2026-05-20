namespace TaskManagerApi.Model
{
    public class TaskItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isCompleted { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;
    }
}
