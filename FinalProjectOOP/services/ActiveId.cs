using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Singleton in order to use a userId and transfer it to all pages
// This is the website I used to create this:
/*https://refactoring.guru/design-patterns/singleton/csharp/example#:~:text=Singleton%20is%20a%20creational%20design,the%20modularity%20of%20your%20code.*/

namespace FinalProjectOOP.services
{
    public sealed class ActiveId
    {
        private ActiveId() { }

        public string UserId { get; set; }

        private static ActiveId _instance;
        private static readonly object _lock = new object();

        public static ActiveId Instance(string userId)
        {
            if(_instance == null )
            {

                lock(_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new ActiveId();
                        _instance.UserId = userId;
                    }
                }
                
            }

            return _instance;
        }
    }
}
