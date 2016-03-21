using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    public static Graph graph;
    private List<Node> graphNodes = new List<Node>();
    private List<Node> viableNodes = new List<Node>();

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
            graphNodes[i].CanReachPlayer = CanReachPlayer(graphNodes[i]);
        }

        viableNodes.Clear();
        for(int i = 0; i < graphNodes.Count; i++)
        {
            if(graphNodes[i].IsViable())
            {
                viableNodes.Add(graphNodes[i]);
            }
        }
    }

    public bool[] GetMarked()
    {
        bool[] marked = new bool[graphNodes.Count];

        for(int i = 0; i < marked.Length; i++)
        {
            marked[i] = true;
        }

        for(int i = 0; i < viableNodes.Count; i++)
        {
            marked[viableNodes[i].ID] = false;
        }

        return marked;
    }

    public Node GetNode(int index)
    {
        return graphNodes[index];
    }

    public void AddNode(Node node)
    {
        graphNodes.Add(node);
    }

    public int TotalNodeCount()
    {
        return graphNodes.Count;
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
