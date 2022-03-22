using Opa.ToDoList.Common.Models;
using Opa.ToDoList.Common.Services;
using Opa.ToDoList.Entities.Business.Entities;
using Opa.ToDoList.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Opa.ToDoList.Prism.ViewModels
{
    public class TaskPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;
        private ObservableCollection<OpaTask> taskList;
        private DelegateCommand addCommand;
        private DelegateCommand profileCommand;
        private DelegateCommand moreCommand;
        private DelegateCommand deleteTaskCommand;
        private DelegateCommand refreshCommand;
        private DelegateCommand<OpaTask> editTaskCommand;
        private OwnerResponse ownerResponse;
        private string name;
        private bool isRunning;
        private bool isEnabled;
        private bool isRefreshing;
        private static TaskPageViewModel instance;

        public TaskPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
            IsEnabled = true;
            if (isRefreshing)
            {
                RefreshTasks();
                this.isRefreshing = false;
            }
        }
        public static TaskPageViewModel GetInstance()
        {
            return instance;
        }


        public Owner Owner { get; set; }
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => SetProperty(ref this.isRefreshing, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => SetProperty(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetProperty(ref this.isEnabled, value);
        }

        public ObservableCollection<OpaTask> TaskList
        {
            get => this.taskList;
            set => SetProperty(ref this.taskList, value);
        }

        public OwnerResponse OwnerResponse
        {
            get => ownerResponse;
            set => SetProperty(ref ownerResponse, value);
        }

        public DelegateCommand RefreshCommand => this.refreshCommand ?? (this.refreshCommand = new DelegateCommand(RefreshTasks));

        private async void RefreshTasks()
        {
            this.isRefreshing = true;
            IsEnabled = false;
            IsRunning = true;
            this.TaskList.Clear();
            await GetDataOwner();
            this.IsRefreshing = false;
        }

        private async Task GetDataOwner()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = this.HasNoInternetConnection;
            if (connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Comprobar la conexión a Internet .", "Aceptar");
                return;
            }
            
            var ownerResponse = await this.apiService.GetOwnerByEmailAsync(url, "api", "/owners/GetOwnerByEmail", this.OwnerResponse.Email);

            if (ownerResponse != null)
            {
                if (ownerResponse.IsSuccess)
                {
                    this.TaskList = new ObservableCollection<OpaTask>(OwnerResponse.Tasks);
                }
                else
                {
                    IsEnabled = true;
                    IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Error", "Problemas de conexion .", "Aceptar");
                    return;
                }
            }
        }

        public DelegateCommand AddCommand => this.addCommand ?? (this.addCommand = new DelegateCommand(AddTask));

        private async void AddTask()
        {

            var param = new NavigationParameters()
            {
                { "addTask", this.OwnerResponse}
            };
            await this.navigationService.NavigateAsync(nameof(AddEditTaskPage), param);
            IsEnabled = true;
            IsRunning = false;
        }

        public DelegateCommand ProfileCommand => this.profileCommand ?? (this.profileCommand = new DelegateCommand(ProfileUser));

        private void ProfileUser()
        {
            throw new NotImplementedException();
        }

        public DelegateCommand MoreCommand => this.moreCommand ?? (this.moreCommand = new DelegateCommand(MoreMenu));

        private void MoreMenu()
        {
            throw new NotImplementedException();
        }

        public DelegateCommand DeleteTaskCommand => this.deleteTaskCommand ?? (this.deleteTaskCommand = new DelegateCommand(DeleteTask));

        private void DeleteTask()
        {
            throw new NotImplementedException();
        }

        public DelegateCommand<OpaTask> EditTaskCommand => this.editTaskCommand ?? (this.editTaskCommand = new DelegateCommand<OpaTask>(EditTask));

        private async void EditTask(OpaTask task)
        {
            var param = new NavigationParameters()
            {
                { "editTask", new TaskRequest
                    {
                        CategoryId = task.Category.Id,
                        CompletedDate = task.CompletedDate,
                        Id = task.Id,
                        CreatedDate = task.CreatedDate,
                        Description = task.Description,
                        Name = task.Name,
                        OwnerId = this.OwnerResponse.Id,
                        TaskStateId = task.TaskState.Id
                    }
                },{"owner", this.OwnerResponse }
            };
            await this.navigationService.NavigateAsync(nameof(AddEditTaskPage), param);
            IsEnabled = true;
            IsRunning = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("owner"))
            {
                this.OwnerResponse = parameters.GetValue<OwnerResponse>("owner");                
                this.Name = OwnerResponse.FirstName;
                this.TaskList = new ObservableCollection<OpaTask>(this.OwnerResponse.Tasks);
            }
        }
    }
}
