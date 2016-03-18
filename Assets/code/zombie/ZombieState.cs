using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieState : IComparer<ZombieState>
{
    public ZombieState prev;
    public float distance;
    public Node node;

    public ZombieState(Node node, float distance)
    {
        prev = null;
        this.distance = distance;
        this.node = node;
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.distance = prev.distance + connection.distance;
        this.node = connection.node;
    }

    public int Compare(ZombieState x, ZombieState y)
    {
        if(x.distance < y.distance)
        {
            return -1;
        }
        if(x.distance > y.distance)
        {
            return 1;
        }
        return 0;
    }
}
