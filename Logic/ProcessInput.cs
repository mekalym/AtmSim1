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

        public static void CollecctPin(bool validPin, out string pinInput)
        {
            pinInput = null;
            Console.WriteLine("Please Enter a Pin: ");
            while (!validPin)
            {
                pinInput = Console.ReadLine();
                validPin = Validator.ValidatePin(pinInput.ToString());
            }
        }

        public static bool RetryCheck()
        {
            Console.WriteLine("Please Enter (Y)es to perform another Transaction or (N)o to Exit: ");
            var continueAction = Console.ReadLine();
            return continueAction.ToLower() == "y"; 
        }
    }
}
