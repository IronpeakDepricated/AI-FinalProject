public class Queue<T>
{
    class Node
    {
        public T item;
        public Node next;

        public Node(T item, Node next = null)
        {
            this.item = item;
            this.next = next;
        }
    };

    private int count;
    private Node front;
    private Node back;

    public Queue()
    {
        count = 0;
        front = null;
        back = null;
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public void Push(T item)
    {
        if(count == 0)
        {
            back = new Node(item, null);
            front = back;
        }
        else
        {
            back.next = new Node(item, null);
            back = back.next;
        }
        count++;
    }

    public T Peek()
    {
        return front.item;
    }

    public T Pop()
    {
        T item = front.item;
        front = front.next;
        count--;
        return item;
    }

}
