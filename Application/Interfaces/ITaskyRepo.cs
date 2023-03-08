using Domain;

namespace Application
{
    public interface ITaskyRepo
    {
        ICollection<Tasky> GetEveryoneTasks();
        ICollection<Tasky> GetAllMyTasks(string userName);
        //  ICollection<Tasky> GetAllMyTasks(Guid userID);
        Tasky GetTask(Guid id);
        bool CreateTask(Tasky task, string userID);
        bool EditTask(Tasky task);
        bool DeleteTask(Tasky task);
        bool Save();
        ICollection<Tasky> FilterByImportance(string importance, string userID);
        ICollection<Tasky> FilterByStatus(string status, string userID);

    }
}
