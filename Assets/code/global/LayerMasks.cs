using UnityEngine;

public static class LayerMasks
{
    public static readonly LayerMask ZombieReachNode;
    public static readonly LayerMask CanNodeReachPlayer;
    public static readonly LayerMask FindAdjNodes;
    public static readonly LayerMask SubGraphGeneration;

    static LayerMasks()
    {
        ZombieReachNode = 1;
        CanNodeReachPlayer = 1;
        FindAdjNodes = 1 + (1 << 31);
        SubGraphGeneration = 1;
    }
}
