using Opa.ToDoList.Common.Services;
using Opa.ToDoList.Entities.Business.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Opa.ToDoList.Prism.ViewModels
{
    public class TaskPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;

        public ObservableCollection<OpaTask> TaskList { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }

        public TaskPageViewModel(
            INavigationService navigationService, 
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
        }
    }
}
