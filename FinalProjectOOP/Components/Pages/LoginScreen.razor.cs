using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;


namespace FinalProjectOOP.Components.Pages
{
    public partial class LoginScreen : ComponentBase
    {
        private Account account = new Account();
        private bool validLog = false;
        private bool notValid = false;
        private ActiveId activeId = ActiveId.Instance("FillerString");

        [Inject] NavigationManager NavigationManager { get; set; }

        AccountDb accountDb = new AccountDb();

        protected override void OnInitialized()
        {

        }

        private async Task LoginAccount()
        {
            Task.Run(() => account = accountDb.LogIn(account.Email, account.Password));

            if (account.UserId != "0" && account.UserId != null)
            {
                activeId.UserId = account.UserId;
                validLog = true;
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                notValid = true;
                await Task.Delay(1000);
                notValid = false;
            }  
        }
    }
}
