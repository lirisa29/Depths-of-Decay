public interface IQueue<T>{
    void Enqueue(T item);
    T Dequeue();
    bool IsEmpty();
    int GetSize();
}