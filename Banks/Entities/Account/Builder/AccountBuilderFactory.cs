using Banks.Entities.Account.AccountExceptions;

namespace Banks.Entities.Account.Builder;

internal static class AccountBuilderFactory
{
    internal static IAccountBuilder Create(AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Credit => new CreditAccountBuilder(),
            AccountType.Debit => new DebitAccountBuilder(),
            AccountType.Deposit => new DepositAccountBuilder(),
            _ => throw new AccountDataException("Wrong account type given.")
        };
    }
}