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

    public void SetDistanceToPlayer()
    {
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            GraphNodes[i].DistanceToPlayer = Vector3.Distance(GraphNodes[i].Component.transform.position, Player.player.transform.position);
        }
    }

    public void GenerateSubGraphs(Player player)
    {
        CreateTriggers(player);
        SubGraphs = new List<SubGraph>();
        bool[] marked = new bool[GraphNodes.Count];
        int graphidx = 0;
        for (int i = 0; i < GraphNodes.Count; i++) {
            if (!marked[i])
            {
                List<Node> sub = new List<Node>();
                sub.Add(GraphNodes[i]);
                GraphNodes[i].subIndex = graphidx;
                marked[i] = true;
                //generate whole subgraph for given node i
                for (int j = 0; j < sub.Count; j++) {
                    for (int k = 0; k < sub[j].adjNodes.Count; k++) {
                        if (InSameSubGraph(sub[j].Component.transform.position, sub[j].adjNodes[k].node.Component.transform.position)) {
                            Node a = sub[j];
                            Node b = sub[j].adjNodes[k].node;
                            if (!marked[b.ID] && !(a.CanReachPlayer && b.CanReachPlayer))
                            {
                                sub.Add(b);
                                marked[b.ID] = true;
                                b.subIndex = graphidx;
                            }
                        }
                    }
                }
                SubGraphs.Add(new SubGraph(sub));
                graphidx++;
            }
        }
    }
    public bool InSameSubGraph(Vector3 origin, Vector3 destination)
    {
        Vector3 direction = destination - origin;
        if(Physics.Raycast(origin + direction.normalized*0.1f, direction, Vector3.Distance(origin, destination) - 0.2f, LayerMasks.FindAdjNodes))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public IGraph GetSubgraph(Vector3 position)
    {
        IGraph subgraph = this;
        float minDistance = float.MaxValue;
        for(int i = 0; i < GraphNodes.Count; i++)
        {
            if(Node.CanReach(position, GraphNodes[i].Component.transform.position, LayerMasks.ZombieReachNode) && !Physics.Linecast(position, GraphNodes[i].Component.transform.position, LayerMasks.FindAdjNodes))
            {
                float distance = Vector3.Distance(position, GraphNodes[i].Component.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    subgraph = SubGraphs[GraphNodes[i].subIndex];
                }
            }
        }
        return subgraph;
    }

    void CreateTriggers(Player player)
    {
        for(int i = 0; i < triggers.Length; i++)
        {
            GameObject.Destroy(triggers[i]);
        }
        List<Node> viablenodes = GetViableNodes(player);
        triggers  = new GameObject[viablenodes.Count];

        for(int i = 0; i < viablenodes.Count; i++)
        {
            triggers[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 direction = viablenodes[i].Component.transform.position - player.transform.position;
            RaycastHit hit;
            Physics.SphereCast(player.transform.position,0.49f, direction, out hit, float.MaxValue, LayerMasks.SubGraphGeneration);
            float dist = Vector3.Distance(hit.point, player.transform.position);
            triggers[i].transform.position = player.transform.position + (direction.normalized*dist) / 2;
            triggers[i].transform.LookAt(player.transform.position);
            triggers[i].transform.localScale = new Vector3(0.0001f, 1, hit.distance);
            triggers[i].GetComponent<MeshRenderer>().material.color = Color.cyan;
            triggers[i].layer = LayerMask.NameToLayer("SubGraphDivider");
        }
    }

    List<Node> GetViableNodes(Player player)
    {
        List<Node> visible = GraphNodes.FindAll(x => x.CanReachPlayer);
        List<Node> adj = new List<Node>();
        bool[] marked = new bool[GraphNodes.Count];
        // find all non player reachable nodes that are neigbours of reachable nodes
        foreach (Node n in visible) {
            foreach (NodeConnection nc in n.adjNodes) {
                if (!nc.node.CanReachPlayer && !marked[nc.node.ID]) {
                    adj.Add(nc.node);
                    marked[nc.node.ID] = true;
                }
            }
        }
        List<Node> viable = new List<Node>();
        // for each node find the node with a shortest distance to player from this node
        // if 2 player visible nodes are not connected but both reachable from this they are both selected
        foreach (Node n in adj) {
            Node champion = null;
            float dist = float.MaxValue;
            List<Node> potential = new List<Node>();
            List<float> potentialdist = new List<float>();
            //find champion (best node) and possible nodes
            foreach (NodeConnection nc in n.adjNodes) {
                if (nc.node.CanReachPlayer) {
                    if (champion == null)
                    {
                        champion = nc.node;
                        dist = nc.distance + (Vector3.Distance(nc.node.Component.transform.position, player.transform.position));
                    }
                    else if (dist > nc.distance + (Vector3.Distance(nc.node.Component.transform.position, player.transform.position)))
                    {
                        potential.Add(champion);
                        potentialdist.Add(dist);
                        champion = nc.node;
                        dist = nc.distance + (Vector3.Distance(nc.node.Component.transform.position, player.transform.position));
                    }
                    else
                    {
                        potential.Add(nc.node);
                        potentialdist.Add(nc.distance + (Vector3.Distance(nc.node.Component.transform.position, player.transform.position)));
                    }
                }
            }

            //if champion has been seen dont re add him
            if (!marked[champion.ID])
            {
                marked[champion.ID] = true;
                viable.Add(champion);
            }

            // check if any of the potential nodes is not connected to the champion
            // if more than one is found pick the best of those
            if (potential.Count != 0)
            {
                dist = float.MaxValue;
                int best = -1;
                for (int i = 0; i < potential.Count; i++) {
                    if (!champion.IsInAdjList(potential[i]))
                    {
                        if (dist > potentialdist[i])
                        {
                            best = i;
                            dist = potentialdist[i];
                        }
                    }
                }
                if (best != -1)
                {
                    if (!marked[potential[best].ID])
                    {
                        marked[potential[best].ID] = true;
                        viable.Add(potential[best]);
                    }
                }
            }
        }
        return viable;
    }
}
