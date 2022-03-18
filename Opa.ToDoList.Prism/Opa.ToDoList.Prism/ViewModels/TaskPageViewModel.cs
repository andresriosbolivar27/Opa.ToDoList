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
    public class TaskPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;
        private ObservableCollection<OpaTask> taskList;
        private DelegateCommand addCommand;
        private DelegateCommand profileCommand;
        private DelegateCommand moreCommand;
        private DelegateCommand deleteTaskCommand;
        private DelegateCommand editTaskCommand;
        private OwnerResponse ownerResponse;
        private string name;


        public TaskPageViewModel(
            INavigationService navigationService, 
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
            IsEnabled = true;            
        }

        public Owner Owner { get; set; }
        public string Name
        {
            get => this.name;
            set => SetProperty(ref this.name, value);
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

        public DelegateCommand EditTaskCommand => this.editTaskCommand ?? (this.editTaskCommand = new DelegateCommand(EditTask));

        private void EditTask()
        {
            throw new NotImplementedException();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("owner"))
            {
                this.OwnerResponse = parameters.GetValue<OwnerResponse>("owner");
                this.Name = OwnerResponse.FirstName;
                this.TaskList = new ObservableCollection<OpaTask>(OwnerResponse.Tasks);
            }
        }
    }
}
