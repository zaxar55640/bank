using Banks.Entities.Account.AccountExceptions;
using Banks.Observer;

namespace Banks.Entities.Account;

public class CreditAccount : IAccount
{
    private decimal _limit;
    private decimal _commission;
    private decimal _commissionCount;
    private Guid _accountID;
    private decimal _balance = 0;
    private List<Transaction> _transactions;
    private decimal _suslimit;
    private Client _owner;
    private DateTime date;

    public CreditAccount(decimal limit, decimal commission, Guid accountId, Client owner, decimal suslimit)
    {
        _commission = commission;
        _limit = limit;
        _accountID = accountId;
        _transactions = new List<Transaction>();
        _commissionCount = 0;
        _suslimit = suslimit;
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
        if (_balance - value < -1 * _limit) throw new AccountWrongOperation("Not enough money for withdraw");
        _balance -= value;
        Guid tId = Guid.NewGuid();
        AddTransaction(tId, "-");
    }

    public void CancelTransaction(Guid trID)
    {
        if (_transactions.Any(t => t.TID == trID))
            _transactions.RemoveAll(t => t.TID == trID);
        else throw new AccountWrongOperation($"Can't find: {trID} transaction.");
    }

    public void CountCommission(bool mode)
    {
        if (mode)
        {
            if (DateTime.Now < date.AddMonths(1))
            {
                if (_balance < 0)
                {
                    _commissionCount += _commission / 365 * Math.Abs(_balance);
                }
            }
            else
            {
                TakeCommission();
                date = DateTime.Now;
            }
        }
        else
        {
            if (DateTime.Now < date.AddSeconds(31))
            {
                if (_balance < 0)
                {
                    _commissionCount += _commission / 365 * Math.Abs(_balance);
                }
            }
            else
            {
                TakeCommission();
                date = DateTime.Now;
            }
        }
    }

    public void TakeCommission()
    {
        _balance -= _commissionCount;
        _commissionCount = 0;
    }

    public void ChangeCommission(decimal value)
    {
        if (value < 0) throw new AccountDataException("Value can't be negative");
        _commission = value;
    }

    public void Transfer(decimal value, IAccount accountToTransfer)
    {
        if (_owner.Suspicious() && _suslimit < value)
            throw new AccountWrongOperation("Can't deposit more. Give passport information.");
        if (value < 0) throw new AccountWrongOperation("Can't deposit negative value");
        if (Math.Abs(_balance - value) > _limit) throw new AccountWrongOperation("Not enough money for transfer");
        _balance -= value;
        Guid tId = Guid.NewGuid();
        accountToTransfer.Deposit(value);
        accountToTransfer.AddTransaction(tId, "+");
        AddTransaction(tId, "-");
    }

    public void Update(bool mode)
    {
        CountCommission(mode);
    }

    public void ChangeLimit(decimal value) => _suslimit = value;
    public decimal AccruePercent() => throw new AccountWrongOperation("Can't count percent on credit account");
    public void CountPercent(bool mode = true) => throw new AccountWrongOperation("Can't count percent on credit account");
    public void AddTransaction(Guid id, string operation) => _transactions.Add(new Transaction(id, operation));
}