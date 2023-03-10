using System;
using Application;
using Domain;
using Microsoft.EntityFrameworkCore;
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

        public ICollection<Tasky> FilterByImportance(string importance, string userID)
        {
            var user = _DB.Usuarios
                .Include(u => u.Tasks)
                .Where(u => u.Id == userID)
                .FirstOrDefault();
            if (user == null) return null;

            var TasksIMPORTANCE = user.Tasks.Where(task => task.Importance == importance).ToList();
            return TasksIMPORTANCE;
        }

        public ICollection<Tasky> FilterByStatus(string status, string userID)
        {
            var user = _DB.Usuarios
                .Include(u => u.Tasks)
                .Where(u => u.Id == userID)
                .FirstOrDefault();
            if (user == null) return null;

            var tasksSTATUS = _DB.Tasks.Where(task => task.Status == status).ToList();
            return tasksSTATUS;
        }

        public ICollection<Tasky> GetEveryoneTasks()
        {
            var everyoneTasks = _DB.Tasks.OrderBy(task => task.Id).ToList();
            return everyoneTasks;
        }

        public ICollection<Tasky> GetAllMyTasks(string userID)
        {
            var user = _DB.Usuarios.Where(u => u.Id == userID).FirstOrDefault();
            if(user == null) return null;
            
            var userTasks = _DB.Tasks.Where(t => t.OwnerId == userID).ToList();

            return userTasks;
        }

        public Tasky GetTask(Guid taskID)
        {
            var task = _DB.Tasks.Where(task => task.Id == taskID).FirstOrDefault();
            return task;
        }

        public bool CreateTask(Tasky task, string userID)
        {
           var user = _DB.Usuarios
            .Include(u => u.Tasks)          //EAGERLY LOADING
            .Where(u => u.Id == userID)
            .FirstOrDefault();

            if (user != null) {
                task.OwnerId = user.Id;
                _DB.Tasks.Add(task);
                user.Tasks.Add(task);
                return Save();
            }
            return false;
        }

        public bool EditTask(Tasky task)
        {
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
