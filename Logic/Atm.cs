using ATMSimulator.Entities;
using ATMSimulator.Entities.Enums;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Xml.Linq;

namespace ATMSimulator.Logic
{
    public class Atm
    {
        private string accountsPath = "C:\\Users\\USER\\Documents\\Visual Studio 2015\\Projects\\ATMsim\\DataStore\\account.txt";
        private string transactionPath = "C:\\Users\\USER\\Documents\\Visual Studio 2015\\Projects\\ATMsim\\DataStore\\transaction.txt";
        public List<Account> accounts = new List<Account>();
        private List<Transaction> transactions = new List<Transaction>();
        private string accountNumberSequence = "003452";
        private Random _random = new Random();

        public Atm()
        {
            LoadAccounts(accountsPath);
            LoadTransactions(transactionPath);
        }

        public void ViewAccounts()
        {
            if(!accounts.Any())
            {
                Console.WriteLine("There is currently no account in the system.");
                return;
            }

            foreach (var account in accounts)
            {
                Console.WriteLine($"{account.AccountName} {account.AccountNumber} {account.Balance.ToString("0.00")}");
            }
        }

        public void CreateAccount(string accountName, int accoutType, int pin)
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                AccountName = accountName,
                AccountNumber = $"{accountNumberSequence}{GenerateRandomNumbers()}",
                Balance = 0.0m,
                Pin = pin,
                AccountType = accoutType == 1 ? AccountTypeEnum.Savings.ToString() : AccountTypeEnum.Current.ToString(),
            };

            Console.WriteLine($"An account with account number {account.AccountNumber} has been created for you.");
            Console.WriteLine();

