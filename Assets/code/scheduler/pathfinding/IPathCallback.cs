using UnityEngine;
using System.Collections.Generic;

public interface IPathCallback
{
    List<Node> PlotPath();
    void OnPathComplete(List<Node> path);

    void CleanupCurrentPath();
    bool KeepInPathScheduler();
    bool WantsToRecalculatePath();
}
