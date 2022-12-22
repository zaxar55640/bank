using Banks.Entities.Account;
using Banks.Observer;

namespace Banks.Entities;

public class Client : IObserverClient
{
    private readonly List<IAccount> _accounts;
    private int _passport;
    public Client(string name, string adress, int passport = 0)
    {
        Name = name;
        Address = adress;
        _passport = passport;
        _accounts = new List<IAccount>();
    }

    public string Name { get; set; }
    public string Address { get; set; }

    public int GetPassport()
    {
        return _passport;
    }

    public void AddPassport(int passport)
    {
        if (passport == 0) throw new ClientDataException("Write correct passport number!");
        _passport = passport;
    }

    public bool Suspicious()
    {
        if (_passport == 0)
            return true;
        return false;
    }

    public void AddAccount(IAccount account)
    {
        if (_accounts.Contains(account)) throw new ClientDataException($"Client {Name} already has this account.");
        _accounts.Add(account);
    }

    public void Notification(string info)
    {
        Console.WriteLine(info);
    }

    public IReadOnlyList<Guid> GetClientAccountsByID()
    {
        IReadOnlyList<Guid> list = new List<Guid>();
        _accounts.ForEach(p => list.Append(p.GetAccountID()));
        return list;
    }
}