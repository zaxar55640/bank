using Banks.Entities.Account.AccountExceptions;

namespace Banks.Entities.Account.Builder;

public class DepositAccountBuilder : IAccountBuilder
{
    private Guid? _accountId = null;
    private decimal? _lowPercent = null;
    private decimal? _middlePercent = null;
    private decimal? _highPercent = null;
    private DateTime? _unlockDate = null;
    private Client? _owner = null;
    private decimal? _suslimit;

    public IAccount Build()
    {
        if (!_accountId.HasValue) throw new AccountDataException("Account ID is missing");
        if (!_lowPercent.HasValue) throw new AccountDataException("Low percent is missing");
        if (!_middlePercent.HasValue)
            throw new AccountDataException("Middle percent is missing");
        if (!_highPercent.HasValue) throw new AccountDataException("High percent is missing");
        if (!_unlockDate.HasValue)
            throw new AccountDataException("Unlock date is missing");
        if (_owner is null)
            throw new AccountDataException("Owner is missing");
        if (!_suslimit.HasValue) throw new AccountDataException("Sus limit is missing");

        return new DepositAccount(_lowPercent.Value, _middlePercent.Value, _highPercent.Value, _accountId.Value, _unlockDate.Value, _owner, _suslimit.Value);
    }

    public IAccountBuilder SetAccountId(Guid id)
    {
        if (id == default) throw new AccountDataException("Can't use this ID for Deposit account.");
        _accountId = id;
        return this;
    }

    public IAccountBuilder SetOwner(Client owner)
    {
        _owner = owner ?? throw new AccountDataException("Owner is not existing");
        return this;
    }

    public IAccountBuilder SetLimit(decimal limit)
    {
        throw new AccountWrongOperation("Can't set limit for deposit account");
    }

    public IAccountBuilder SetSusLimit(decimal value)
    {
        if (value < 0) throw new AccountDataException("Can't set negative limit on account");
        _suslimit = value;
        return this;
    }

    public IAccountBuilder SetUnlockDate(DateTime dateTime)
    {
        _unlockDate = dateTime;
        return this;
    }

    public IAccountBuilder SetPercent(decimal percent)
    {
        throw new AccountWrongOperation("Can't set default percent for Deposit account, choose Low/Middle/High.");
    }

    public IAccountBuilder SetCommission(decimal commission)
    {
        throw new AccountWrongOperation("Can't set commission on deposit account.");
    }

    public IAccountBuilder SetLowPercent(decimal lowPercent)
    {
        if (lowPercent > _middlePercent)
        {
            throw new AccountDataException(
                $" LowPercent - {lowPercent} can not be more then MiddlePercent - {_middlePercent}");
        }

        if (lowPercent > _highPercent)
        {
            throw new AccountDataException(
                $" LowPercent - {lowPercent} can not be more then HighPercent - {_highPercent}");
        }

        _lowPercent = lowPercent;
        return this;
    }

    public IAccountBuilder SetMiddlePercent(decimal middlePercent)
    {
        if (middlePercent < _lowPercent)
        {
            throw new AccountDataException(
                $" LowPercent - {_lowPercent} can not be more then MiddlePercent - {_middlePercent}");
        }

        if (middlePercent > _highPercent)
        {
            throw new AccountDataException(
                $" middlePercent - {middlePercent} can not be more then HighPercent - {_highPercent}");
        }

        _middlePercent = middlePercent;
        return this;
    }

    public IAccountBuilder SetHighPercent(decimal highPercent)
    {
        if (highPercent < _lowPercent)
        {
            throw new AccountDataException(
                $" LowPercent - {_lowPercent} can not be more then highPercent - {highPercent}");
        }

        if (highPercent < _middlePercent)
        {
            throw new AccountDataException(
                $" middlePercent - {_middlePercent} can not be more then HighPercent - {_highPercent}");
        }

        _highPercent = highPercent;
        return this;
    }
}