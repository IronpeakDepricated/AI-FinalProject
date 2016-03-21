using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieState
{
    public ZombieState prev;
    public float distance;
    public Node node;

    public ZombieState(Node node, float distance)
    {
        prev = null;
        this.distance = distance;
        this.node = node;
        if (node.CanReachPlayer)
        {
            this.distance += Vector3.Distance(node.transform.position, Player.player.transform.position);
        }
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.distance = prev.distance + connection.distance;
        this.node = connection.node;
        if (node.CanReachPlayer) {
            this.distance += Vector3.Distance(node.transform.position, Player.player.transform.position);
        }
    }
}
