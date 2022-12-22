namespace Banks.Entities;

public class BankBuilder
{
    private string? _name;
    private Guid? _bankId;
    private decimal? _limit;
    private decimal? _commission;
    private decimal? _lowcommission;
    private decimal? _highcommission;
    private decimal? _percent;
    private decimal? _suslimit;
    public Bank Build()
    {
        if (_name == string.Empty || _name == null) throw new BankDataException($"Not Enough Data: name is missing.");
        if (!_bankId.HasValue) throw new BankDataException($"Not Enough Data: _bankId is missing.");
        if (!_limit.HasValue) throw new BankDataException($"Not Enough Data: _limit is missing.");
        if (!_commission.HasValue) throw new BankDataException($"Not Enough Data: commission is missing.");
        if (!_lowcommission.HasValue) throw new BankDataException($"Not Enough Data: low commission is missing.");
        if (!_highcommission.HasValue) throw new BankDataException($"Not Enough Data: high commission is missing.");
        if (!_percent.HasValue) throw new BankDataException($"Not Enough Data: percent is missing.");
        if (!_suslimit.HasValue) throw new BankDataException($"Not Enough Data: sus limit is missing.");
        return new Bank(
            _name,
            _bankId.Value,
            _limit.Value,
            _commission.Value,
            _lowcommission.Value,
            _highcommission.Value,
            _percent.Value,
            _suslimit.Value);
    }

    public BankBuilder SetName(string name)
    {
        if (name == string.Empty) throw new BankDataException($"You can't set empty string as a name for a bank.");
        _name = name;
        return this;
    }

    public BankBuilder SetBankId(Guid id)
    {
        if (id == default) throw new BankDataException($"Can't set this ID for account, try another one.");
        _bankId = id;
        return this;
    }

    public BankBuilder SetLimit(decimal limit)
    {
        if (limit < 0) throw new BankDataException($"Can't set negative limit.");
        _limit = limit;
        return this;
    }

    public BankBuilder SetSusLimit(decimal limit)
    {
        if (limit < 0) throw new BankDataException($"Can't set negative limit.");
        _suslimit = limit;
        return this;
    }

    public BankBuilder SetCommission(decimal commission)
    {
        if (commission < 0) throw new BankDataException($"Can't set negative commission.");
        _commission = commission;
        return this;
    }

    public BankBuilder SetLowCommission(decimal commission)
    {
        if (commission < 0) throw new BankDataException($"Can't set negative commission.");
        _lowcommission = commission;
        return this;
    }

    public BankBuilder SetHighCommission(decimal commission)
    {
        if (commission < 0) throw new BankDataException($"Can't set negative commission.");
        _highcommission = commission;
        return this;
    }

    public BankBuilder SetPercent(decimal percent)
    {
        if (percent < 0) throw new BankDataException($"Can't set negative percent.");
        _percent = percent;
        return this;
    }
}