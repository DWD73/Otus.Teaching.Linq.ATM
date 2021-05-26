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
            IEnumerable<User> users = Users.Where(x => x.Login == login && x.Password == password);
            
            return users;          
        }

        
        public List<string> GetUserInfoCash(int userId)
        {
            List<string> list = new List<string>();

            var accounts = Users.Join(Accounts, u => u.Id, a => a.UserId, (u, a) => new
            {
                UserId = a.UserId,
                OpeningDate = a.OpeningDate,
                CashAll = a.CashAll
            }).Where(u => u.UserId == userId).ToList();

            foreach (var t in accounts)
            {
                list.Add(($"Счет открыт {t.OpeningDate}: на счету {t.CashAll} рублей"));
            }

            return list;

        }

        public List<string> GetUserInfoCash2(int userId)
        {
            List<string> list = new List<string>();

            var accounts = Users.Join(Accounts, u => u.Id, a => a.UserId, (u, a) => new
            {
                UserId = a.UserId,
                OpeningDate = a.OpeningDate,
                CashAll = a.CashAll
            }).Where(u => u.UserId == userId).ToList();

            foreach (var t in accounts)
            {
                list.Add(($"Счет открыт {t.OpeningDate}: на счету {t.CashAll} рублей"));
            }

            return list;

        }
    }
}