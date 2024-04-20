using MySqlConnector;
using Dapper;
using FinalProjectOOP.Models;
using System;

namespace FinalProjectOOP.services
{
    public class AccountDb
    {
        protected MySqlConnection connection;

        public AccountDb()
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

            var sql = @"CREATE TABLE IF NOT EXISTS accounts (
                UserId VARCHAR(36) PRIMARY KEY,
                Firstname VARCHAR(255) NOT NULL,
                Lastname VARCHAR(255) NOT NULL,
                Permission INT(4) CHECK (Permission BETWEEN 0 AND 2),
                Email VARCHAR(255) UNIQUE,
                Password VARCHAR(255),
                Phone VARCHAR(10)
            )";

            //Create an admin account


            connection.Execute(sql);

            connection.Close();

            Account admin = new Account();
            admin = GetAccount("AdminId");
            if (admin == null)
            {
                admin = GetAccountByEmail("Admin", "password123");
                if (admin == null)
                {
                    admin = AdminLogin();
                    AddAccount(admin);
                }
            }

            AdminAccount adminAccount = AdminAccount.Instance(admin);
        }

        private Account AdminLogin()
        {
            Account admin = new Account();
            admin = new Account
            {
                UserId = "AdminId",
                Firstname = "Admin",
                Lastname = "User",
                Permission = 2,
                Email = "Admin",
                Password = "password123"
            };
            return admin;
        }

        // Takes in book attribtues and adds it to books database
        public void AddAccount(Account account)
        {
            connection.Open();

            string sql = $"INSERT INTO accounts(UserId, Firstname, Lastname, Permission, Password, Email, Phone) " +
                $"VALUES (\"{account.UserId}\", \"{account.Firstname}\", \"{account.Lastname}\", {account.Permission}, \"{account.Password}\", \"{account.Email}\", \"{account.Phone}\")";

            connection.Execute(sql);

            connection.Close();
        }

        public Account GuestLogin()
        {
            ActiveId activeId = ActiveId.Instance("filler");
            activeId.UserId = "0";
            Account account = new Account
            {
                UserId = "0",
                Firstname = "Guest",
                Lastname = "User",
                Permission = 0
            };
            return account;
        }

        public Account LogIn(string email, string password)
        {
            ActiveId activeId = ActiveId.Instance("fillerString");
            Account account = null;
            connection.Open();
            var sql = $"SELECT * FROM accounts WHERE Email = \"{email}\" AND Password = \"{password}\"";
            var cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    account = new Account
                    {
                        UserId = reader.GetString("UserId"),
                        Firstname = reader.GetString("Firstname"),
                        Lastname = reader.GetString("Lastname"),
                        Permission = reader.GetInt32("Permission"),
                        Password = reader.GetString("Password"),
                        Email = reader.GetString("Email"),
                        Phone = reader.GetString("Phone"),
                    };
                }
                connection.Close();
            }
            if (account != null && account.Email == email && account.Password == password)
            {
                activeId.UserId = account.UserId;
                return account;
            }
            else
            {
                account = GuestLogin();
                return account;
            }

            
            
        }

        // takes isbn as a parameter and use select query to find a book
        public Account GetAccount(string userId)
        {
            Account account = null;
            connection.Open();
            var sql = @$"SELECT * FROM accounts WHERE UserId = '{userId}'";
            var cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    account = new Account
                    {
                        UserId = reader.GetString("UserId"),
                        Firstname = reader.GetString("Firstname"),
                        Lastname = reader.GetString("Lastname"),
                        Permission = reader.GetInt32("Permission"),
                        Password = reader.GetString("Password"),
                        Email = reader.GetString("Email"),
                        Phone = reader.GetString("Phone"),
                    };
                }
            }

            connection.Close();

            return account;
        }

        // takes in all book attributes and allows editing
        public void UpdateAccount(Account account)
        {
            connection.Open();

            string sql = $"UPDATE accounts SET Firstname = \"{account.Firstname}\"," +
                $"Lastname = \"{account.Lastname}\"," +
                $"Permission = \"{account.Permission}\", " +
                $"Email = \"{account.Email}\"," +
                $"Phone = \"{account.Phone}\"," +
                $"Password = \"{account.Password}\"" +
                $"WHERE UserId = \"{account.UserId}\"";

            connection.Execute(sql);

            connection.Close();
        }

        // returns account using email and password
        public Account GetAccountByEmail(string email, string password)
        {
            Account account = null;
            connection.Open();
            var sql = @$"SELECT * FROM accounts WHERE email LIKE '{email}' AND password LIKE '{password}'";
            var cmd = new MySqlCommand(sql, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    account = new Account
                    {
                        UserId = reader.GetString("UserId"),
                        Firstname = reader.GetString("Firstname"),
                        Lastname = reader.GetString("Lastname"),
                        Permission = reader.GetInt32("Permission"),
                        Password = reader.GetString("Password"),
                        Email = reader.GetString("Email"),
                        Phone = reader.GetString("Phone"),
                    };
                }
            }

            connection.Close();

            return account;
        }

        // Deletes a book from database
        public void DeleteAccount(string userId)
        {
            connection.Open();

            string sql = $"DELETE FROM accounts WHERE UserId = \"{userId}\"";

            connection.Execute(sql);

            connection.Close();

        }
    }
}
