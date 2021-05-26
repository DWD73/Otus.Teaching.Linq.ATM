using System;

namespace Otus.Teaching.Linq.ATM.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string PassportSeriesAndNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return ($"{FirstName} {MiddleName} {SurName} " +
                $"\n\t телефон {Phone} " +
                $"\n\t паспорт {PassportSeriesAndNumber} " +
                $"\n\t дата регистрации {RegistrationDate}");
            
        }
    }
}