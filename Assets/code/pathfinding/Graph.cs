using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Graph : IGraph
{

    public static Graph graph;

    public GraphComponent Component;
    public List<Node> GraphNodes { get; set; }
    public List<SubGraph> SubGraphs;

    GameObject[] triggers;

    public Graph(GraphComponent Component)
    {
        Graph.graph = this;
        this.Component = Component;
        GraphNodes = new List<Node>();
        triggers = new GameObject[1];
    }

    public void SetNodeIDs()
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].ID = i;
        }
    }

    public void GenerateGraph()
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].FindAdjNodes(GraphNodes);
        }
    }

    public void SetPlayerReachableNodes()
    {
        for (int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].CanReachPlayer = Component.CanReachPlayer(GraphNodes[i]);
        }
    }

    public void GenerateSubgraphs(Player player)
    {
        CreateTriggers(player);



        SubGraphs = new List<SubGraph>();
        SubGraphs.Add(new SubGraph(GraphNodes));
    }

    public SubGraph GetSubgraph(Vector3 position)
    {
        return SubGraphs[0];
    }

    GameObject[] CreateTriggers(Player player)
    {
        for(int i = 0; i < triggers.Length; i++)
        {
            GameObject.Destroy(triggers[i]);
        }
        List<Node> reachnodes = GetViableNodes(player);
        triggers  = new GameObject[reachnodes.Count];

        for(int i = 0; i < reachnodes.Count; i++)
        {
            triggers[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 direction = reachnodes[i].Component.transform.position - player.transform.position;
            RaycastHit hit;
            Physics.Raycast(player.transform.position, direction, out hit);
            Vector3 delta = hit.point - player.transform.position;
            triggers[i].transform.position = player.transform.position + delta / 2;
            triggers[i].transform.LookAt(player.transform.position);
            triggers[i].transform.localScale = new Vector3(0.1f, 0.1f, hit.distance);
            triggers[i].GetComponent<MeshRenderer>().material.color = Color.cyan;
            triggers[i].layer = LayerMask.NameToLayer("SubGraphDivider");
        }

        return triggers;
    }

    List<Node> GetViableNodes(Player player)
    {
        return GraphNodes.FindAll(x => x.CanReachPlayer);
    }

}
