using System;
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




        public List<int> GetUserAccounts(int userId)
        {
            List<int> list = new List<int>();

            var accounts = Users.Join(Accounts, u => u.Id, a => a.UserId, (u, a) => new
            {
                UserId = a.UserId,
                OpeningDate = a.OpeningDate,
                CashAll = a.CashAll,
                AccountId = a.Id
            }).Where(u => u.UserId == userId).ToList();

            foreach (var t in accounts)
            {
                list.Add(t.AccountId);
            }

            return list;

        }


        public List<string> GetUserInfoCashByAccount(int accountId)
        {
            List<string> list = new List<string>();

            var accounts = Users.Join(Accounts, u => u.Id, a => a.UserId, (u, a) => new
            {
                UserId = a.UserId,
                OpeningDate = a.OpeningDate,
                CashAll = a.CashAll,
                AccountId = a.Id
            }).Where(a => a.AccountId == accountId).ToList();

            foreach (var t in accounts)
            {
                list.Add(($"Счет открыт {t.OpeningDate}: на счету {t.CashAll} рублей\n"));
            }

            return list;

        }

        public List<string> GetUserInfoCashByAccountDetail(int accountId)
        {
            List<string> list = new List<string>();

            var hh = History.Where(x => x.AccountId == accountId).ToList();
            foreach (var t in hh)
            {
                list.Add(($"\t{t.OperationDate}: {(t.OperationType == OperationType.InputCash ? "внесено" : "снято")} {t.CashSum} рублей"));
            }


            return list;

        }

        public List<string> GetAllInputCashByUser()
        {
            List<string> list = new List<string>();

            var inputCash = (from user in Users
                             join account in Accounts on user.Id equals account.UserId
                             join history in History on account.Id equals history.AccountId
                             select new
                             {
                                 user.FirstName,
                                 user.MiddleName,
                                 user.SurName,
                                 history.AccountId,
                                 history.OperationType,
                                 history.OperationDate,
                                 history.CashSum

                             }).Where(x => x.OperationType == OperationType.InputCash).ToList();


            foreach (var item in inputCash)
            {
                list.Add($"Счет {item.AccountId}  \n\t {item.OperationDate} внесено {item.CashSum} рублей. " +
                    $"( Счет клиента {item.FirstName} {item.MiddleName} {item.SurName} )");
            }



            return list;
        }


        public List<string> GetAllAccountByCashSum(decimal currenSum)
        {
            List<string> list = new List<string>();

            var inputCash = (from user in Users
                             join account in Accounts on user.Id equals account.UserId
                             //join history in History on account.Id equals history.AccountId
                             select new
                             {
                                 user.FirstName,
                                 user.MiddleName,
                                 user.SurName,                               
                                 account.Id,
                                 account.CashAll

                             }).Where(x => x.CashAll > currenSum).ToList();


            foreach (var item in inputCash)
            {
                list.Add($"Счет {item.Id} с общей суммой {item.CashAll} рублей. " +
                    $"( Счет клиента {item.FirstName} {item.MiddleName} {item.SurName} )");
            }



            return list;
        }





    }
}