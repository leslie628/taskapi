namespace TaskManagerApi.Model
{
    public class SubTask
    {
        public string name { get; set; }
        public string? description { get; set; }
        public double estimated_time_hours { get; set; }
    }
    public class TaskSuggestion
    {
        public string Priority { get; set; }
        public List<SubTask> Subtasks { get; set; }
        public double total_estimated_time_hours { get; set; }
    }
}
