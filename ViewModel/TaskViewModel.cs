using System.Collections.Generic;
using System.Windows.Input;
using TaskManager.Model;
using TaskManager.Model.Database;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class TaskViewModel : BaseViewModel
    {
        public ICommand ConfirmCommand { get; set; }

        public Task Task { get; }
        public List<User> Users { get; }
        public List<Status> Statuses { get; }
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; updateUser(value); }
        }
        public Status CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; updateStatus(value); }
        }

        private User _currentUser;
        private Status _currentStatus;
        private readonly TaskRepository _taskRepository;
        private readonly StatusRepository _statusRepository;
        private readonly MembershipRepository _membershipRepository;

        public TaskViewModel(Task task)
        {
            _taskRepository = new TaskRepository(WindowViewModel.DatabaseContext);
            _statusRepository = new StatusRepository(WindowViewModel.DatabaseContext);
            _membershipRepository = new MembershipRepository(WindowViewModel.DatabaseContext);

            User user = MainViewModel.User;

            Roles role = (Roles)_membershipRepository.Read(x => x.ProjectId == task.ProjectId).RoleId;

            Task = task;
            CurrentStatus = task.Status;
            Statuses = role > Roles.Administrator
                ? _statusRepository.ReadAllExclude(Model.Database.Statuses.Done)
                : _statusRepository.ReadAll();

            Users = _membershipRepository.GetProjectUsers(task.ProjectId);
            CurrentUser = task.User;
        }

        private void updateUser(User user)
        {
            Task.UserId = user.Id;
            _taskRepository.Update(Task);
        }
        private void updateStatus(Status status)
        {
            Task.StatusId = status.Id;
            _taskRepository.Update(Task);
        }
    }
}
