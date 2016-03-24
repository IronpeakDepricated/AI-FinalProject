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
        ExecuteRequests(requests.Size());
    }

    void Update()
    {
        if(requests.IsEmpty() == false)
        {
            ExecuteRequests(10);
        }
    }

    void ExecuteRequests(int count)
    {
        int size = requests.Size();
        for(int i = 0; i < size && 0 < count; i++)
        {
            IPathCallback request = requests.Pop();
            if(request.KeepInPathScheduler())
            {
                if(request.WantsToRecalculatePath())
                {
                    request.CleanupCurrentPath();
                    request.OnPathComplete(request.PlotPath());
                    count--;
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
