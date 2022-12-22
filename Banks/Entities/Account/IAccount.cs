namespace Banks.Entities.Account;

public interface IAccount
{
    decimal GetBalance();
    Guid GetAccountID();
    void Deposit(decimal value);
    void Withdrawal(decimal value);
    void CancelTransaction(Guid trID);
    void TakeCommission();
    void CountCommission(bool mode);
    decimal AccruePercent();
    void CountPercent(bool mode);
    void Transfer(decimal value, IAccount accountToTransfer);
    void AddTransaction(Guid id, string operation);
    void ChangeLimit(decimal value);
    void ChangeCommission(decimal value);
    void Update(bool mode);
}
