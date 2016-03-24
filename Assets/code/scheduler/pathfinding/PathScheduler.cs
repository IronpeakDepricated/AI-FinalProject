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

    void Start()
    {
        for(int i = 0; i < requests.Size(); i++)
        {
            ExecuteNextRequest();
        }
    }

    void Update()
    {
        if(requests.IsEmpty() == false)
        {
            ExecuteNextRequest();
        }
    }

    void ExecuteNextRequest()
    {
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

    public void AddToPathScheduler(IPathCallback callback)
    {
        requests.Push(callback);
    }
}
