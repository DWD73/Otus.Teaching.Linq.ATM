using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.Linq.ATM.Core.Entities;

namespace Otus.Teaching.Linq.ATM.Core.Services
{
    public class ATMManager
    {
        public IEnumerable<Account> Accounts { get; private set; }
        
        public IEnumerable<User> Users { get; private set; }
        
        public IEnumerable<OperationsHistory> History { get; private set; }
        
        public ATMManager(IEnumerable<Account> accounts, IEnumerable<User> users, IEnumerable<OperationsHistory> history)
        {
            Accounts = accounts;
            Users = users;
            History = history;
        }

        //TODO: Добавить методы получения данных для банкомата

        public IEnumerable<User> GetUserInfo(string login, string password)
        {           
            return Users.Where(x => x.Login == login && x.Password == password);           
        }
    }
}