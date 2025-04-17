using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int Index { get; private set; }
    public int tileSize = 1;
    private MapManager.MapType currentType;    

    public void SetIndex(Vector2Int index)
    {
        Index = index;
        transform.position = new Vector3(Index.x * tileSize, 0, Index.y * tileSize);
        UpdateTexture();
    }
    
    public void SetMapType(MapManager.MapType mapType)
    {
        currentType = mapType;
        UpdateTexture();
    }
    public void UpdateTexture()
    {
        //Material mat = 
    }
}
