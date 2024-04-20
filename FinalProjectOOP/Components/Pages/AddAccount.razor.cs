using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;


namespace FinalProjectOOP.Components.Pages
{
    public partial class AddAccount : ComponentBase
    {
        private Models.Account account;
        private bool isSaved = false;
        private bool isValid = true;

        // Used to navigate back to Home Page
        [Inject] NavigationManager NavigationManager { get; set; }

        // use to access accountdb functions
        AccountDb accountDb = new AccountDb();

        protected override void OnInitialized()
        {
            // create new id for user
            account = new Account
            {
                UserId = Guid.NewGuid().ToString(),
                Permission = 1,
            };

        }

        // takes in the credentials inputted then adds it to database if it does not exist
        private async Task RegisterAccount()
        {
            Account checkAccount = accountDb.GetAccountByEmail(account.Email, account.Password);
            if (
                account.Firstname != null &&
                account.Lastname != null &&
                account.Email != null &&
                account.Password != null &&
                checkAccount == null)
            {
                // Register Account
                Task.Run(() => accountDb.AddAccount(account));
                // Navigate back to the Home page

                isValid = true;
                isSaved = true;
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/account");
            }
            else
            {
                isValid = false;
                await Task.Delay(2000);
                isValid = true;
            }
        }



    }
}
