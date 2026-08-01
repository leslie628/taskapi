using Microsoft.AspNetCore.Http.HttpResults;
using TaskManagerApi.Data;
using TaskManagerApi.Model;

namespace TaskManagerApi.services
{
    public interface ITaskService
    {
        List<TaskItem> GetAll();
        TaskItem GetById(int id);
        Task<TaskItem> Create(TaskItem task);
        Task CreateBulk(List<CreateTaskRequest> tasks);
        void Update(TaskItem task);
        void Delete(int id);
        void DeleteBulk(List<int> ids);
    }
    public class TaskService: ITaskService
    {
       private readonly AppDbContext _context;
        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public List<TaskItem> GetAll()
        {
            return _context.TaskItems.ToList();
        }

        public TaskItem GetById(int id)
        {
            return _context.TaskItems.Find(id);
        }

        public async Task<TaskItem> Create(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task CreateBulk(List<CreateTaskRequest> tasks)
        {
            var entities=tasks.Select(t => new TaskItem
            {
                title = t.title,
                description = t.description,
                isCompleted = t.isCompleted,
                CreatedDate = DateTime.UtcNow,
            }).ToList();
            await _context.TaskItems.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public void Update(TaskItem task)
        {
            _context.TaskItems.Update(task);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                _context.SaveChanges();
            }
        }
        public void DeleteBulk(List<int> ids)
        {
            var tasks = _context.TaskItems.Where(t => ids.Contains(t.id)).ToList();
            _context.TaskItems.RemoveRange(tasks);
            _context.SaveChanges();
        }

    }
}
