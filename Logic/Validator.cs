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
            return true;
        }
    }
}
