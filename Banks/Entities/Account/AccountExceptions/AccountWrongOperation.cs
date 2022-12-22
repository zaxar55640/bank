namespace Banks.Entities.Account.AccountExceptions;

public class AccountWrongOperation : Exception
{
    public AccountWrongOperation(string message)
        : base(message)
    {
    }
}