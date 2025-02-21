using UnityEngine;

[CreateAssetMenu(fileName = "MapLimits", menuName = "ScriptableObjects/MapLimits")]
public class MapLimits : ScriptableObject
{
    // Limites da Pista
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    // X mínimo da Pista
    public float MinX
    {
        get { return minX; }
        set { minX = value; }
    }

    // X máximo da Pista
    public float MaxX
    {
        get { return maxX; }
        set { maxX = value; }
    }

    // Z mínimo da Pista
    public float MinZ
    {
        get { return minZ; }
        set { minZ = value; }
    }

    // Z máximo da Pista
    public float MaxZ
    {
        get { return maxZ; }
        set { maxZ = value; }
    }
}
