namespace Banks.Observer;

public interface IObservable
{
    void AddObserver(IObserver o);
    void RemoveObserver(IObserver o);
    void NotifyObservers(bool mode);
}