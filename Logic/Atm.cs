using ATMSimulator.Entities;
using ATMSimulator.Entities.Enums;

namespace ATMSimulator.Logic
{
    public class Atm
    {
        private string accountsPath = "C:\\Users\\adauda\\Desktop\\ATMSimulator\\DataStore\\account.txt";
        private string transactionPath = "C:\\Users\\adauda\\Desktop\\ATMSimulator\\DataStore\\transaction.txt";
        private List<Account> accounts = new List<Account>();
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
            ProcessInput.CollecctPin(validPin, out pinInput);

            atm.CreateAccount(nameInput, accountTypeInput, int.Parse(pinInput));
            Console.WriteLine("Account Creation Completed");
        }

        public void BalanceEnquiry(string accountNumber, int pin)
        {
            var account = accounts.Where(account => account.AccountNumber == accountNumber).SingleOrDefault();
            if (account == null)
            {
                Console.WriteLine("Account number provided does not exist, Please confirm and try again.");
                Console.WriteLine();
                return;
            }

            if(account.Pin != pin)
            {
                Console.WriteLine("Incorrect Pin!");
                Console.WriteLine();
                return;
            }

            Console.WriteLine($"Your account balance is: {account.Balance.ToString("0.00")}");
            Console.WriteLine();
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
    }
}
