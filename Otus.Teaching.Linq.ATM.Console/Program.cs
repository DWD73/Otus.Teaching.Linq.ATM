using System;
using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.Linq.ATM.Core.Services;
using Otus.Teaching.Linq.ATM.DataAccess;

namespace Otus.Teaching.Linq.ATM.Console
{
    class Program
    {
        

        static void Main(string[] args)
        {
            System.Console.WriteLine("Старт приложения-банкомата...");

            var atmManager = CreateATMManager();
            

            //TODO: Далее выводим результаты разработанных LINQ запросов

            ServiceManager(atmManager);

            System.Console.WriteLine("Завершение работы приложения-банкомата...");
            
            
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
                    case Command.UACash:
                        UserInfoCash(aTMManager);
                        break;
                    case Command.Exit:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static int UserId { get; set; }

        private static void UserInfo(ATMManager aTMManager)
        {
            string login = GetPasswordLogin("логин");
            string password = GetPasswordLogin("пароль");

            foreach (var t in aTMManager.GetUserInfo(login, password).ToList())
            {
                UserId = t.Id;
                System.Console.WriteLine($"{t.ToString()}");
            }
                

            System.Console.WriteLine();
        }

        private static void UserPrint()
        {

        }

        

        private static void UserInfoCash(ATMManager aTMManager)
        {
            if (UserId <= 0)
            {
                System.Console.WriteLine("Нуль значение");
            }
            else
            {
                List<string> list = aTMManager.GetUserInfoCash(UserId);

                for (int i = 0; i < aTMManager.GetUserInfoCash(UserId).Count; i++)
                {
                    System.Console.WriteLine(list[i]);
                }
                
            }

        
            System.Console.WriteLine();
        }

        

        private static string GetPasswordLogin(string text)
        {
            System.Console.WriteLine($"Введите {text}");
            
            while(true)
            {
                var input = System.Console.ReadLine();
                if(!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    System.Console.WriteLine($"{text} не должен быть пустым");
                }
            }
            
        }


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
                    System.Console.WriteLine("Не должно быть пустым");
                }
            }
        }

       

        private static void HelpMessage()
        {
            System.Console.WriteLine($"{Command.UA} - Вывод информации о заданном аккаунте по логину и паролю");
            System.Console.WriteLine($"{Command.UACash} - Вывод информации о счетах заданного клиента");
            System.Console.WriteLine($"{Command.Help} - Помощь по навигации");
            System.Console.WriteLine($"{Command.Exit} - Завершение программы");
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
        UACash,
        Exit
    }
}