using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public ZombieState path;

    SubGraph gman;

	void Update ()
    {
        path = null;
        // If the zombie can reach the player it will move towards it
        if(Node.CanReach(transform.position, Player.player.transform.position, LayerMasks.CanNodeReachPlayer))
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.player.transform.position, Time.deltaTime * 10);
            path = null;
            return;
        }
        else
        {
            gman = Graph.graph.GetSubgraph(transform.position) as SubGraph;

            PlotPath(gman);
        }

		MoveDownPath(path);
	} 

	void PlotPath(IGraph subgraph)
	{
		bool[] marked = new bool[Graph.graph.GraphNodes.Count];
		for(int i = 0; i < subgraph.GraphNodes.Count; i++)
		{
			marked[i] = false;
		}
		PriorityQueue<ZombieState> queue = new PriorityQueue<ZombieState>();
		for(int i = 0; i < subgraph.GraphNodes.Count; i++)
		{
			float distance = Vector3.Distance(transform.position, subgraph.GraphNodes[i].Component.transform.position);
			if(Node.CanReach(transform.position, subgraph.GraphNodes[i].Component.transform.position, LayerMasks.CanNodeReachPlayer) && distance != 0)
			{
				marked[subgraph.GraphNodes[i].ID] = true;
				ZombieState state = new ZombieState(subgraph.GraphNodes[i], distance);
				queue.Push(state.distance, state);
			}
		}

		while(queue.IsEmpty() == false)
		{
			path = queue.Pop();
			if(path.node.CanReachPlayer)
			{
				break;
			}
			for(int i = 0; i < path.node.adjNodes.Count; i++)
			{
				if(marked[path.node.adjNodes[i].node.ID] == false)
				{
					marked[path.node.adjNodes[i].node.ID] = true;
					ZombieState state = new ZombieState(path, path.node.adjNodes[i]);
					queue.Push(state.distance, state);
				}
			}
		}
	}

	// Pre: The zombie has found a path
	// Post: The zombie has been moved down the path chosen
	void MoveDownPath(ZombieState path) 
	{
		ZombieState n = path;
		if(path != null)
		{
			while(n.prev != null)
			{
				n = n.prev;
			}
            transform.position = Vector3.MoveTowards(transform.position, n.node.Component.transform.position, 5 * Time.deltaTime);
		}
	}

    void OnDrawGizmos()
    {
        if(path == null)
        {
			Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Player.player.transform.position);
            return;
        }
		if (Application.isPlaying == false) 
		{
			return;
		}
        ZombieState n = path;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(n.node.Component.transform.position, Player.player.transform.position);
        while(n.prev != null)
        {
            Gizmos.DrawLine(n.node.Component.transform.position, n.prev.node.Component.transform.position);
            n = n.prev;
        }
        Gizmos.DrawLine(n.node.Component.transform.position, transform.position);
    }

    void OnDrawGizmosSelected()
    {
        if(Application.isPlaying == false)
            return;
        for(int i = 0; i < gman.GraphNodes.Count; i++)
        {
            //Gizmos.DrawSphere(gman.GraphNodes[i].Component.transform.position, 2);
        }
    }

}
