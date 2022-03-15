using Opa.ToDoList.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opa.ToDoList.Prism.ViewModels
{
    public class TaskPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;

        public TaskPageViewModel(
            INavigationService navigationService, 
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
        }
    }
}
