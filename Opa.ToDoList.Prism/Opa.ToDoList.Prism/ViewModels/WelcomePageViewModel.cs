using Opa.ToDoList.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace Opa.ToDoList.Prism.ViewModels
{
    public class WelcomePageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        private DelegateCommand loginCommand;
        public WelcomePageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            this.navigationService = navigationService;
            //this.Title = Languages.Login;
            this.IsEnabled = true;
            this.IsRemember = true;
        }

        public DelegateCommand LoginCommand => this.loginCommand ?? (this.loginCommand = new DelegateCommand(Login));

        public bool IsRemember { get; set; }

        public string Email { get; set; }

        public string Password
        {
            get => this.password;
            set => SetProperty(ref this.password, value);
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

        private async void Login()
        {
            await this.navigationService.NavigateAsync(nameof(LoginPage));
        }
    }
}
