using Banks.Entities.Account.AccountExceptions;
using Banks.Observer;

namespace Banks.Entities.Account;

public class DepositAccount : IAccount
{
    private decimal _amountForLowPercent = 50000;
    private decimal _amountForMiddleAndHighPercent = 100000;
    private decimal _balance;
    private Guid _accountId;
    private decimal _lowPercent;
    private decimal _middlePercent;
    private decimal _highPercent;
    private DateTime _depositUnlockDate;
    private List<Transaction> _transactions;
    private decimal _percentPerMonthCount;
    private decimal _suslimit;
    private Client _owner;
    private DateTime date;
    public DepositAccount(
        decimal lowPercent,
        decimal middlePercent,
        decimal highPercent,
        Guid accountId,
        DateTime depositUnlockDate,
        Client owner,
        decimal suslimit)
    {
        if (lowPercent > middlePercent) throw new AccountDataException("LowPercent can't be more then MiddlePercent");
        if (middlePercent > highPercent) throw new AccountDataException("middlePercent can't be more then highPercent");
        if (lowPercent > highPercent) throw new AccountDataException("LowPercent can't be more then highPercent");
        if (depositUnlockDate < DateTime.Now) throw new AccountDataException("Unlock date should be in future");
        _balance = 0;
        _lowPercent = lowPercent;
        _middlePercent = middlePercent;
        _highPercent = highPercent;
        _accountId = accountId;
        _depositUnlockDate = depositUnlockDate;
        _transactions = new List<Transaction>();
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
        return _accountId;
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
        if (DateTime.Now < _depositUnlockDate)
            throw new AccountWrongOperation($"Can't withdraw until {_depositUnlockDate}");
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
        if (DateTime.Now < _depositUnlockDate)
            throw new AccountWrongOperation($"Can't transfer money until {_depositUnlockDate}");
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
                decimal percent = 0;
                if (_balance < _amountForLowPercent)
                {
                    percent = _lowPercent;
                }
                else if (_balance > _amountForMiddleAndHighPercent)
                {
                    percent = _highPercent;
                }
                else
                {
                    percent = _middlePercent;
                }

                _percentPerMonthCount += percent / 365 * _balance;
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
                decimal percent = 0;
                if (_balance < _amountForLowPercent)
                {
                    percent = _lowPercent;
                }
                else if (_balance > _amountForMiddleAndHighPercent)
                {
                    percent = _highPercent;
                }
                else
                {
                    percent = _middlePercent;
                }

                _percentPerMonthCount += percent / 365 * _balance;
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
        if (value < 0) throw new AccountDataException("Commission can't be negative.");
        _middlePercent = value;
        _lowPercent = value - 0.5M;
        _highPercent = value + 0.5M;
    }

    public void ChangeLimit(decimal value) => _suslimit = value;
    public void AddTransaction(Guid id, string operation) => _transactions.Add(new Transaction(id, operation));
    public void CountCommission(bool mode = true) => throw new AccountWrongOperation("Can't count commission on debit account");
}