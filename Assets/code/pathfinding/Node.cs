using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Node
{
    [System.Serializable]
    public class NodeSelected
    {
        float timeselected;
        float duration;
        Zombie zombie;

        public Zombie Zombie
        {
            get
            {
                return zombie;
            }
        }

        public NodeSelected(Zombie zombie, float currenttime, float duration)
        {
            this.timeselected = currenttime;
            this.duration = duration;
            this.zombie = zombie;
        }

        public void Select(float currenttime, float duration)
        {
            this.timeselected = currenttime;
            this.duration = duration;
        }

        public bool Expired(float currenttime)
        {
            return currenttime >= timeselected + duration;
        }

    }

    public int ID;
    public int DepthFromPlayer;
    public float DistanceToPlayer;
    public List<NodeConnection> adjNodes = new List<NodeConnection>();
    public int Selected;
    public int subIndex;
    public NodeComponent Component;
    public Queue<NodeSelected> Selections;

    public bool CanReachPlayer
    {
        get
        {
            return DepthFromPlayer == 0;
        }
    }

    public Node(NodeComponent Component)
    {
        Selections = new Queue<NodeSelected>();
        Graph.graph.GraphNodes.Add(this);
        this.DepthFromPlayer = 0;
        this.Component = Component;
        this.DistanceToPlayer = 0;
        this.Selected = 0;
        this.ID = 0;
    }

    public void Select(Zombie zombie, float duration)
    {
        int count = Selections.Size();
        for(int i = 0; i < count; i++)
        {
            NodeSelected node = Selections.Pop();
            if(node.Zombie == zombie)
            {
                node.Select(Time.timeSinceLevelLoad, duration);
                Selections.Push(node);
                return;
            }
            Selections.Push(node);
        }
        Selections.Push(new NodeSelected(zombie, Time.timeSinceLevelLoad, duration));
        Selected++;
    }

    public void Deselect(Zombie zombie)
    {
        int count = Selections.Size();
        for(int i = 0; i < count; i++)
        {
            NodeSelected node = Selections.Pop();
            if(node.Zombie == zombie)
            {
                Selected--;
                return;
            }
            Selections.Push(node);
        }
    }

    public void DeselectExpired()
    {
        if(Selections.IsEmpty())
        {
            return;
        }
        int count = Mathf.Max(Selections.Size() / 10, 1);
        for(int i = 0; i < count; i++)
        {
            NodeSelected node = Selections.Pop();
            if(node.Expired(Time.timeSinceLevelLoad) == false)
            {
                Selections.Push(node);
            }
            else
            {
                Selected--;
            }
        }
    }

    public void FindAdjNodes(List<Node> nodes)
    {
        for(int i = 0; i < Graph.graph.GraphNodes.Count; i++)
        {
            if(this != Graph.graph.GraphNodes[i] && CanReach(Component.transform.position, Graph.graph.GraphNodes[i].Component.transform.position, LayerMasks.FindAdjNodes))
            {
                adjNodes.Add(new NodeConnection(Graph.graph.GraphNodes[i], Vector3.Distance(Component.transform.position, Graph.graph.GraphNodes[i].Component.transform.position)));
            }
        }
    }

    public static bool CanReach(Vector3 origin, Vector3 target, LayerMask mask)
    {
        Vector3 x = target - origin;
        Vector3 cross = Vector3.Cross(x, Vector3.up);
        cross = cross.normalized * 0.49f;
        Vector3 a1 = target + cross;
        Vector3 b1 = origin + cross;
        if(Physics.Linecast(a1, b1, mask))
        {
            return false;
        }
        Vector3 a2 = target - cross;
        Vector3 b2 = origin - cross;
        if(Physics.Linecast(a2, b2, mask))
        {
            return false;
        }
        return true;
    }

    public bool IsInAdjList(Node n) {
        foreach (NodeConnection nc in adjNodes) {
            if (nc.node.Equals(n)) {
                return true;
            }
        }
        return false;
    }

}
