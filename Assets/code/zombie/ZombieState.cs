using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ZombieState
{
    public ZombieState prev;
    public Node node;
    public float distance;
    public float g;
    public float f;

    public ZombieState(Node node, float distance)
    {
        this.prev = null;
        this.distance = distance;
        this.g = distance + node.Selected * 10;
        this.node = node;
        this.f = this.g + node.DistanceToPlayer;
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.distance = prev.distance + connection.distance;
        this.g = prev.g + connection.distance + connection.node.Selected * 10;
        this.node = connection.node;
        this.f = this.g + node.DistanceToPlayer;
    }

    public Path ToPath()
    {
        List<Node> path = new List<Node>();
        AddToPath(this, path);
        return new Path(path, distance);
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
