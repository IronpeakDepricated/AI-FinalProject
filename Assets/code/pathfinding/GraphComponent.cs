using UnityEngine;
using System.Collections;

public class GraphComponent : MonoBehaviour
{

    public Graph Graph;

    void Awake()
    {
        Graph = new Graph(this);
    }

    void Start()
    {
        Graph.SetNodeIDs();
        Graph.GenerateGraph();
        Graph.SetPlayerReachableNodes();
    }

    void Update()
    {
        Graph.SetPlayerReachableNodes();
        Graph.GenerateSubgraphs(Player.player);
    }

    public bool CanReachPlayer(Node node)
    {
        return Node.CanReach(Player.player.transform.position, node.Component.transform.position);
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying == false)
            return;
        for (int i = 0; i < Graph.GraphNodes.Count; i++)
        {
            Graph.GraphNodes[i].Component.OnDrawGizmosSelected();
        }
    }

}
