namespace Triangulation.Algorithm.PriorityQueue
{
    public interface IPriorityQueue<T>
    {
        int Count { get; }
        void Enqueue(T item);
        T Peek();
        T Dequeue();
        void Update(T item);
    }
}