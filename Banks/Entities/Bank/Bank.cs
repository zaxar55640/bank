using Banks.Entities.Account;
using Banks.Entities.Account.Builder;
using Banks.Observer;

namespace Banks.Entities;

public class Bank : IObserver, IObservableForClient
{
    private readonly List<Client> _clients;
    private readonly List<IAccount> _accounts;
    private readonly List<IObserverClient> _observers;
    private string _name;
    private Guid _bankId;

    public Bank(string name, Guid bankId, decimal lim, decimal commission, decimal lowCommission, decimal highCommission, decimal percent, decimal suslimit)
    {
        _clients = new List<Client>();
        _accounts = new List<IAccount>();
        _name = name;
        _bankId = bankId;
        Limit = lim;
        Commission = commission;
        LowCommission = lowCommission;
        HighCommission = highCommission;
        Percent = percent;
        _observers = new List<IObserverClient>();
        SusLimit = suslimit;
    }

    public decimal Limit { get; set; }
    public decimal Commission { get; set; }
    public decimal LowCommission { get; set; }
    public decimal HighCommission { get; set; }
    public decimal Percent { get; }
    public decimal SusLimit { get; set; }
    public Client AddClient(Client client)
    {
        if (_clients.Contains(client)) throw new BankDataException($"Client {client} is already exists in bank system.");
        _clients.Add(client);
        return client;
    }

    public void AddAccountToClient(IAccount account, Client client)
    {
        if (!_clients.Contains(client)) throw new BankDataException($"Client {client} is not registered in bank.");
        client.AddAccount(account);
        _accounts.Add(account);
    }

    public Client GetClient(Client client)
    {
        if (_clients.Contains(client)) throw new BankDataException($"Client {client} is not registered in bank.");
        return client;
    }

    public IAccount GetAccount(IAccount account)
    {
        if (_accounts.Contains(account)) throw new BankDataException($"Account {account} is not registered in bank.");
        return account;
    }

    public IAccount? GetAccountByID(Guid id)
    {
        return _accounts.FirstOrDefault(p => p.GetAccountID() == id);
    }

    public void ChangeAccountsLimit(decimal newlim)
    {
        if (newlim < 0) throw new BankDataException("Limit can't be negative");
        _accounts.ForEach(p => p.ChangeLimit(newlim));
        Limit = newlim;
        NotifyObservers($"Bank {_name} has changed limits to {newlim}.");
    }

    public void ChangePercent(decimal value)
    {
        if (value < 0)
            throw new BankDataException("Can't change percent to negative value.");
        _accounts.ForEach(p => p.ChangeCommission(value));
        NotifyObservers($"Bank {_name} has changed percent to {value}.");
    }

    public IAccount CreateCreditAccount(Client client)
    {
        var accountId = Guid.NewGuid();
        IAccount account = AccountBuilderFactory.Create(AccountType.Credit)
            .SetAccountId(accountId)
            .SetCommission(Commission)
            .SetLimit(Limit)
            .SetSusLimit(SusLimit)
            .SetOwner(client)
            .Build();
        _accounts.Add(account);
        if (!_clients.Contains(client))
        {
            _clients.Add(client);
        }

        client.AddAccount(account);
        return account;
    }

    public IAccount CreateDepositAccount(Client client)
    {
        var accountId = Guid.NewGuid();
        DateTime time = DateTime.Now.AddYears(1);
        IAccount account = AccountBuilderFactory.Create(AccountType.Deposit)
            .SetAccountId(accountId)
            .SetLowPercent(LowCommission)
            .SetMiddlePercent(Commission)
            .SetHighPercent(HighCommission)
            .SetUnlockDate(time)
            .SetOwner(client)
            .SetSusLimit(SusLimit)
            .Build();
        _accounts.Add(account);
        if (!_clients.Contains(client))
        {
            _clients.Add(client);
        }

        client.AddAccount(account);

        return account;
    }

    public IAccount CreateDebitAccount(Client client)
    {
        var accountId = Guid.NewGuid();
        IAccount account = AccountBuilderFactory.Create(AccountType.Debit)
            .SetAccountId(accountId)
            .SetPercent(Commission)
            .SetOwner(client)
            .SetSusLimit(SusLimit)
            .Build();
        _accounts.Add(account);
        if (!_clients.Contains(client))
        {
            _clients.Add(client);
        }

        client.AddAccount(account);
        return account;
    }

    public void Update(bool mode)
    {
        _accounts.ForEach(p => p.Update(mode));
    }

    public void AddObserver(IObserverClient o) => _observers.Add(o);
    public void RemoveObserver(IObserverClient o) => _observers.Remove(o);
    public void NotifyObservers(string info) => _observers.ForEach(p => p.Notification(info));
    public List<IAccount> GetAccounts() => _accounts;
}