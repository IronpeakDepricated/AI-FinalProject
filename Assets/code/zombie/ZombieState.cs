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
        f = g + Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        this.node = node;
        if (node.CanReachPlayer)
        {
            this.g += Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        }
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.g = prev.g + connection.distance;
        this.node = connection.node;
        float dist = Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        this.f = this.g + dist;
        if(node.CanReachPlayer)
        {
            this.g += Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        }
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
