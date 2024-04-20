using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProjectOOP.Models;

namespace FinalProjectOOP.services
{
    public sealed class AdminAccount
    {
        private AdminAccount() { }

        public Account AdminUser { get; set; }

        private static AdminAccount _instance;
        private static readonly object _lock = new object();

        public static AdminAccount Instance(Account account)
        {
            if (_instance == null)
            {

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AdminAccount();
                        _instance.AdminUser = account;
                    }
                }

            }

            return _instance;
        }
    }
}
