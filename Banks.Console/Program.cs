using Banks.Entities;
using Banks.Entities.Account;
using Banks.Services;

CentralBank cb = CentralBank.GetInstance();
Bank sberbank = cb.CreateBank("Sberbank", 2, 50000, 1, 3, 2, 10000);
Bank alfabank = cb.CreateBank("AlfaBank", 3, 10000, 1, 4, 1, 20000);
Bank vtb = cb.CreateBank("VTB", 3, 20000, 2, 5, 2, 10000);
Console.WriteLine("Write your Name and Surname:");
var name = Console.ReadLine();
Console.WriteLine("Write your Address:");
var address = Console.ReadLine();
Console.WriteLine("Do you want to add your passport number details to enhance your possibilities? Yes or No");
string? ans = Console.ReadLine();
Client user = new Client("setter", "setter");
if (address != null)
    user.Address = address;
if (name != null)
    user.Name = name;

int passport = 0;
if (ans == "Yes")
{
    Console.WriteLine("Write your passport number:");
    passport = Convert.ToInt32(Console.ReadLine());
    user.AddPassport(passport);
}

bool check = true;
while (check)
{
    Console.WriteLine(@"Write one of numbers below to give a system command:
    1 To create an account.
    2 To Transfer Money to Account
    3 To Deposit Money to Account
    4 Show your accounts id's
    5 to Quit
    ");
    var command = Console.ReadLine();
    switch (command)
    {
        case "1":
            Console.WriteLine(@"Which bank do you want to use?
            1 Sberbank 
            2 AlfaBank 
            3 VTB.
            ");
            var bank = Console.ReadLine();
            Bank cBank = sberbank;
            switch (bank)
            {
                case "1":
                    cBank = sberbank;
                    break;
                case "2":
                    cBank = alfabank;
                    break;
                case "3":
                    cBank = vtb;
                    break;
            }

            Console.WriteLine(@"Which type of account do you want to create?
            1 Credit 
            2 Debit 
            3 Deposit.
            ");
            var type = Console.ReadLine();
            switch (type)
            {
                case "1":
                    cBank.CreateCreditAccount(user);
                    break;
                case "2":
                    cBank.CreateDebitAccount(user);
                    break;
                case "3":
                    cBank.CreateDepositAccount(user);
                    break;
            }

            Console.WriteLine("You account had been Created!");
            break;
        case "2":
            Console.WriteLine("Write account's id to transfer money from.");
            IAccount? from = cb.FindAccountById(new Guid(Console.ReadLine() ?? throw new InvalidOperationException()));
            Console.WriteLine("Write account's id to transfer money to.");
            IAccount? to = cb.FindAccountById(new Guid(Console.ReadLine() ?? throw new InvalidOperationException()));
            Console.WriteLine("Write amount to transfer.");
            int value = Convert.ToInt32(Console.ReadLine());
            if (from != null && to != null) cb.TransactionBetweenBanks(from, to, value);
            Console.WriteLine("Success!");
            break;
        case "3":
            Console.WriteLine("Write account's id to deposit money to.");
            IAccount? acc = cb.FindAccountById(new Guid(Console.ReadLine() ?? throw new InvalidOperationException()));
            Console.WriteLine("Write amount to transfer.");
            int valueToDeposit = Convert.ToInt32(Console.ReadLine());
            acc?.Deposit(valueToDeposit);
            break;
        case "4":
            IReadOnlyList<Guid> useraccounts = user.GetClientAccountsByID();
            Console.WriteLine($"{useraccounts}");
            break;
        case "5":
            check = false;
            break;
    }
}