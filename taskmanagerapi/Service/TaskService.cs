using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using taskmanagerapi.Models;

namespace taskmanagerapi.Service
{
    public class TaskService
    {
        private readonly TaskdbContext _taskdbContext;

        public TaskService(TaskdbContext taskdbContext)
        {
            _taskdbContext = taskdbContext;
        }

        public async Task<Models.Task> CreateTask(Models.Task task, Guid userId)
        {
            EntityEntry<Models.Task> addedTask = await _taskdbContext.Tasks.AddAsync(task);
            await _taskdbContext.SaveChangesAsync();
            await _taskdbContext.TaskUsers.AddAsync(new TaskUser
            { 
                TaskId = addedTask.Entity.TaskId,
                UserId = userId.ToString()
            });
            await _taskdbContext.SaveChangesAsync();    
            return addedTask.Entity;
        }  
        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            var task = await _taskdbContext.Tasks.FindAsync(taskId);
            _taskdbContext.TaskUsers.Remove(task.TaskUsers.First(x=>x.TaskId == taskId));
            _taskdbContext.Tasks.Remove(task);
            await _taskdbContext.SaveChangesAsync();    
        }
        public async Task<Models.Task> UpdateTask(Models.Task task)
        {
            EntityEntry<Models.Task> addedTask = _taskdbContext.Tasks.Update(task);
            await _taskdbContext.SaveChangesAsync();
            return addedTask.Entity;
        } 
        public async Task<List<Models.Task>> GetTasks(Guid userId)
        {
            return await _taskdbContext.TaskUsers.Where(x=> x.UserId == userId.ToString()).Select(x => x.Task).ToListAsync() ;
        }
    }
}
