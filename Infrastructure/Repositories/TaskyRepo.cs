using System;
using Application;
using Domain;
using Persistence;

// namespace Persistence
namespace Infrastructure.Repositories
{ 
    public class TaskyRepo : ITaskyRepo
    {
        private readonly LuxonDB _DB;
        public TaskyRepo(LuxonDB DB)
        {
            _DB = DB;
        }

        public bool Save()
        {
            var changes = _DB.SaveChanges();
            return changes > 0 ? true : false;
        }

        public ICollection<Tasky> FilterByImportance(string importance)
        {
            var TasksIMPORTANCE = _DB.Tasks.Where(task => task.Importance == importance).ToList();
            return TasksIMPORTANCE;
        }

        public ICollection<Tasky> FilterByStatus(string status)
        {
            var tasksSTATUS = _DB.Tasks.Where(task => task.Status == status).ToList();
            return tasksSTATUS;
        }

        public ICollection<Tasky> GetEveryoneTasks()
        {
            var everyoneTasks = _DB.Tasks.OrderBy(task => task.Id).ToList();
            return everyoneTasks;
        }

        public ICollection<Tasky> GetAllTasks(int userID)
        {
            var userTasks = _DB.Tasks.Where(Task => Task.OwnerId == userID).ToList();
            return userTasks;
        }

        public Tasky GetTask(Guid taskID)
        {
            var task = _DB.Tasks.Where(task => task.Id == taskID).FirstOrDefault();
            // var task = _DB.Tasks.Where(task => task.Id == userID).FirstOrDefault();
            return task;
        }

        public bool CreateTask(Tasky task)
        {
            _DB.Tasks.Add(task);
            return Save();
        }

        public bool EditTask(Tasky task)
        {
            //_DB.Tasks.Where(Task => Task.Id == task.Id).FirstOrDefault();
            _DB.Tasks.Update(task); 
            return Save();
        }

        public bool DeleteTask(Tasky task)
        {
            _DB.Tasks.Remove(task);
            return Save();
        }
    }
}
