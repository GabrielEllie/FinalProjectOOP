using MySqlConnector;
using Dapper;
using System;
using FinalProjectOOP.Models;

namespace FinalProjectOOP.services
{
    public class BorrowDb
    {
        protected MySqlConnection connection;

        public BorrowDb()
        {
            // get environemnt variable
            //*string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            //string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD

            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "password",
                Database = "ooplibrary", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS borrowlist (
                Isbn VARCHAR(13) PRIMARY KEY,
                Userid VARCHAR(36) NOT NULL,
                Startdate DATE
            )";

            connection.Execute(sql);

            connection.Close();
        }

        public void AddBorrow(string userid, string isbn)
        {
            connection.Open();

            string sql = $"INSERT INTO borrowlist(Userid, Isbn, Startdate) VALUES (\"{userid}\", \"{isbn}\", CURDATE())";

            connection.Execute(sql);

            connection.Close();
        }

        public void DeleteBorrow(string userid, string isbn)
        {
            connection.Open();

            string sql = $"DELETE FROM borrowlist WHERE Userid = \"{userid}\" AND Isbn = \"{isbn}\"";

            connection.Execute(sql);

            connection.Close();
        }
        public void DeleteBorrowedBooks(string userid)
        {
            connection.Open();

            string sql = $"DELETE FROM borrowlist WHERE Userid = \"{userid}\"";

            connection.Execute(sql);

            connection.Close();
        }

        public List<Borrow> BorrowedBooks(string userid)
        {
            List<Borrow> booksBorrowed = new List<Borrow>();
            connection.Open();

            string sql = $"SELECT * FROM books NATURAL JOIN borrowlist WHERE Userid = \"{userid}\"";
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Borrow borrow = new Borrow
                    {
                        Isbn = reader.GetString("Isbn"),
                        Title = reader.GetString("Title"),
                        UserId = reader.GetString("Userid"),
                        Date = reader.GetDateOnly("Startdate"),
                    };

                    booksBorrowed.Add(borrow);
                }
            }

            connection.Close();

            return booksBorrowed;
        }

        public bool BorrowExists(string userid, string isbn)
        {
            Borrow borrow = null;

            connection.Open();

            string sql = $"SELECT * FROM books NATURAL JOIN borrowlist WHERE Userid = \"{userid}\" AND Isbn = \"{isbn}\"";
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    borrow = new Borrow
                    {
                        Isbn = reader.GetString("Isbn"),
                        UserId = reader.GetString("Userid"),
                        Date = reader.GetDateOnly("Startdate"),
                    };

                }
            }

            connection.Close();

            if (borrow != null)
            {
                return true;
            }

            return false;
        }

    }
}
