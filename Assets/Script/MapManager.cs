using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum MapType { fire, ice, electricity }
    public MapType currentMap = MapType.fire;

    public TileManager tileManager;

    private void Start()
    {
        //currentMap = 
        
    }
}
            
            