namespace Banks.Entities;

public class BankDataException : Exception
{
    public BankDataException(string message)
        : base(message)
    {
    }
}