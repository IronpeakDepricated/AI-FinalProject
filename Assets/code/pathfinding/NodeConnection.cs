[System.Serializable]
public class NodeConnection
{
    public Node node;
    public float distance;

    public NodeConnection(Node node, float distance)
    {
        this.node = node;
        this.distance = distance;
    }
}
