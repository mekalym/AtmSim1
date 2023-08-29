using ATMSimulator.Entities.Enums;
using ATMSimulator.Logic;

namespace ATMSimulator // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            var atm = new Atm();
            while(true)
            {
                Display.DisplayMenu();
                Console.Write("Please enter a value: ");
                var validInput = int.TryParse(Console.ReadLine(), out int input);
                if(!validInput)
                {
                    Console.WriteLine("Please enter a valid number!");
                    Console.WriteLine();
                    continue;
                }

                switch (input)
                {
                    case 1:
                        Atm.CreateAccount(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;

                    case 2:
                        Console.WriteLine("Please enter account number to deposit to: ");
                        var accountNumber = Console.ReadLine();

                        Console.WriteLine("Please enter amount to deposit:");
                        var amount = decimal.Parse(Console.ReadLine());

                        atm.Transact(accountNumber, amount, TransactionTypeEnum.Deposit);

                        break;
                    case 3:
                        Console.WriteLine("Please enter account number to withdraw from: ");
                        var accWith = Console.ReadLine();

                        Console.WriteLine("Please enter amount to withdraw:");
                        var withdrawalAmount = decimal.Parse(Console.ReadLine());

                        atm.Transact(accWith, withdrawalAmount, TransactionTypeEnum.Withdrawal);
                        break;
                    case 4:
                        Console.WriteLine("I Entered to Transfer");
                        break;
                    case 5:
                        Console.WriteLine("I Entered to View Transactions");
                        break;
                    case 6:
                        Console.WriteLine("Please enter your account number: ");
                        var accBalance = Console.ReadLine();

                        Console.WriteLine("Please enter your pin:");
                        var pin = int.Parse(Console.ReadLine());

                        Console.WriteLine("Getting account balance.....");
                        atm.BalanceEnquiry(accBalance, pin);
                        break;
                    case 7:
                        Console.WriteLine("Loading Accounts......");
                        atm.ViewAccounts();
                        Console.WriteLine();
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;
                        break;
                    case 8:
                        Console.WriteLine("I Entered to Exit");
                        break;
                    default:
                        Console.WriteLine("You Entered an invalid Number");
                        break;
                }
            }
        }
    }
}