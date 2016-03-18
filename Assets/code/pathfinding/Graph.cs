﻿using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    public static Graph graph;
    public List<Node> graphNodes = new List<Node>();

    void Awake()
    {
        Graph.graph = this;
    }

    void Start()
    {
        for(int i = 0; i < graphNodes.Count; i++)
        {
            graphNodes[i].ID = i;
        }
    }
   
    void Update()
    {
        for(int i = 0; i < graphNodes.Count; i++)
        {
            graphNodes[i].ReachPlayer = CanReachPlayer(graphNodes[i]);
        }
    }

    bool CanReachPlayer(Node node)
    {
        return Node.CanReach(Player.player.transform.position, node.transform.position);
    }

    public void OnDrawGizmosSelected()
    {
        if(Application.isPlaying == false)
            return;
        for(int i = 0; i < graphNodes.Count; i++)
        {
            graphNodes[i].OnDrawGizmosSelected();
        }
    }
}
