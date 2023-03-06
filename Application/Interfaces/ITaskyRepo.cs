using Domain;

namespace Application
{
    public interface ITaskyRepo
    {
        ICollection<Tasky> GetEveryoneTasks();
        ICollection<Tasky> GetAllTasks(int userID);
        Tasky GetTask(Guid id);
        bool CreateTask(Tasky task);
        bool EditTask(Tasky task);
        bool DeleteTask(Tasky task);
        bool Save();
        ICollection<Tasky> FilterByImportance(string importance);
        ICollection<Tasky> FilterByStatus(string status);

    }
}
