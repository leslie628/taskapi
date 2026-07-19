namespace TaskManagerApi.Model
{
    public class CreateTaskRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public bool isCompleted { get; set; }
    }
}
