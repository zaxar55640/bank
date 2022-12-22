using System.Runtime.CompilerServices;
using Banks.Entities.Account.AccountExceptions;
using Banks.Observer;

namespace Banks.Entities.Account;

public class DebitAccount : IAccount
{
    private decimal _percent;
    private decimal _percentPerMonthCount;
    private Guid _accountID;
    private decimal _balance;
    private decimal _suslimit;
    private List<Transaction> _transactions;
    private Client _owner;
    private DateTime date;

    public DebitAccount(decimal percent, Guid id, Client owner, decimal limit)
    {
        _percent = percent;
        _accountID = id;
        _balance = 0;
        _transactions = new List<Transaction>();
        _percentPerMonthCount = 0;
        _suslimit = limit;
        _owner = owner;
        date = DateTime.Now;
    }

    public decimal GetBalance()
    {
        return _balance;
    }

    public Guid GetAccountID()
    {
        return _accountID;
    }

    public void Deposit(decimal value)
    {
        if (_owner.Suspicious() && _suslimit < value)
            throw new AccountWrongOperation("Can't deposit more. Give passport information.");
        if (value < 0) throw new AccountWrongOperation("Can't deposit negative value");
        _balance += value;
        Guid tId = Guid.NewGuid();
        AddTransaction(tId, "+");
    }

    public void Withdrawal(decimal value)
    {
        if (_owner.Suspicious() && _suslimit < value)
            throw new AccountWrongOperation("Can't deposit more. Give passport information.");
        if (value < 0) throw new AccountWrongOperation("Can't deposit negative value");
        if (_balance < value) throw new AccountWrongOperation("Not enough money for withdraw");
        _balance -= value;
        Guid tId = Guid.NewGuid();
        AddTransaction(tId, "-");
    }

    public void Transfer(decimal value, IAccount accountToTransfer)
    {
        if (_owner.Suspicious() && _suslimit < value)
            throw new AccountWrongOperation("Can't deposit more. Give passport information.");
        if (value < 0) throw new AccountWrongOperation("Can't deposit negative value");
        if (_balance < value) throw new AccountWrongOperation("Not enough money for withdraw");
        _balance -= value;
        Guid tId = Guid.NewGuid();
        accountToTransfer.Deposit(value);
        accountToTransfer.AddTransaction(tId, "+");
        AddTransaction(tId, "-");
    }

    public void CancelTransaction(Guid trID)
    {
        if (_transactions.Any(t => t.TID == trID))
            _transactions.RemoveAll(t => t.TID == trID);
        else throw new AccountWrongOperation($"Can't find: {trID} transaction.");
    }

    public void TakeCommission()
    {
        throw new AccountWrongOperation("Can't take commission from debit account.");
    }

    public decimal AccruePercent()
    {
        _balance += _percentPerMonthCount;
        _percentPerMonthCount = 0;
        return _balance;
    }

    public void CountPercent(bool mode)
    {
        if (mode)
        {
            if (DateTime.Now < date.AddMonths(1))
            {
                _percentPerMonthCount += _percent / 365 * _balance;
            }
            else
            {
                AccruePercent();
                date = DateTime.Now;
            }
        }
        else
        {
            if (DateTime.Now < date.AddSeconds(31))
            {
                _percentPerMonthCount += _percent / 365 * _balance;
            }
            else
            {
                AccruePercent();
                date = DateTime.Now;
            }
        }
    }

    public void Update(bool mode = true)
    {
        CountPercent(mode);
    }

    public void ChangeCommission(decimal value)
    {
        if (value < 0) throw new AccountDataException("Value can't be negative");
        _percent = value;
    }

    public void ChangeLimit(decimal value) => _suslimit = value;
    public void AddTransaction(Guid id, string operation) => _transactions.Add(new Transaction(id, operation));
    public void CountCommission(bool mode = true) => throw new AccountWrongOperation("Can't count commission on debit account");
}