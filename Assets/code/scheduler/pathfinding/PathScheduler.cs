using UnityEngine;

public class PathScheduler : MonoBehaviour
{

    public static PathScheduler scheduler;

    private Queue<IPathCallback> requests;

    void Awake()
    {
        scheduler = this;
        requests = new Queue<IPathCallback>();
    }

    void Update()
    {
        if(requests.IsEmpty() == false)
        {
            Debug.Log("Request");
            IPathCallback request = requests.Pop();
            request.CleanupCurrentPath();
            if(request.KeepInPathScheduler())
            {
                if(request.WantsToRecalculatePath())
                {
                    request.OnPathComplete(request.PlotPath());
                }
                requests.Push(request);
            }
        }
    }

    public void AddToPathScheduler(IPathCallback callback)
    {
        requests.Push(callback);
    }
}
