using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProjectOOP.Models;
using FinalProjectOOP.services;

namespace FinalProjectOOP.Components.Pages
{
    public partial class BookDetails : ComponentBase
    {
        private Book book;
        private Account account;

        [Parameter]
        public string Isbn { get; set; }

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        // book database accessor
        BooksDb booksDb = new BooksDb();
        AccountDb accountDb = new AccountDb();
        ActiveId activeId = ActiveId.Instance("0");
        protected override void OnInitialized()
        {
            if (activeId.UserId == "0")
            {
                account = accountDb.GuestLogin();
            }
            else
            {
                account = accountDb.GetAccount(activeId.UserId);
            }
            book = booksDb.GetBook(Isbn);
        }

        private void EditBook(Book book)
        {
            // Navigate to the EditBook page
            NavigationManager.NavigateTo($"/editbook/{book.Isbn}");
        }

        private void DeleteBook(Book book)
        {
            // Delete the book from the database
            booksDb.DeleteBook(book.Isbn);

            NavigationManager.NavigateTo("/");
        }

    }
}
