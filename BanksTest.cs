using Banks.Entities;
using Banks.Entities.Account;
using Banks.Entities.Account.AccountExceptions;
using Banks.Services;
using Xunit;
namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void CreateAccountsTransferMoney_AccountsCreatedMoneyTransfered()
    {
        CentralBank cb = CentralBank.GetInstance(1);
        Client cl1 = new Client("Zalhar", "Map", 123455);
        Client cl2 = new Client("zazar", "pam");
        Bank bank1 = cb.CreateBank("asd", 2, 10000, 1, 3, 10, 10000);
        Bank bank2 = cb.CreateBank("abas", 2, 10000, 1, 3, 10, 10000);
        IAccount a1 = bank1.CreateDebitAccount(cl1);
        IAccount a2 = bank1.CreateDepositAccount(cl2);
        IAccount a3 = bank2.CreateDepositAccount(cl2);
        a1.Deposit(10000);
        a1.Transfer(5000, a2);
        cb.TransactionBetweenBanks(a1, a3, 2000);
        Assert.Equal(5000, a2.GetBalance());
        Assert.Equal(3000, a1.GetBalance());
        Assert.Equal(2000, a3.GetBalance());
    }

    [Fact]
    public void CreateSusAccount_CantTransferOverLimit()
    {
        CentralBank cb = CentralBank.GetInstance(1);
        Client cl1 = new Client("Zalhar", "Map", 123455);
        Client cl2 = new Client("zazar", "pam");
        Bank bank1 = cb.CreateBank("asd", 2, 10000, 1, 3, 10, 10000);
        IAccount a1 = bank1.CreateDebitAccount(cl1);
        IAccount a2 = bank1.CreateDebitAccount(cl2);
        a2.Deposit(5000);
        Assert.Throws<AccountWrongOperation>(() => a2.Deposit(11000));
        Assert.Throws<AccountWrongOperation>(() => a2.Deposit(11000));
        a2.ChangeLimit(120000);
        a2.Deposit(120000);
        Assert.Equal(125000, a2.GetBalance());
    }
}