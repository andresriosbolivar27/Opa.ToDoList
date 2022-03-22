using Newtonsoft.Json;
using Opa.ToDoList.Common.Models;
using Opa.ToDoList.Common.Services;
using Opa.ToDoList.Entities.Business.Entities;
using Opa.ToDoList.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Opa.ToDoList.Prism.ViewModels
{
    public class AddEditTaskPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;
        private string name;
        private string description;
        private bool isRunning;
        private bool isEnabled;
        private bool isEdit;
        private DateTime date;
        private DelegateCommand<Owner> saveCommand;
        private TaskRequest task;
        private OwnerResponse ownerResponse;
        private ObservableCollection<TaskStateResponse> taskStates;
        private ObservableCollection<TaskCategoryResponse> taskCategories;
        private TaskStateResponse taskState;
        private TaskCategoryResponse taskCategory;

        public AddEditTaskPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
            this.IsEnabled = true;
            this.MinimumDate = DateTime.Now;
        }

        public DelegateCommand<Owner> SaveCommand => this.saveCommand ?? (this.saveCommand = new DelegateCommand<Owner>(SaveAsync));
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
        }

        public DateTime Date
        {
            get => this.date;
            set => SetProperty(ref this.date, value);
        }

        public DateTime MinimumDate { get; set; }

        public string Description
        {
            get => this.description;
            set => SetProperty(ref this.description, value);
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

        public TaskRequest Task
        {
            get => this.task;
            set => SetProperty(ref this.task, value);
        }

        public ObservableCollection<TaskStateResponse> TaskStates
        {
            get => this.taskStates;
            set => SetProperty(ref this.taskStates, value);
        }

        public TaskCategoryResponse TaskCategory
        {
            get => this.taskCategory;
            set => SetProperty(ref this.taskCategory, value);
        }

        public ObservableCollection<TaskCategoryResponse> TaskCategories
        {
            get => this.taskCategories;
            set => SetProperty(ref this.taskCategories, value);
        }

        public TaskStateResponse TaskState
        {
            get => this.taskState;
            set => SetProperty(ref this.taskState, value);
        }

        public OwnerResponse OwnerResponse
        {
            get => this.ownerResponse;
            set => SetProperty(ref this.ownerResponse, value);
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Ingresar Un Nombre", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Ingresar Una Descripción", "Aceptar");
                return false;
            }

            if (this.TaskCategory == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Seleccionar Una Categoria", "Aceptar");
                return false;
            }

            if (this.TaskState == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Seleccionar Un Estado", "Aceptar");
                return false;
            }

            return true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("editTask"))
            {
                IsEdit = true;
                this.Title = "Editar Tarea";
                LoadData();
                this.Task = parameters.GetValue<TaskRequest>("editTask");
                this.OwnerResponse = parameters.GetValue<OwnerResponse>("owner");
                this.Name = this.Task.Name;
                this.Description = this.Task.Description;
                this.TaskCategories = TaskCategories;
                this.TaskStates = TaskStates;
                this.TaskCategory = TaskCategories.SingleOrDefault(x => x.Id == this.Task.CategoryId);
                this.TaskState = TaskStates.SingleOrDefault(x => x.Id == this.Task.TaskStateId);
                this.TaskCategory.Name = TaskCategories.FirstOrDefault(x => x.Id == this.Task.CategoryId).Name;
                this.TaskState.Name = TaskStates.FirstOrDefault(x => x.Id == this.Task.TaskStateId).Name;

            }
            else
            {
                this.Task = new TaskRequest();
                IsEdit = false;
                this.Title = "Adicionar Tarea";
                this.OwnerResponse = parameters.GetValue<OwnerResponse>("addTask");
                LoadData();
            }
        }

        private void LoadData()
        {
            LoadTaskCategoriesAsync();
            LoadTaskStatesAsync();
        }

        private async void LoadTaskStatesAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<TaskStateResponse>(
                url,
                "/api",
                "/TaskStates");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var taskStates = (List<TaskStateResponse>)response.Result;
            TaskStates = new ObservableCollection<TaskStateResponse>(taskStates);

            if (Task.TaskStateId != 0)
            {
                TaskState = TaskStates.FirstOrDefault(pt => pt.Id == TaskState.Id);
            }
        }

        private async void LoadTaskCategoriesAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<TaskCategoryResponse>(
                url,
                "/api",
                "/TaskCategories");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            var taskCategories = (List<TaskCategoryResponse>)response.Result;
            TaskCategories = new ObservableCollection<TaskCategoryResponse>(taskCategories);

            if (Task.CategoryId != 0)
            {
                TaskCategory = TaskCategories.FirstOrDefault(pt => pt.Id == TaskCategory.Id);
            }
        }

        private async void SaveAsync(Owner ownerRequest)
        {
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var owner = this.OwnerResponse;

            var task = new TaskRequest
            {
                Name = this.Name,
                Description = this.Description,
                CategoryId = this.TaskCategory.Id,
                TaskStateId = this.TaskState.Id,
                OwnerId = owner.Id,
                CreatedDate = this.Date,
                CompletedDate = this.IsEdit == true && this.TaskState.Id == 1 ? DateTime.Now : DateTime.MinValue,
                Id = this.Task.Id
            };

            Response<object> response;
            if (IsEdit)
            {
                response = await this.apiService.PutAsync(
                    url,
                    "/api",
                    "/tasks",
                    this.Task.Id,
                    task);
            }
            else
            {
                response = await this.apiService.PostAsync(
                    url,
                    "/api",
                    "/tasks/RegisterTask",
                    task);
            }

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            IsRunning = false;
            IsEnabled = true;

            await App.Current.MainPage.DisplayAlert(
                "Tarea",
                this.IsEdit == true ? "Tarea actualizada con exito" : "Tarea registrada con exito",
                "Aceptar");

            var ownerResponse = await this.apiService.GetOwnerByEmailAsync(url, "api", "/owners/GetOwnerByEmail", this.ownerResponse.Email);
            this.OwnerResponse = ownerResponse.Result;
            var param = new NavigationParameters()

            {
                { "owner", this.OwnerResponse}
            };
            await this.navigationService.NavigateAsync(nameof(TaskPage), param);
        }
    }
}
