using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL
{
    public enum CCType
    {
        VISA, MC
    }


    /// <summary>
    /// Credit card validation.
    /// Supports : VISA and MasterCard
    /// Reference: http://www.merriampark.com/anatomycc.htm
    /// Contains LUHN (mod 10) check
    /// by D.S.
    /// http://aspnetcafe.com
    /// </summary>
    public class CreditCardValidator
    {
        protected string _CardNumber = "";


        public CreditCardValidator(string aCardNumber)
        {
            _CardNumber = aCardNumber.Replace(" ", "").Replace("-", "");
            ProcessValidation();
        }


        protected bool _IsValid;


        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }


        private CCType _CardType;


        public CCType CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }

        protected void ProcessValidation()
        {
            IsValid = false;


            do
            {
                // Reg Ex check //
                Regex RegExNumber = new Regex(@"(?<firsttwo>(?<firstone>\d)\d)\d{11,14}");
                Match m = RegExNumber.Match(_CardNumber);
                if (!m.Success) break;
                string number = m.Groups[0].Value; // only digits //
                string firstNum = m.Groups["firstone"].Value;
                int firstTwoNum = int.Parse(m.Groups["firsttwo"].Value);
                bool passIssuer = (firstNum == "4") || ((firstTwoNum >= 51) && (firstTwoNum <= 55));
                if (!passIssuer) break;
                if (firstNum == "4") CardType = CCType.VISA;
                if ((firstTwoNum >= 51) && (firstTwoNum <= 55)) CardType = CCType.MC;
                // Now make Luhn check //
                bool passLuhn = LuhnCheck(number);
                if (!passLuhn) break;
                //
                IsValid = true;
            } while (false);
        }


        /// <summary>
        /// Performs mod 10 check
        /// </summary>
        /// <param name="cardNumber">Card Number with only numbers</param>
        /// <returns></returns>
        protected bool LuhnCheck(String cardNumber)
        {
            int sum = 0;
            bool timesTwo = false;


            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber.Substring(i, 1));
                int addend;
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                    {
                        addend -= 9;
                    }
                }
                else
                {
                    addend = digit;
                }
                sum += addend;
                timesTwo = !timesTwo;
            }


            int modulus = sum % 10;
            return (modulus == 0);
        }
    }
}
