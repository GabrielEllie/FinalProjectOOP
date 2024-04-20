using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;
using System.Data.SqlTypes;

namespace FinalProjectOOP.Components.Pages
{
    public partial class Library : ComponentBase
    {
        ActiveId userId = ActiveId.Instance("0");
        private Book filteredBook = new Book();
        private bool byTitle;
        private bool byAuthor;
        private bool byCategory;
        private bool byYear;
        private bool alreadyBorrowed = false;
        private bool borrowSuccess = false;

        // navigate back to home page
        [Inject] NavigationManager NavigationManager { get; set; }


        // database accessor
        private BooksDb booksDb = new BooksDb();
        private List<Book> books = new List<Book>();
        private AccountDb accountDb = new AccountDb();
        private BorrowDb borrowDb = new BorrowDb();
        private Models.Account account = new Models.Account();

        protected override void OnInitialized()
        {
            
            // initialize the database
            booksDb.InitializeDatabase();
            accountDb.InitializeDatabase();
            borrowDb.InitializeDatabase();


            // This account will be used to determine if you can borrow a book
            if (userId.UserId == "0")
            {
                account = accountDb.GuestLogin();
            }
            else
            {
                account = accountDb.GetAccount(userId.UserId);
            }
            books = booksDb.GetBooks(null, false, false, false, false);

        }

     
        private void EditBook(Book book)
        {
            // Navigate to the EditBook page
            NavigationManager.NavigateTo($"/editbook/{book.Isbn}");
        }

        private void ViewBookDetails(Book book)
        {
            // Navigate to the BookDetails page
            NavigationManager.NavigateTo($"/bookdetails/{book.Isbn}");
        }

        private void DeleteBook(Book book)
        {
            booksDb.DeleteBook(book.Isbn);
            books.Remove(book);
        }
        private void GetFilteredBooks()
        {
            books = booksDb.GetBooks(filteredBook, byTitle, byAuthor, byYear, byCategory);

        }

        private async Task BorrowBook(Book book)
        {

            await Task.Run( async () =>
            {
                if (borrowDb.BorrowExists(account.UserId, book.Isbn))
                {
                    
                    alreadyBorrowed = true;
                    await Task.Delay(3000);
                    alreadyBorrowed = false;
                    
                }
                else
                {
                    borrowDb.AddBorrow(account.UserId, book.Isbn);
                    borrowSuccess = true;
                    await Task.Delay(3000);
                    borrowSuccess = false;
                }
            });
        }
    }

    

}
