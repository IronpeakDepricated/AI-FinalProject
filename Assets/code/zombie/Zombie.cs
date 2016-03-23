using UnityEngine;
using System.Collections.Generic;
using System;

public class Zombie : MonoBehaviour, IPathCallback
{

    public List<Node> Path;

    void Start()
    {
        PathScheduler.scheduler.AddToPathScheduler(this);
    }

    void Update()
    {
        // If the zombie can reach the player it will move towards it
        if(Node.CanReach(transform.position, Player.player.transform.position, LayerMasks.CanNodeReachPlayer))
        {
            MoveTowards(Player.player.transform.position, 10);
            return;
        }

        MoveDownPath(Path);
    }

    int pathindex = 0;
    void MoveDownPath(List<Node> path)
    {
        if(path != null && pathindex < path.Count)
        {
            if(MoveTowards(path[pathindex].Component.transform.position, 5))
            {
                pathindex++;
            }
        }
    }

    bool MoveTowards(Vector3 destination, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        return transform.position == destination;
    }


    void OnDrawGizmos()
    {
        if(Path == null)
        {
            return;
        }
        if(Application.isPlaying == false)
        {
            return;
        }
        Gizmos.color = Color.black;
        Vector3 position = transform.position;
        for(int i = 0; i < Path.Count; i++)
        {
            Gizmos.DrawLine(position, Path[i].Component.transform.position);
            position = Path[i].Component.transform.position;
        }
        Gizmos.DrawLine(position, Player.player.transform.position);
    }

    public List<Node> PlotPath()
    {
        IGraph subgraph = Graph.graph.GetSubgraph(transform.position);

        if(subgraph == null)
        {
            Debug.Log("Couldn't find subgraph");
            return null;
        }

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

        ZombieState goal = null;
        while(queue.IsEmpty() == false)
        {
            goal = queue.Pop();
            if(goal.node.CanReachPlayer)
            {
                break;
            }
            for(int i = 0; i < goal.node.adjNodes.Count; i++)
            {
                if(marked[goal.node.adjNodes[i].node.ID] == false)
                {
                    marked[goal.node.adjNodes[i].node.ID] = true;
                    ZombieState state = new ZombieState(goal, goal.node.adjNodes[i]);
                    queue.Push(state.distance, state);
                }
            }
        }

        List<Node> path = goal.ToPath();
        for(int i = 0; i < path.Count; i++)
        {
            path[i].Selected++;
        }
        return path;

    }

    public void OnPathComplete(List<Node> path)
    {
        pathindex = 0;
        Path = path;
    }

    public void CleanupCurrentPath()
    {
        for(int i = 0; i < Path.Count; i++)
        {
            Path[i].Selected--;
        }
    }

    public bool KeepInPathScheduler()
    {
        return true;
    }

    public bool WantsToRecalculatePath()
    {
        return true;
    }
}
