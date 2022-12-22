namespace Banks.Entities.Account.AccountExceptions;

public class AccountDataException : Exception
{
    public AccountDataException(string message)
        : base(message)
    {
    }
}