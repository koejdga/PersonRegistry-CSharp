using Person.Exceptions;
using System;
using System.Threading;

namespace Person.Models
{
    public class PersonModel
    {

        #region Zodiac signs enums
        public enum WesternZodiacSign
        {
            Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, Pisces
        }
        public enum ChineseZodiacSign
        {
            Rat = 1924, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig
        }
        #endregion

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public EmailAddressModel Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public bool IsAdult { get; private set; }
        public WesternZodiacSign SunSign { get; private set; }
        public ChineseZodiacSign ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }


        public PersonModel(string name, string surname, string email, DateTime birthDate)
        {
            // Thread.Sleep(3000);

            if (BirthDateIsInFuture(birthDate))
            {
                throw new BirthDateIsInFutureException("Birth date cannot be in the future");
            }
            
            if (BirthDateIsTooFarAway(birthDate))
            {
                throw new BirthDateIssTooFarAwayException("Age of a person cannot be more than 135");
            }

            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Email = new EmailAddressModel(email);

            IsBirthday = CheckIfBirthday(birthDate);
            IsAdult = CheckIfAdult(birthDate);

            SunSign = ComputeWesternZodiacSign(birthDate);
            ChineseSign = ComputeChineseZodiacSign(birthDate);
        }

        public PersonModel(string name, string surname, string email) : this(name, surname, email, default)
        {}

        public PersonModel(string name, string surname, DateTime birthDate) : this(name, surname, default, birthDate)
        {}

        public PersonModel(RegisterPersonModel newPerson) :
            this(newPerson.Name, newPerson.Surname, newPerson.Email.ToString(), newPerson.BirthDate)
        {}

        public PersonModel(DbPersonModel dbPerson) :
            this(dbPerson.Name, dbPerson.Surname, dbPerson.Email, Convert.ToDateTime(dbPerson.BirthDate))
        { }

        private bool BirthDateIsInFuture(DateTime birthDate)
        {
            return birthDate > DateTime.UtcNow.Date;
        }

        private bool BirthDateIsTooFarAway(DateTime birthDate)
        {
            return ComputeAge(birthDate) > 135;
        }

        private bool CheckIfBirthday(DateTime date)
        {
            return date.Day == DateTime.UtcNow.Day && date.Month == DateTime.UtcNow.Month;
        }

        private bool CheckIfAdult(DateTime birthDate)
        {
            return ComputeAge(birthDate) >= 18;
        }

        private int ComputeAge(DateTime birthDate)
        {
            return (DateTime.Now - birthDate).Days / 365;
        }

        private WesternZodiacSign ComputeWesternZodiacSign(DateTime date)
        {
            if (date < new DateTime(date.Year, 1, 21))
            {
                return WesternZodiacSign.Capricorn;
            }
            if (date < new DateTime(date.Year, 2, 19))
            {
                return WesternZodiacSign.Aquarius;
            }
            if (date < new DateTime(date.Year, 3, 21))
            {
                return WesternZodiacSign.Pisces;
            }
            if (date < new DateTime(date.Year, 4, 21))
            {
                return WesternZodiacSign.Aries;
            }
            if (date < new DateTime(date.Year, 5, 22))
            {
                return WesternZodiacSign.Taurus;
            }
            if (date < new DateTime(date.Year, 6, 22))
            {
                return WesternZodiacSign.Gemini;
            }
            if (date < new DateTime(date.Year, 7, 23))
            {
                return WesternZodiacSign.Cancer;
            }
            if (date < new DateTime(date.Year, 8, 23))
            {
                return WesternZodiacSign.Leo;
            }
            if (date < new DateTime(date.Year, 9, 24))
            {
                return WesternZodiacSign.Virgo;
            }
            if (date < new DateTime(date.Year, 10, 24))
            {
                return WesternZodiacSign.Libra;
            }
            if (date < new DateTime(date.Year, 11, 23))
            {
                return WesternZodiacSign.Scorpio;
            }
            if (date < new DateTime(date.Year, 12, 22))
            {
                return WesternZodiacSign.Sagittarius;
            }
            return WesternZodiacSign.Capricorn;

        }

        private ChineseZodiacSign ComputeChineseZodiacSign(DateTime date)
        {
            int year = date.Year;
            while (year > 1935)
            {
                year -= 12;
            }

            while (year < 1924)
            {
                year += 12;
            }

            return (ChineseZodiacSign)year;
        }

    }
}
