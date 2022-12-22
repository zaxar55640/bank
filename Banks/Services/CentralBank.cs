using Banks.Entities;
using Banks.Entities.Account;
using Banks.Observer;
using Hangfire;

namespace Banks.Services;

public class CentralBank : IObservable
{
    private static CentralBank? cb;
    private readonly List<Bank> _banks;
    private readonly List<IObserver> _observers;

    public CentralBank(int time = 86400000, bool mode = true)
    {
        _banks = new List<Bank>();
        _observers = new List<IObserver>();
        Count = 1;
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(time);
                NotifyObservers(mode);
            }
        });
    }

    public int Count { get; set; }
    public static CentralBank GetInstance(int time = 86400000, bool mode = true)
    {
        if (cb == null)
        {
            cb = new CentralBank(time, mode);
        }

        return cb;
    }

    public Bank CreateBank(string name, decimal commission, decimal limit, decimal lowcom, decimal highcom, decimal percent, decimal suslim)
    {
        Guid id = Guid.NewGuid();
        Bank bank = new BankBuilder()
            .SetCommission(commission)
            .SetLimit(limit)
            .SetLowCommission(lowcom)
            .SetHighCommission(highcom)
            .SetName(name)
            .SetBankId(id)
            .SetPercent(percent)
            .SetSusLimit(suslim)
            .Build();
        _banks.Add(bank);
        AddObserver(bank);
        return bank;
    }

    public void TransactionBetweenBanks(IAccount accountFrom, IAccount to, decimal value)
    {
        accountFrom.Transfer(value, to);
    }

    public void AddObserver(IObserver o)
    {
        if (o == null) throw new BankDataException("Can't add this observer to Central Bank.");
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver o)
    {
        if (_observers.Contains(o)) throw new BankDataException("This observer is already in system of Central Bank.");
        _observers.Remove(o);
    }

    public void NotifyObservers(bool mode = true)
    {
        _observers.ForEach(p => p.Update(mode));
    }

    public IAccount? FindAccountById(Guid id)
    {
        IAccount? account = _banks.First(p => p.GetAccountByID(id) != null).GetAccountByID(id);
        return account;
    }

    public List<Bank> GetBanks()
    {
        return _banks;
    }
}