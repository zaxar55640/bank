using Banks.Entities.Account.AccountExceptions;

namespace Banks.Entities.Account.Builder;

public class CreditAccountBuilder : IAccountBuilder
{
    private decimal? _limit = null;
    private decimal? _commission = null;
    private Guid? _accountID = null;
    private Client? _owner = null;
    private decimal? _suslimit;

    public IAccount Build()
    {
        if (!_limit.HasValue) throw new AccountDataException($"Not Enough Data: {_limit} is missing.");
        if (!_commission.HasValue) throw new AccountDataException($"Not Enough Data: {_commission} is missing.");
        if (!_accountID.HasValue) throw new AccountDataException($"Not Enough Data: {_accountID} is missing.");
        if (!_suslimit.HasValue) throw new AccountDataException($"Not Enough Data: {_suslimit} is missing.");
        if (_owner is null)
            throw new AccountDataException("Owner is missing");
        return new CreditAccount(_limit.Value, _commission.Value, _accountID.Value, _owner, _suslimit.Value);
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
        _limit = limit;
        return this;
    }

    public IAccountBuilder SetSusLimit(decimal value)
    {
        if (value < 0) throw new AccountDataException("Can't set negative limit on account");
        _suslimit = value;
        return this;
    }

    public IAccountBuilder SetUnlockDate(DateTime dateTime)
    {
        throw new AccountWrongOperation($"You can't set Unlock Date for credit account.");
    }

    public IAccountBuilder SetPercent(decimal percent)
    {
        throw new AccountWrongOperation($"You can't set any percent for credit account.");
    }

    public IAccountBuilder SetCommission(decimal commission)
    {
        if (commission < 0) throw new AccountDataException($"Can't set this ID for account, try another one");
        _commission = commission;
        return this;
    }

    public IAccountBuilder SetLowPercent(decimal lowPercent)
    {
        throw new AccountWrongOperation($"You can't set any percent for credit account.");
    }

    public IAccountBuilder SetMiddlePercent(decimal middlePercent)
    {
        throw new AccountWrongOperation($"You can't set any percent for credit account.");
    }

    public IAccountBuilder SetHighPercent(decimal highPercent)
    {
        throw new AccountWrongOperation($"You can't set any percent for credit account.");
    }
}