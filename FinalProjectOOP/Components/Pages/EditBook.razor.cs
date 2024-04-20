using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;


namespace FinalProjectOOP.Components.Pages
{
    public partial class EditBook : ComponentBase
    {
        private Book book = new Book();
        private bool isSaved = false;

        [Parameter]
        public string Isbn { get; set; }

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        // book database accessor
        BooksDb booksDb = new BooksDb();

        protected override void OnInitialized()
        {
            // Get the book from the database using the BookId
            book = booksDb.GetBook(Isbn);
        }

        private async Task UpdateBook()
        {
            // Update the book in the database
            Task.Run(() => booksDb.UpdateBook(book));

            // Navigate back to the Home page
            isSaved = true;

            // Wait for 1 second before navigating back to the Home page
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/");
        }
    }

}
