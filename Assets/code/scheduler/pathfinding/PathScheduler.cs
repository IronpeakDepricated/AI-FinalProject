using UnityEngine;

public class PathScheduler : MonoBehaviour
{

    public static PathScheduler scheduler;

    public int millisperframe = 500;
    private Queue<IPathCallback> requests;

    void Awake()
    {
        scheduler = this;
        requests = new Queue<IPathCallback>();
    }

    void Start()
    {

    }

    void Update()
    {
        ExecuteRequests(millisperframe);
    }

    void ExecuteRequests(int millis)
    {
        long start = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        int size = requests.Size();
        for(int i = 0; i < size; i++)
        {
            IPathCallback request = requests.Pop();
            ExecuteRequest(request);
            long current = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
            if(current > (start + millis))
            {
                //Debug.Log("Requests executed: " + i);
                return;
            }
        }
    }

    void ExecuteRequest(IPathCallback request)
    {
        if(request.KeepInPathScheduler())
        {
            if(request.WantsToRecalculatePath())
            {
                request.CleanupCurrentPath();
                request.OnPathComplete(request.PlotPath());
            }
            requests.Push(request);
        }
    }

    public void AddToPathScheduler(IPathCallback callback)
    {
        ExecuteRequest(callback);
    }

    public void RemoveFromScheduler(IPathCallback callback)
    {
        int size = requests.Size();
        for(int i = 0; i < size; i++)
        {
            IPathCallback request = requests.Pop();
            if(request == callback)
            {
                return;
            }
            requests.Push(request);
        }
    }
}
