using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public ZombieState path;

	void Update ()
    {
        path = null;
		// If the zombie can reach the player it will move towards it
        if(Node.CanReach(transform.position, Player.player.transform.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.player.transform.position, Time.deltaTime * 10);
            path = null;
            return;
        }
        else
        {
			PlotPath ();
        }

		MoveDownPath(path);
	}

	void PlotPath()
	{
		bool[] marked = new bool[Graph.graph.GraphNodes.Count];
		for(int i = 0; i < Graph.graph.GraphNodes.Count; i++)
		{
			marked[i] = false;
		}
		PriorityQueue<ZombieState> queue = new PriorityQueue<ZombieState>();
		for(int i = 0; i < Graph.graph.GraphNodes.Count; i++)
		{
			float distance = Vector3.Distance(transform.position, Graph.graph.GraphNodes[i].Component.transform.position);
			if(Node.CanReach(transform.position, Graph.graph.GraphNodes[i].Component.transform.position) && distance != 0)
			{
				marked[Graph.graph.GraphNodes[i].ID] = true;
				ZombieState state = new ZombieState(Graph.graph.GraphNodes[i], distance);
				queue.Push(state.distance, state);
			}
		}

		int expansions = 0;
		while(queue.IsEmpty() == false)
		{
			expansions++;
			ZombieState next = queue.Pop();
			if(next.node.CanReachPlayer)
			{
				path = next;
				break;
			}
			for(int i = 0; i < next.node.adjNodes.Count; i++)
			{
				if(marked[next.node.adjNodes[i].node.ID] == false)
				{
					marked[next.node.adjNodes[i].node.ID] = true;
					ZombieState state = new ZombieState(next, next.node.adjNodes[i]);
					queue.Push(state.distance, state);
				}
			}
		}
		Debug.Log(expansions);
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
			if (n.node == null) 
			{
				Debug.Log ("n is null");
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

}
