using Opa.ToDoList.Common.Services;
using Opa.ToDoList.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;


namespace Opa.ToDoList.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IApiService apiService;
        private string password;
        private bool isRunning;
        private bool isEnabled;

        private DelegateCommand loginCommand;
        private DelegateCommand registerCommand;


        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
            IsEnabled = true;
            IsRemember = true;
            BackCommand = new Command(BackCommandHandler);
            Email = "nacional12344@gmail.com";
            Password= "123456";
        }

        public DelegateCommand LoginCommand => this.loginCommand ?? (this.loginCommand = new DelegateCommand(Login));
        public DelegateCommand RegisterCommand => this.registerCommand ?? (this.registerCommand = new DelegateCommand(Register));
        public ICommand BackCommand { get; set; }
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
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes Ingresar un correo",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debes Ingresar una contraseña.", "Aceptar");
                return;
            }

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = this.HasNoInternetConnection;
            if (connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Comprobar la conexión a Internet .", "Aceptar");
                return;
            }
            
            IsEnabled = false;
            IsRunning = true;

            var ownerResponse = await this.apiService.GetOwnerByEmailAsync(url, "api", "/owners/GetOwnerByEmail", Email);

            if (ownerResponse != null )
            {
                if (ownerResponse.IsSuccess)
                {
                    var validPassword = await this.apiService.ValidatePassword(url, "api", "/Account/ValidatePassword", ownerResponse.Result.Email, this.Password);

                    if (!validPassword.IsSuccess)
                    {
                        IsEnabled = true;
                        IsRunning = false;
                        await App.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos .", "Aceptar");
                        return;
                    }
                }
                else
                {
                    IsEnabled = true;
                    IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Error", "Problemas de conexion .", "Aceptar");
                    return;
                }
            }

            var param = new NavigationParameters()
            {
                { "owner", ownerResponse.Result }
            };
            await this.navigationService.NavigateAsync(nameof(TaskPage), param);
            IsEnabled = true;
            IsRunning = false;
        }

        public async void Register()
        {
            await this.navigationService.NavigateAsync("AddEditOwnerPage");
        }
    }
}