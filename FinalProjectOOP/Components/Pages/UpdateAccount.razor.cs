using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;


namespace FinalProjectOOP.Components.Pages
{
    public partial class UpdateAccount : ComponentBase
    {
        private Account account = new Account();
        private bool isSaved = false;

        [Parameter]
        public string UserId { get; set; }

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

    // book database accessor
        AccountDb accountDb = new AccountDb();

        protected override void OnInitialized()
        {
            // Get the book from the database using the BookId
            account = accountDb.GetAccount(UserId);
        }

        private async Task EditAccount()
        {
            // Update the book in the database
            Task.Run(() => accountDb.UpdateAccount(account));

            // Navigate back to the Home page
            isSaved = true;

            // Wait for 1 second before navigating back to the Home page
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/account");
        }
    }

}
