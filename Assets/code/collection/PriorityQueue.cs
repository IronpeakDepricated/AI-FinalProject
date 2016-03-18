public class PriorityQueue<T>
{
    class Node
    {
        public Node next;
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

        public Node(float weight, T item, Node next)
        {
            this.weight = weight;
            this.item = item;
            this.next = next;
        }
    }

    private int count;
    private Node front, back;

    public PriorityQueue()
    {
        count = 0;
        front = null;
        back = null;
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public void Push(float weight, T item)
    {
        if(IsEmpty())
        {
            front = new Node(weight, item, null);
            back = front;
            count++;
        }
        else
        {
            if(weight < front.Weight)
            {
                front = new Node(weight, item, front);
                count++;
            }
            else if(weight >= back.Weight)
            {
                back.next = new Node(weight, item, null);
                back = back.next;
                count++;
            }
            else
            {
                Node temp = front;
                while(temp.next.Weight <= weight)
                {
                    temp = temp.next;
                }
                temp.next = new Node(weight, item, temp.next);
                count++;
            }
        }
    }

    public T Pop()
    {
        count--;
        T ret = front.Item;
        front = front.next;
        return ret;
    }

    public T Peek()
    {
        T ret = front.Item;
        return ret;
    }

    public override string ToString()
    {
        string ret = "";
        Node temp = front;
        while(temp != null)
        {
            ret += temp.Weight + " ";
            temp = temp.next;
        }
        return ret;
    }
}
