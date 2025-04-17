using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterial : MonoBehaviour
{
    public static TileMaterial Instance { get; private set; }
    public Material fireMat, iceMat, electricityMat;

    private void Awake()
    {
        Instance = this;
    }
    public Material GetMaterial(MapManager.MapType mapType, Vector2 index)
    {
        switch (mapType)
        {
            case MapManager.MapType.fire: return fireMat;
            case MapManager.MapType.ice: return iceMat;
            case MapManager.MapType.electricity: return electricityMat;
        }
        return fireMat;
    }
}
