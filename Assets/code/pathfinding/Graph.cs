using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    public static Graph graph;
    public List<Node> graphNodes = new List<Node>();

    void Awake()
    {
        Graph.graph = this;
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
        return Node.CanReachNode(Player.player.transform.position, node);
    }
}
