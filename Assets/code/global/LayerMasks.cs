using UnityEngine;

public static class LayerMasks
{
    public static readonly LayerMask ZombieReachNode;
    public static readonly LayerMask CanNodeReachPlayer;
    public static readonly LayerMask FindAdjNodes;

    static LayerMasks()
    {
        ZombieReachNode = 1;
        CanNodeReachPlayer = 1;
        FindAdjNodes = 1 + (1 << 31);
    }
}
