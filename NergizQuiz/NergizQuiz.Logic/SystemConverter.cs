using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    public enum NumberSystem
    {
        Binary,
        Octal,
        Decimal,
        Hexadecimal
    }
    public static class SystemCalculator
    {
        #region fields
        public const string ZERO = "0";
        #endregion // fields

        #region public methods
        /// <summary>
        /// Converts a number from decimal number system to other number systems.
        /// </summary>
        /// <param name="originalNumber">The decimal number</param>
        /// <param name="fromSystem">The original numbers system</param>
        /// <param name="toSystem">The system you want the number to be converted to</param>
        /// <returns>Returns "0" if there was an error, otherwise returns a string
        /// Containing the converted number
        /// </returns>
        public static string ConvertNumber(string originalNumber, NumberSystem fromSystem, NumberSystem toSystem)
        {

            string convertedNum = Calculate(originalNumber, fromSystem, toSystem);

            if (convertedNum == string.Empty)
                return ZERO;
            else
                return convertedNum;
        }
        /// <summary>
        /// Checks wether all of the digits of a number are valid. Does
        /// The number system contains all of the digits?
        /// </summary>
        /// <param name="number"></param>
        /// <param name="system"></param>
        static public bool IsValidNumber(string number, NumberSystem system)
        {
            char[] bin = { '0', '1' };
            char[] oct = { '0', '1', '2', '3', '4', '5', '6', '7' };
            char[] dec = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                           'A', 'B', 'C', 'D', 'E', 'F' };
            char[] current = { };

            switch (system)
            {
                case NumberSystem.Binary:
                    current = bin;
                    break;
                case NumberSystem.Octal:
                    current = oct;
                    break;
                case NumberSystem.Decimal:
                    current = dec;
                    break;
                case NumberSystem.Hexadecimal:
                    current = hex;
                    break;
            }

            foreach (char num in number)
            {
                if (!current.Contains(num))
                    return false;
            }

            return true;
        }
        #endregion // public methods

        #region private methods
        private static string Calculate(string originalNumber, NumberSystem fromSystem, NumberSystem toSystem)
        {
            int number = ChangeToDecimal(originalNumber, fromSystem);
            StringBuilder builder = new StringBuilder();
            int digits = GetBase(toSystem);

            do
            {
                int remainder = number % digits;
                builder.Insert(0, ConvertToOneDigit(remainder));
                number /= digits;
            } while (number > 0);

            return builder.ToString();
        }
        private static int GetBase(NumberSystem system)
        {
            switch (system)
            {
                case NumberSystem.Binary:
                    return 2;
                case NumberSystem.Octal:
                    return 8;
                case NumberSystem.Hexadecimal:
                    return 16;
                default:
                    return 10;
            }
        }
        private static char ConvertToOneDigit(int number)
        {
            char[] possibleDigits = { '0', '1', '2', '3', '4', '5',
                '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            if (number > possibleDigits.Length)
                throw new ArgumentException("The number should not be greater than 15");

            return (possibleDigits[number]);
        }
        private static int ChangeToDecimal(string number, NumberSystem originalSystem)
        {  
            double systemBase = Convert.ToDouble(GetBase(originalSystem));

            if (originalSystem == NumberSystem.Decimal)
            {
                int result;
                bool succeed = int.TryParse(number, out result);
                if (succeed)
                    return result;
                else
                    return 0;
            }
          
            else
            {
                // make sure its done from Least Important to Most
                number = Reverse(number); 
                int totalNumber = 0;

                for (int i = 0; i < number.Length; i++)
                {
                    int weight = Convert.ToInt32(Math.Pow(systemBase, i));
                    totalNumber += ConvertToInteger(number[i]) * weight;
                }

                return totalNumber;
            }

        }
        private static int ConvertToInteger(char number)
        {
            number = number.ToString().ToUpper()[0];
            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };
            if (!letters.Contains(number))
                return int.Parse(number.ToString());
            else
            {
                switch (number)
                {
                    case 'A':
                        return 10;
                    case 'B':
                        return 11;
                    case 'C':
                        return 12;
                    case 'D':
                        return 13;
                    case 'E':
                        return 14;
                    default:
                        return 15;
                }
            }

        }
        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion // private methods
    }
}
