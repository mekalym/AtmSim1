namespace ATMSimulator.Logic
{
    public class ProcessInput
    {
        public static void CollecctAccountType(bool validAccountTypeInput, int accountTypeInput)
        {
            Console.WriteLine("Please select account type to create: ");
            Console.WriteLine("1. Savings");
            Console.WriteLine("2. Current");
            while (!validAccountTypeInput)
            {
                var accountTypeValidInput = int.TryParse(Console.ReadLine(), out accountTypeInput);
                if (!accountTypeValidInput)
                {
                    Console.WriteLine("Please Enter a valid Number");
                    continue;
                }
                validAccountTypeInput = Validator.ValidateAccountType(accountTypeInput);
            }
        }
        public static void CollectAccountNumber(bool validAccountNumber,out string accountNumber)
        {
            accountNumber = null;
            Console.WriteLine("Please enter account number: ");
            while (!validAccountNumber)
            {
                accountNumber = Console.ReadLine();
                validAccountNumber = Validator.ValidateAccountNumber(accountNumber);
                if (!validAccountNumber)
                {
                    Console.WriteLine("Please Enter a valid Number");
                    continue;
                }
            }
            
        }
        public static void CollectDepAmount(bool validAmount, out string amount)
        {
            amount = null;
            Console.WriteLine("Please enter amount to deposit: ");
            while (!validAmount)
            {
                amount = Console.ReadLine();
                validAmount = Validator.ValidateAmount(amount);
                if (!validAmount)
                {
                    continue;
                }
                
            }

        }        
        public static void CollectWithAmount(bool validAmount, out string amount)
        {
            amount = null;
            Console.WriteLine("Please enter amount: ");
            while (!validAmount)
            {
                amount = Console.ReadLine();
                validAmount = Validator.ValidateAmount(amount);
                if (!validAmount)
                {
                    continue;
                }

            }

        }
        
        public static void CollectPin(bool validPin, out string pinInput)
        {
            pinInput = null;
            Console.WriteLine("Please Enter Your Pin: ");
            while (!validPin)
            {
                pinInput = Console.ReadLine();
                validPin = Validator.ValidatePin(pinInput.ToString());
            }
        }
        public static bool RetryCheck()
        {
            while (true)
            {
                Console.WriteLine("Please Enter (Y)es to perform another Transaction or (N)o to Exit: ");
                var continueAction = Console.ReadLine().ToLower();

                if (continueAction == "y")
                {
                    return true; 
                }
                else if (continueAction == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                }
            }
        }        

    }
}
