using UnityEngine;
using System.Collections.Generic;

public interface IPathCallback
{
    Path PlotPath();
    void OnPathComplete(Path path);

    void CleanupCurrentPath();
    bool KeepInPathScheduler();
    bool WantsToRecalculatePath();
}
