using Microsoft.AspNetCore.Components;
using FinalProjectOOP.services;
using FinalProjectOOP.Models;
using System.Text;
using System;

namespace FinalProjectOOP.Components.Pages
{
    public partial class AddBook : ComponentBase
    {
        private Book book;
        public bool isSaved = false;

        // book database accessor
        private BooksDb booksDb = new BooksDb();

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Isbn { get; set; }

        protected override void OnInitialized()
        {
            book = new Book
            {
                Isbn = GenerateISBN()
            };
        }
        private async Task SaveBook()
        {
            // Add the book to the list of books
            Task.Run(() => booksDb.AddBook(book));

            // Navigate back to the Home page
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/");
        }

        // Everything below this is AI generated ISBN generator
        private static string GenerateISBN()
        {
            Random random = new Random();
            string prefix = "978";
            string registrationGroup = random.Next(0, 10).ToString();
            string registrant = random.Next(0, 100000).ToString("D5");
            string publication = random.Next(0, 1000).ToString("D3");

            string isbnWithoutCheckDigit = $"{prefix}{registrationGroup}{registrant}{publication}";

            int checkDigit = CalculateCheckDigit(isbnWithoutCheckDigit);

            string isbn = $"{isbnWithoutCheckDigit}{checkDigit}";

            return isbn;
        }

        static int CalculateCheckDigit(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < isbn.Length; i++)
            {
                int digit = int.Parse(isbn[i].ToString());
                sum += digit * (1 + 2 * (i % 2));
            }

            int checkDigit = (10 - (sum % 10)) % 10;

            return checkDigit;
        }
    }
}
