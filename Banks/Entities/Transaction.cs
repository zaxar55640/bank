namespace Banks.Entities;

public class Transaction
{
    public Transaction(Guid id, string operation)
    {
        TID = id;
        Operation = operation;
    }

    public Guid TID { get; }
    public string Operation { get; }
}