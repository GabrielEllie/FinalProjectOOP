using MySqlConnector;
using Dapper;
using FinalProjectOOP.Models;
using System.Text;

namespace FinalProjectOOP.services
{
    public class BooksDb
    {
        protected MySqlConnection connection;

        public BooksDb()
        {
            // get environemnt variable
            /*string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
*/
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "password",
                Database = "ooplibrary", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        // Initialize the database and create the books table
        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS books (
                Isbn VARCHAR(13) PRIMARY KEY,
                Title VARCHAR(255) NOT NULL,
                Author VARCHAR(255) NOT NULL,
                Year INT(4),
                Category VARCHAR(255) NOT NULL,
                About TEXT
            )";

            connection.Execute(sql);

            connection.Close();
        }

        // Takes in book attribtues and adds it to books database
        public void AddBook(Book book)
        {
            connection.Open();

            string sql = $"INSERT INTO books(Isbn, Title, Author, About, Category, Year) VALUES (\"{book.Isbn}\", \"{book.Title}\", \"{book.Author}\", \"{book.About}\", \"{book.Category}\", \"{book.Year}\")";

            connection.Execute(sql);

            connection.Close();
        }


        // uses select query in db then returns a books list
        public List<Book> GetBooks(Book bookFilter, bool byTitle, bool byAuthor, bool byYear, bool byCategory)
        {
            List<Book> books = new List<Book>();
            connection.Open();


            StringBuilder sql = new StringBuilder("SELECT * FROM books");
            if (byTitle || byAuthor || byYear || byCategory)
            {
                sql.Append(" WHERE");
            }
            if (byTitle)
            {
                sql.Append($" Title LIKE \'%{bookFilter.Title}%\' AND");
            }
            if (byAuthor)
            {
                sql.Append($" Author LIKE \'%{bookFilter.Author}%\' AND");
            }
            if (byYear)
            {
                sql.Append($" Year <= \'{bookFilter.Year}\' AND");
            }
            if (byCategory)
            {
                sql.Append($" Category LIKE \'%{bookFilter.Category}%\' AND");
            }

            if (sql.ToString().EndsWith("AND"))
            {
                sql.Length -= 4;
                sql.Append(';');
            }

            var query = $@"{sql.ToString()}";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Book book = new Book
                    {
                        Isbn = reader.GetString("Isbn"),
                        Title = reader.GetString("Title"),
                        Author = reader.GetString("Author"),
                        Category = reader.GetString("Category"),
                        Year = reader.GetInt32("Year"),
                        About = reader.GetString("About"),
                    };

                    books.Add(book);
                }
            }

            connection.Close();

            return books;
        }

        // takes isbn as a parameter and use select query to find a book
        public Book GetBook(string isbn)
        {
            Book book = null;
            connection.Open();
            var sql = @$"SELECT * FROM books WHERE Isbn = '{isbn}'";
            var cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    book = new Book
                    {
                        Isbn = reader.GetString("Isbn"),
                        Title = reader.GetString("Title"),
                        Author = reader.GetString("Author"),
                        Category = reader.GetString("Category"),
                        Year = reader.GetInt32("Year"),
                        About = reader.GetString("About"),
                    };
                }
            }

            connection.Close();

            return book;
        }

        // takes in all book attributes and allows editing
        public void UpdateBook(Book book)
        {
            connection.Open();

            string sql = $"UPDATE books SET Title = \"{book.Title}\"," + 
                $"Author = \"{book.Author}\"," +
                $"About = \"{book.About}\", " +
                $"Category = \"{book.Category}\"," +
                $"Year = \"{book.Year}\"" +
                $"WHERE Isbn = \"{book.Isbn}\"";

            connection.Execute(sql);

            connection.Close();
        }

        // Deletes a book from database
        public void DeleteBook(string isbn)
        {
            connection.Open();

            string sql = $"DELETE FROM books WHERE Isbn = \"{isbn}\"";

            connection.Execute(sql);

            connection.Close();

        }


    }
}
