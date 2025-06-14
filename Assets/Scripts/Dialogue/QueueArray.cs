using System;

public class QueueArray<T> : IQueue<T>
{
    private const int DefaultCapacity = 10; // Default initial size of the queue
    private int front = 0; // Index of the front element in the queue
    private int back = 0; // Index where the next element will be inserted
    private int size = 0; // Current number of elements in the queue
    
    private T[] items = new T[DefaultCapacity];
    
    // Adds an item to the back of the queue
    public void Enqueue(T item)
    {
        items[back] = item; // Store the item at the back index
        size++; // Increase size count
        back++; // Move back pointer forward

        if (back >= items.Length) // Wrap around if needed
        {
            back = 0;
        }

        if (back == front) // If queue is full, increase its capacity
        {
            IncreaseCapacity();
        }
    }

    // Removes and returns the front item of the queue
    public T Dequeue()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty");
        }
        
        T value = items[front]; // Retrieves the front item
        
        front++; // Move front pointer forward
        size--; // Decrease size count

        if (front >= items.Length) // Wrap around if needed
        {
            front = 0;
        }
        return value;
    }

    // Checks if the queue is empty
    public bool IsEmpty()
    {
        return size == 0;
    }
    
    // Returns the current number of elements in the queue
    public int GetSize()
    {
        return size;
    }

    // Doubles the capacity of the queue when full
    private void IncreaseCapacity()
    {
        T[] newItems = new T[items.Length * 2];

        int index = 0;
        while (!IsEmpty()) // Copy elements to new array
        {
            newItems[index] = Dequeue();
            index++;
        }
        
        front = 0; // Reset front pointer
        back = size = items.Length; // Adjust back and size pointers
        items = newItems; // Replace old array with new one
    }
}
