using System;
using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.Linq.ATM.Core.Services;
using Otus.Teaching.Linq.ATM.DataAccess;

namespace Otus.Teaching.Linq.ATM.Console
{
    using System;

    class Program
    {

        private static int UserId { get; set; }

        static void Main(string[] args)
        {
            System.Console.WriteLine($"Старт приложения-банкомата...\n");

            UserId = 0;

            var atmManager = CreateATMManager();


            //TODO: Далее выводим результаты разработанных LINQ запросов
            HelpMessage();

            ServiceManager(atmManager);

            Console.WriteLine("Завершение работы приложения-банкомата...");
            
            
        }

        private static void HelpMessage()
        {
            Console.WriteLine($"Навигация\n");
            Console.WriteLine($"\t{Command.UA} - Вывод информации клиента по логину и паролю");
            Console.WriteLine($"\t{Command.UAC} - Вывод информации о счетах клиента");
            Console.WriteLine($"\t{Command.UAH} - Вывод информации о счетах клиента и их историю");
            Console.WriteLine($"\t{Command.UAll} - Вывод данных о всех операциях пополенения счёта " +
                $"с указанием владельца каждого счёта");
            Console.WriteLine($"\t{Command.UN} - Вывод данных о всех пользователях у которых на счёте " +
                $"сумма больше N (N задаётся из вне и может быть любой);");
            Console.WriteLine($"\t{Command.Help} - Помощь по навигации");

            Console.WriteLine($"\t{Command.Exit} - Завершение программы\n");
        }

        static void ServiceManager(ATMManager aTMManager)
        {
            while (true)
            {
                switch(ReadCommand())
                {
                    case Command.Help:
                        HelpMessage();
                        break;
                    case Command.UA:
                        UserInfo(aTMManager);
                        break;
                    case Command.UAC:
                        UserInfoCash(aTMManager);
                        break;
                    case Command.UAH:
                        UserInfoCashDetail(aTMManager);
                        break;
                    case Command.UAll:
                        AllInputCashByUser(aTMManager);
                        break;
                    case Command.UN:
                        AllAccountByCashSum(aTMManager);
                        break;
                    case Command.Exit:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        

        #region  1. Вывод информации аккаунта по логину и паролю

        private static string GetPasswordLogin(string text)
        {
            Console.WriteLine($"Введите {text}");

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine($"{text} не должен быть пустым");
                }
            }

        }


        /// <summary>
        /// Вывод информации аккаунта по логину и паролю
        /// </summary>
        /// <param name="aTMManager"></param>
        private static void UserInfo(ATMManager aTMManager)
        {
            string login = GetPasswordLogin("логин");
            string password = GetPasswordLogin("пароль");

            foreach (var item in aTMManager.GetUserInfo(login, password).ToList())
            {
                UserId = item.Id;
                Console.WriteLine($"{item.ToString()}");
            }
                

            Console.WriteLine();
        }

        #endregion

        #region 2. Вывод данных о всех счетах заданного пользователя

        /// <summary>
        /// Вывод данных о всех счетах заданного пользователя
        /// </summary>
        /// <param name="aTMManager"></param>
        private static void UserInfoCash(ATMManager aTMManager)
        {
            if (UserId <= 0)
            {
                Console.WriteLine("Не выбран клиент счета(ов)");
            }
            else
            {
                List<string> list = aTMManager.GetUserInfoCash(UserId);

                Console.WriteLine($"Вывод информации о счетах клиента\n");

                for (int i = 0; i < aTMManager.GetUserInfoCash(UserId).Count; i++)
                {
                    Console.WriteLine($"\t{list[i]}");
                }
                
            }

        
            Console.WriteLine();
        }


        #endregion

        #region 3. Вывод информации о счетах клиента и их историю

        private static void UserInfoCashDetail(ATMManager aTMManager)
        {
            if (UserId <= 0)
            {
                Console.WriteLine("Не указан пользователь");
            }
            else
            {
                Console.WriteLine($"Вывод информации о счетах клиента и их историю\n");

                foreach (var account in aTMManager.GetUserAccounts(UserId))
                {
                    PrintInfoCash(aTMManager, account);
                    PrintInfoCashDetail(aTMManager, account);
                }

            }


            Console.WriteLine();
        }

        private static void PrintInfoCash(ATMManager aTMManager, int _account)
        {
            foreach (var account in aTMManager.GetUserInfoCashByAccount(_account))
                Console.WriteLine(account);
        }

        private static void PrintInfoCashDetail(ATMManager aTMManager, int _account)
        {
            foreach (var account in aTMManager.GetUserInfoCashByAccountDetail(_account))
                Console.WriteLine($"{account}\n");
        }


        #endregion

        #region 4. Вывод данных о всех операциях пополенения счёта с указанием владельца каждого счёта

        private static void AllInputCashByUser(ATMManager aTMManager)
        {
            UserId = 0;

            Console.WriteLine($"Вывод данных о всех операциях пополенения счёта\n");

            foreach (var accountCash in aTMManager.GetAllInputCashByUser())
                Console.WriteLine($"\t{accountCash}");
        }

        #endregion

        #region 5. Вывод данных о всех пользователях у которых на счёте сумма больше N

        private static void AllAccountByCashSum(ATMManager aTMManager)
        {
            UserId = 0;

            decimal number;

            Console.WriteLine($"Укажите сумму N для отбора\n");

            var sum = Console.ReadLine();

            if(!string.IsNullOrEmpty(sum) && decimal.TryParse(sum, out number))
            {
                Console.WriteLine($"Вывод данных о всех пользователях у которых на счёте сумма больше N\n");

                foreach (var accountCash in aTMManager.GetAllAccountByCashSum(number))
                    Console.WriteLine($"\t{accountCash}");
            }
            else
            {
                Console.WriteLine("Введите корректную сумму");
            }

            
        }

        #endregion

        private static Command ReadCommand()
        {
            while(true)
            {
                var input = GetCommand();
                if(Enum.TryParse(input, true, out Command command))
                {
                    return command;
                }
            }
        }

        private static string GetCommand()
        {
            while(true)
            {
                System.Console.WriteLine("Введите команду ...");
                var input = System.Console.ReadLine();

                if(!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    System.Console.WriteLine("Комманда не должна иметь пустую строку");
                }
            }
        }

       

       
        

        static ATMManager CreateATMManager()
        {
            //using var dataContext = new ATMDataContext();
            var dataContext = new ATMDataContext();
            var users = dataContext.Users.ToList();
            var accounts = dataContext.Accounts.ToList();
            var history = dataContext.History.ToList();
                
            return new ATMManager(accounts, users, history);
        }
    }


    public enum Command
    {
        Help,
        UA,
        UAC,
        UAH,
        UAll,
        UN,
        Exit
    }
}