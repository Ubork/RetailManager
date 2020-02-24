using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndpoint _userEndpoint;
        private readonly IMapper _mapper;
        private UserModel _selectedUser;
        private string _selectedUserName;
        private BindingList<string> _selectedUsersRoles;
        private BindingList<string> _selectedUsersAvailableRoles = new BindingList<string>();
        private string _selectedUsersRoleToRemove;
        private string _selectedAvailableRoleToAdd;

        BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set 
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set 
            { 
                _selectedUser = value;
                SelectedUserName = value.Email;
                SelectedUsersRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();        //This needs to be awaited - to be addressed later

                NotifyOfPropertyChange(() => SelectedUser);
            }
        }
        public string SelectedUsersRoleToRemove
        {
            get { return _selectedUsersRoleToRemove; }
            set 
            {
                _selectedUsersRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedUsersRoleToRemove);
                NotifyOfPropertyChange(() => Users);
            }
        }
        public string SelectedAvailableRoleToAdd
        {
            get { return _selectedAvailableRoleToAdd; }
            set 
            {
                _selectedAvailableRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedAvailableRoleToAdd);
                NotifyOfPropertyChange(() => Users);
            }
        }
        public string SelectedUserName
        {
            get 
            {
                return _selectedUserName;
            }
            set 
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(()=> SelectedUserName);
            }
        }
        public BindingList<string> SelectedUsersRoles
        {
            get { return _selectedUsersRoles; }
            set 
            {
                _selectedUsersRoles = value;
                NotifyOfPropertyChange(() => SelectedUsersRoles);
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }
        public BindingList<string> SelectedUsersAvailableRoles
        {
            get { return _selectedUsersAvailableRoles; }
            set
            {
                _selectedUsersAvailableRoles = value;
                NotifyOfPropertyChange(() => SelectedUsersAvailableRoles);
            }
        }
        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            _selectedUsersAvailableRoles.Clear();

            foreach (var role in roles)
            {
                if (!SelectedUsersRoles.Contains(role.Value))
                {
                    SelectedUsersAvailableRoles.Add(role.Value);
                }
            }
        }
        public async void AddSelectedRole()
        {
            UserRolePairModel pairing = new UserRolePairModel(SelectedUser.Id, SelectedAvailableRoleToAdd);
            string role = SelectedAvailableRoleToAdd;

            await _userEndpoint.AddUserToRole(pairing);
            
            SelectedUsersRoles.Add(role);
            SelectedUsersAvailableRoles.Remove(role);
        }
        public async void RemoveSelectedRole()
        {
            UserRolePairModel pairing = new UserRolePairModel(SelectedUser.Id, SelectedUsersRoleToRemove);
            string role = SelectedUsersRoleToRemove;
            
            await _userEndpoint.RemoveUserFromRole(pairing);

            SelectedUsersRoles.Remove(role);
            SelectedUsersAvailableRoles.Add(role);
        }
        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, 
            IUserEndpoint userEndpoint, IMapper mapper)
        {
            _status = status;
            _window = window;
            _userEndpoint = userEndpoint;
            _mapper = mapper;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                switch (ex.Message)
                {
                    case "Unauthorized":
                        _status.UpdateMessage("Unauthorized Access", "You donot have permission to interact with the Sales Form");
                        _window.ShowDialog(_status, null, settings);
                        break;

                    default:
                        _status.UpdateMessage("Fatal Exception", ex.Message);
                        _window.ShowDialog(_status, null, settings);
                        break;
                }
                TryClose();
            }
        }
        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            var users = _mapper.Map<List<UserModel>>(userList);
            Users = new BindingList<UserModel>(users);
        }
    }
}