            accounts.Add(account);
            SaveAccounts();
        }

        public void Transact(string accountNumber, decimal amount, TransactionTypeEnum transactionType)
        {
            
            var account = accounts.Where(account => account.AccountNumber == accountNumber).SingleOrDefault();
            //Console.WriteLine($"Account: {account.AccountNumber} Name: {account.AccountName}");
            if (account == null)
            {
                Console.WriteLine("Account number provided does not exist, Please confirm and try again.");
                Console.WriteLine();
                return;
            }            

            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                TransactionDateTime = DateTime.Now,
                Amount = amount,
                AccountId = account.Id,
                TransactionType = transactionType.ToString(),
            };

            account.Balance =  transactionType == TransactionTypeEnum.Deposit ? account.Balance += amount : account.Balance -= amount;
            

            transactions.Add(transaction);
            SaveTransactions();
            SaveAccounts();
            if(transactionType == TransactionTypeEnum.Deposit)
            {
                Console.WriteLine("Successful.");
            }
            if(transactionType == TransactionTypeEnum.Withdrawal)
            {
                Console.WriteLine("Successful.");
            }
            if (transactionType == TransactionTypeEnum.Transfer)
            {
                Console.WriteLine();
            }
            
        }
        public static void CreateAccount(Atm atm)
        {
            int accountTypeInput = default;
            bool validPin = false;
            string pinInput = default;
            var validAccountTypeInput = false;

            Console.WriteLine("Please enter your full name ");
            var nameInput = Console.ReadLine();

            ProcessInput.CollecctAccountType(validAccountTypeInput, accountTypeInput);
            ProcessInput.CollectPin(validPin, out pinInput);

            atm.CreateAccount(nameInput, accountTypeInput, int.Parse(pinInput));
            Console.WriteLine("Account Creation Completed");
        }
       
        public static void Deposit(Atm atm)
        {
            bool validAccountNumber = false;
            bool validAmount = false;
            string accountNumber = default;
            string amount = default;
            
            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            ProcessInput.CollectDepAmount(validAmount, out amount);

            atm.Transact(accountNumber, decimal.Parse(amount), TransactionTypeEnum.Deposit);
        }

        public static void Withdraw(Atm atm)
        {
            bool validAccountNumber = false;
            bool validAmount = false;
            string accountNumber = default;
            string amount = default;

            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            ProcessInput.CollectWithAmount(validAmount, out amount);

            atm.Transact(accountNumber, decimal.Parse(amount), TransactionTypeEnum.Withdrawal);
        }

        public static void Transfer(Atm atm)
        {
            bool validAccountNumber = false;
            bool validAmount = false;
            string accountNumber = default;
            string amount = default;
            bool validPin = false;
            string pinInput = default;

            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            ProcessInput.CollectPin(validPin, out pinInput);
            Console.Write("Transfer amount.");
            ProcessInput.CollectWithAmount(validAmount, out amount);
            atm.Transact(accountNumber, decimal.Parse(amount), TransactionTypeEnum.Transfer);

            Console.Write("To transfer to, ");
            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            atm.Transact(accountNumber, decimal.Parse(amount),TransactionTypeEnum.Deposit);
            
        }

        public bool BalanceEnquiry(string accountNumber, int pin,out bool completed, out bool incorrectPin, out bool incorrectAccountNumber)
        {
            completed = default;
            incorrectPin = default;
            incorrectAccountNumber = default;

            var account = accounts.Where(account => account.AccountNumber == accountNumber).SingleOrDefault();

            if (account == null)
            {
                Console.WriteLine("Account number provided does not exist, Please confirm and try again.");
                Console.WriteLine();
                incorrectAccountNumber = true;
                return completed;
            }
            
            if(account.Pin != pin)
            {
                Console.WriteLine("Incorrect Pin!");
                Console.WriteLine();
                incorrectPin = true;
                return completed;
            }
            Console.WriteLine("Getting account balance.....");

            Console.WriteLine($"Your account balance is: {account.Balance.ToString("0.00")}");
            Console.WriteLine();
            completed = true;
            return completed;
        }
        public static void ViewBalance(Atm atm)
        {
            bool completed = false;                  
            bool validAccountNumber = false;
            bool validPin = false;
            bool incorrectAccountNumber = false;
            bool incorrectPin = false;
            string accountNumber = default;
            string pinInput = default;
            
            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            ProcessInput.CollectPin(validPin, out pinInput);

            while (!completed)
            {
                if(incorrectPin)
                {
                    ProcessInput.CollectPin(validPin, out pinInput);
                }
                if (incorrectAccountNumber)
                {
                    ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
                }
                atm.BalanceEnquiry(accountNumber, int.Parse(pinInput),out completed, out incorrectPin, out incorrectAccountNumber);
            }
        }
        public static void ViewAccount(Atm atm)
        {
            Console.WriteLine("Loading Accounts......");
            atm.ViewAccounts();
            Console.WriteLine();            
        }
        public void ViewTransactions(Atm atm)
        {

            bool completed = false;
            bool validAccountNumber = false;
            bool validPin = false;
            bool incorrectAccountNumber = false;
            bool incorrectPin = false;
            string accountNumber = default;
            string pinInput = default;

            ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
            ProcessInput.CollectPin(validPin, out pinInput);

            while (!completed)
            {
                if (incorrectPin)
                {
                    ProcessInput.CollectPin(validPin, out pinInput);
                }
                if (incorrectAccountNumber)
                {
                    ProcessInput.CollectAccountNumber(validAccountNumber, out accountNumber);
                }
                atm.BalanceEnquiry(accountNumber, int.Parse(pinInput), out completed, out incorrectPin, out incorrectAccountNumber);
            }
                                   
            var account = accounts.Where(account => account.AccountNumber == accountNumber).SingleOrDefault();
            var accountTransactions = transactions.Where(tr => tr.AccountId == account.Id);

            if (!accountTransactions.Any())
            {
                Console.WriteLine($"No transactions found for account {account.AccountNumber}");
                return;
            }
            Console.WriteLine($"Transaction history for {account.AccountName} with account {account.AccountNumber}:");
            foreach (var transaction in accountTransactions)
            {
                Console.WriteLine($"{transaction.Id}  |  {transaction.TransactionType}      |  {transaction.TransactionDateTime}  |  {transaction.Amount}");
            }

        }

        private string GenerateRandomNumbers()
        {
            return _random.Next(0, 9999).ToString("D4");
        }

        private void LoadAccounts(string accountsPath)
        {
            if (File.Exists(accountsPath))
            {
                var accountsLines = File.ReadAllLines(accountsPath);
                foreach (var line in accountsLines)
                {
                    var data = line.Split(",");
                    var account = new Account
                    {
                        Id = data[0],
                        AccountName = data[1],
                        AccountNumber = data[2],
                        AccountType = data[3],
                        Balance = decimal.Parse(data[4]),
                        Pin = int.Parse(data[5]) //TODO: Refactor code to not save pin in clear text
                    };
                    accounts.Add(account);
                }
            }
            else
            {
                File.Create(accountsPath);
            }
        }

        private void LoadTransactions(string transactionsPath)
        {
            if (File.Exists(transactionsPath))
            {
                var transactionLines = File.ReadAllLines(transactionsPath);
                foreach (var line in transactionLines)
                {
                    var data = line.Split(",");
                    var transaction = new Transaction
                    {
                        Id = data[0],
                        TransactionDateTime = DateTime.Parse(data[1]),
                        TransactionType = data[2],
                        Amount = decimal.Parse(data[3]),
                        AccountId = data[4] //TODO: Refactor code to not save pin in clear text
                    };
                    transactions.Add(transaction);
                }
            }
            else
            {
                File.Create(transactionsPath);
            }
        }

        private void SaveAccounts()
        {
            var contents = accounts.Select(x => $"{x.Id},{x.AccountName},{x.AccountNumber},{x.AccountType},{x.Balance},{x.Pin}");
            File.WriteAllLines(accountsPath, contents);
        }

        private void SaveTransactions()
        {
            var contents = transactions.Select(x => $"{x.Id},{x.TransactionDateTime},{x.TransactionType},{x.Amount},{x.AccountId}");
            File.WriteAllLines(transactionPath, contents);
        }

        public static void Run(Atm atm)
        {
            while (true)
            {
                Display.DisplayMenu();
                Console.Write("Please enter a value: ");
                var validInput = int.TryParse(Console.ReadLine(), out int input);
                if (!validInput)
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
                        Atm.Deposit(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;

                    case 3:
                        Atm.Withdraw(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;

                    case 4:
                        Atm.Transfer(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;
                    case 5:
                       
                        atm.ViewTransactions(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;
                    case 6:
                        Atm.ViewBalance(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;
                    case 7:
                        Atm.ViewAccount(atm);
                        if (ProcessInput.RetryCheck())
                            continue;
                        else
                            Environment.Exit(0);
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("You Entered an invalid Number");
                        break;
                }
            }
        }
    }
}
