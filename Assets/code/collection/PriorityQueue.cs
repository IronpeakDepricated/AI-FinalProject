using System;

public class PriorityQueue<T>
{
    // heavily borrowed from http://algs4.cs.princeton.edu/24pq/MaxPQ.java.html
    // since c# has no built in PQ implementation

    class Node
    {
        private float weight;
        private T item;

        public float Weight
        {
            get
            {
                return weight;
            }
        }

        public T Item
        {
            get
            {
                return item;
            }
        }

        public Node(float weight, T item)
        {
            this.weight = weight;
            this.item = item;
        }
    }

    private int count;
    private Node[] arr;

    public PriorityQueue()
    {
        count = 0;
        arr = new Node[8];
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public int Size() {
        return count;
    }

    private void resize(int capacity)
    {
        if (capacity <= count)
        {
            throw new Exception("invalid resize");
        }
        Node[] temp = new Node[capacity];
        for (int i = 1; i <= count; i++)
        {
            temp[i] = arr[i];
        }
        arr = temp;
    }

    public void Push(float key, T val) {
        Node it = new Node(key, val);
        if (count >= arr.Length - 1) resize(2 * arr.Length);
        arr[++count] = it;
        swim(count);
    }

    public T Pop()
    {
        if (IsEmpty()) return default(T);
        T max = arr[1].Item;
        swap(1, count--);
        sink(1);
        arr[count + 1] = null; 
        if ((count > 0) && (count == (arr.Length - 1) / 4)) resize(arr.Length / 2);
        return max;
    }

    public T Peek()
    {
        return arr[1].Item;
    }
    

    private void swim(int k) {
        while (k > 1 && greater(k / 2, k))
        {
            swap(k, k / 2);
            k = k / 2;
        }
    }


    private void sink(int k)
    {
        while (2 * k <= count)
        {
            int j = 2 * k;
            if (j < count && greater(j, j + 1)) j++;
            if (!greater(k, j)) break;
            swap(k, j);
            k = j;
        }
    }

    private void swap(int i, int j) {
        Node tmp = arr[i];
        arr[i] = arr[j];
        arr[j] = tmp;
    }

    private bool greater(int i, int j) {
        return arr[i].Weight > arr[j].Weight;
    }
}
