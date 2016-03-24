using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ZombieState
{
    public ZombieState prev;
    public Node node;
    public float g;
    public float f;

    public ZombieState(Node node, float distance)
    {
        prev = null;
        g = distance;
        this.node = node;
        f = g + node.DistanceToPlayer + node.Selected;
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.g = prev.g + connection.distance;
        this.node = connection.node;
        this.f = this.g + node.DistanceToPlayer + node.Selected;
    }

    public List<Node> ToPath()
    {
        List<Node> path = new List<Node>();
        AddToPath(this, path);
        return path;
    }

    void AddToPath(ZombieState state, List<Node> path)
    {
        if(state != null)
        {
            AddToPath(state.prev, path);
            path.Add(state.node);
        }
    }

}
