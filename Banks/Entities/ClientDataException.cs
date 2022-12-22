namespace Banks.Entities;

public class ClientDataException : Exception
{
    public ClientDataException(string message)
        : base(message)
    {
    }
}