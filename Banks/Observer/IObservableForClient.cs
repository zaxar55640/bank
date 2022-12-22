namespace Banks.Observer;

public interface IObservableForClient
{
    void AddObserver(IObserverClient o);
    void RemoveObserver(IObserverClient o);
    void NotifyObservers(string info);
}