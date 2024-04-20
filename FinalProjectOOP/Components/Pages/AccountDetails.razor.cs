using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;


namespace FinalProjectOOP.Components.Pages
{
    public partial class AccountDetails : ComponentBase
    {

        private Models.Account account = new Account();
        // shows a popup once a the account is deleted
        private bool isDeleted = false;
        // shows length of string and replaces with "*"
        private string MaskedPassword { get; set; }

        private List<Borrow> borrowList = new List<Borrow>();
        private List<Book> books = new List<Book>();

        [Parameter]
        public string UserId { get; set; }

        ActiveId activeId = ActiveId.Instance("0");

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        // book database accessor
        AccountDb accountDb = new AccountDb();
        BorrowDb borrowDb = new BorrowDb();

        // Logs in as guest user to manage account
        protected override void OnInitialized()
        {
            UserId = activeId.UserId;
            // Shows password length without showing actual password
            account = accountDb.GetAccount(UserId);
            if (UserId == "0")
            {
                account = accountDb.GuestLogin();
                MaskedPassword = " ";
            }
            else
            {
                account = accountDb.GetAccount(UserId);
                MaskedPassword = new string('*', account.Password.Length);
                borrowList = borrowDb.BorrowedBooks(account.UserId);   
            }
            
        }

        // Navigates to Add Account page to input credentials
        private void RegisterAccount()
        {
            NavigationManager.NavigateTo("/addaccount");
        }

        // Deletes account from database then logs in as a guest
        private async void DeleteAccount(Account account)
        {
            // Permanently Deletes Account
            accountDb.DeleteAccount(account.UserId);
            borrowDb.DeleteBorrowedBooks(account.UserId);
            account = accountDb.GuestLogin();
            isDeleted = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/");
        }

        // Changes the credentials of the account
        private void UpdateAccount(Account account)
        {
            NavigationManager.NavigateTo($"/editaccount/{UserId}");
        }

        // Logs in to an existing account
        private void Login()
        {
            NavigationManager.NavigateTo($"/loginscreen");
        }
        
        // Logs out of the account and logs in as a guest user
        private void Logout()
        {
            account = accountDb.GuestLogin();
            NavigationManager.NavigateTo($"/");
        }

        private void DeleteBorrow(Borrow borrow)
        {
            foreach (Borrow b in borrowList)
            {
                if (b.Isbn == borrow.Isbn)
                {
                    borrowDb.DeleteBorrow(UserId, borrow.Isbn);
                }
            }
            borrowList.Remove(borrow);
        }
    }
}
