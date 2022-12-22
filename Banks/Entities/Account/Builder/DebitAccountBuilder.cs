using Banks.Entities.Account.AccountExceptions;

namespace Banks.Entities.Account.Builder;

public class DebitAccountBuilder : IAccountBuilder
{
    private decimal? _percent;
    private Guid? _accountID;
    private Client? _owner;
    private decimal? _suslimit;

    public IAccount Build()
    {
        if (!_percent.HasValue) throw new AccountDataException($"Not Enough Data: {_percent} is missing.");
        if (!_accountID.HasValue) throw new AccountDataException($"Not Enough Data: {_accountID} is missing.");
        if (!_suslimit.HasValue) throw new AccountDataException($"Not Enough Data: limit for sus accounts is missing.");
        if (_owner is null)
            throw new AccountDataException("Owner is missing");
        return new DebitAccount(_percent.Value, _accountID.Value, _owner, _suslimit.Value);
    }

    public IAccountBuilder SetOwner(Client owner)
    {
        _owner = owner ?? throw new AccountDataException("Owner is not existing");
        return this;
    }

    public IAccountBuilder SetAccountId(Guid id)
    {
        if (id == default) throw new AccountDataException($"Can't set this ID for account, try another one");
        _accountID = id;
        return this;
    }

    public IAccountBuilder SetLimit(decimal limit)
    {
        throw new AccountWrongOperation("Can't set a limit on a debit account");
    }

    public IAccountBuilder SetSusLimit(decimal value)
    {
        if (value < 0) throw new AccountDataException("Can't set negative limit on account");
        _suslimit = value;
        return this;
    }

    public IAccountBuilder SetUnlockDate(DateTime dateTime)
    {
        throw new AccountWrongOperation("Can't set an unlock time on a debit account");
    }

    public IAccountBuilder SetPercent(decimal percent)
    {
        if (percent < 0) throw new AccountDataException($"Percent can't be negative");
        _percent = percent;
        return this;
    }

    public IAccountBuilder SetCommission(decimal commission)
    {
        throw new AccountWrongOperation("Can't set commission on a debit account");
    }

    public IAccountBuilder SetLowPercent(decimal lowPercent)
    {
        throw new AccountWrongOperation("Can't set commission on a debit account");
    }

    public IAccountBuilder SetMiddlePercent(decimal middlePercent)
    {
        throw new AccountWrongOperation("Can't set commission on a debit account");
    }

    public IAccountBuilder SetHighPercent(decimal highPercent)
    {
        throw new AccountWrongOperation("Can't set commission on a debit account");
    }
}