namespace ATMSimulator.Logic
{
    public class Validator
    {
        public static bool ValidateAccountType(int accountType)
        {
            if (accountType >= 1 && accountType <= 2)
            {
                return true;
            }
            else
            {
                Console.WriteLine();
                Console.Write("Please input a value between 1 and 2: ");
                return false;
            }
        }

        public static bool ValidatePin(string pin)
        {
            if (!int.TryParse(pin, out _))
            {
                Console.WriteLine();
                Console.Write("Pin must be digits: ");
                return false;
            }
            if (pin.Length != 4)
            {
                Console.WriteLine();
                Console.Write("Pin must be 4 digit long: ");
                return false;
            }
            if (pin != pin)
            {
                Console.WriteLine("Incorrect Pin!");
                Console.WriteLine();
                return false;
            }
            return true;
        }

        public static bool ValidateAccountNumber(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                Console.WriteLine("Please enter a valid account Number: ");
                return false;
            }
            if (accountNumber.Length != 10)
            {
                Console.WriteLine("Please enter a 10 digit account number: ");
                return false;
            }
            if (!long.TryParse(accountNumber, out _))
            {
                Console.WriteLine("account number must be digits: ");
                return false;
            }
            if (accountNumber == null)
            {
                Console.WriteLine("Account number provided does not exist, Please confirm and try again.");
                Console.WriteLine();
                return false;
            }

            
            return true;
        }
        public static bool ValidateAmount(string amount)
        {
            decimal validAmount = 0;
            if (!decimal.TryParse(amount, out validAmount))
            {
                Console.WriteLine("Invalid amount entered. Please enter a valid numeric amount.");
                return false;
            }
            if (validAmount <= 0)
            {
                Console.WriteLine("Invalid amount entered. Please enter a value greater than Zero(0).");
                return false;
            }
            return true;
            
        }


    }
}
