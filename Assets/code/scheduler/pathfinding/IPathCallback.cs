using UnityEngine;
using System.Collections.Generic;

public interface IPathCallback
{
    List<Vector3> PlotPath();
    void OnPathComplete(List<Vector3> path);

    void CleanupCurrentPath();
    bool KeepInPathScheduler();
    bool WantsToRecalculatePath();
}
