﻿using UnityEngine;
using System.Collections.Generic;

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
        if(node.CanReachPlayer)
        {
            this.distance += Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        }
    }

    public ZombieState(ZombieState prev, NodeConnection connection)
    {
        this.prev = prev;
        this.distance = prev.distance + connection.distance;
        this.node = connection.node;
        if(node.CanReachPlayer)
        {
            this.distance += Vector3.Distance(node.Component.transform.position, Player.player.transform.position);
        }
    }

    public List<Vector3> ToPath()
    {
        List<Vector3> path = new List<Vector3>();
        AddToPath(this, path);
        return path;
    }

    void AddToPath(ZombieState state, List<Vector3> path)
    {
        if(state != null)
        {
            AddToPath(state.prev, path);
            path.Add(state.node.Component.transform.position);
        }
    }

}
