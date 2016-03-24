using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour, IPathCallback
{

    public Path Path;

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
            if(Path != null && Path.Nodes.Count != 0)
            {
                Path.Nodes[Path.Nodes.Count - 1].Select(this, 10);
            }
            CleanupCurrentPath();
            return;
        }

        MoveDownPath(Path);
    }

    int pathindex = 0;
    void MoveDownPath(Path path)
    {
        if(path != null && pathindex < path.Nodes.Count)
        {
            if(MoveTowards(path.Nodes[pathindex].Component.transform.position, 5))
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
        for(int i = pathindex; i < Path.Nodes.Count; i++)
        {
            Gizmos.DrawLine(position, Path.Nodes[i].Component.transform.position);
            position = Path.Nodes[i].Component.transform.position;
        }
        Gizmos.DrawLine(position, Player.player.transform.position);
    }

    public Path PlotPath()
    {
        if(Node.CanReach(transform.position, Player.player.transform.position, LayerMasks.CanNodeReachPlayer))
        {
            return new Path(new List<Node>(), Vector3.Distance(transform.position, Player.player.transform.position));
        }
        
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
                queue.Push(state.f, state);
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
                if(marked[goal.node.adjNodes[i].node.ID] == false && goal.node.subIndex == goal.node.adjNodes[i].node.subIndex)
                {
                    ZombieState state = new ZombieState(goal, goal.node.adjNodes[i]);
                    marked[goal.node.adjNodes[i].node.ID] = true;
                    queue.Push(state.f, state);
                }
            }
        }

        if(goal == null)
        {
            return new Path(new List<Node>(), 0);
        }

        return goal.ToPath();
    }

    public void OnPathComplete(Path path)
    {
        Path = path;
        pathindex = 0;
        if(path.Nodes.Count != 0)
        {
            Path.Nodes[Path.Nodes.Count - 1].Select(this, 1000);
        }
    }

    public void CleanupCurrentPath()
    {
        if(Path != null && Path.Nodes.Count != 0)
        {
            Path.Nodes[Path.Nodes.Count - 1].Deselect(this);
        }
        pathindex = 0;
        Path = null;
    }

    public bool KeepInPathScheduler()
    {
        return true;
    }

    public bool WantsToRecalculatePath()
    {
        if(Path == null || Path.Nodes.Count == 0)
            return true;
        return Path.Nodes[Path.Nodes.Count - 1].CanReachPlayer == false;
    }
}
