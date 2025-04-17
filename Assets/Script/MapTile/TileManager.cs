using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("플레이어 트랜스폼")]
    public Transform playerTr;
    [Header("타일 프리팹")]
    public GameObject tilePrefab;
    [Header("타일 사이즈")]
    public int tileSize = 1;
    [Header("바닥 사이즈 7이 제일 적정"), Tooltip("홀수로 설정하기 (캐릭터를 중앙에 두기 위해)"), Range(3, 11)]
    public int gridSize = 3;//홀수로 사용할것

    private List<Tile> tiles = new List<Tile>();
    private Vector2Int playertile;
    private MapManager.MapType currentMap;


    public void SetMapType(MapManager.MapType mapType)
    {
        currentMap = mapType;
        foreach (var tile in tiles)
        {
            tile.SetMapType(currentMap);
        }
    }

    private void Start()
    {

        int half = gridSize / 2;
        for (int x = -half; x <= half; x++)
        {
            for (int y = -half; y <= half; y++)
            {
                Vector2Int index = new Vector2Int(x, y);
                Vector3 pos = new Vector3(x * tileSize, 0, y * tileSize);
                GameObject maketile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                Tile tile = maketile.GetComponent<Tile>();
                tile.SetIndex(index);
                tile.SetMapType(currentMap);
                tiles.Add(tile);
            }
            playertile = GetPlayerTileIndex();
        }
    }
    private void Update()
    {
        Vector2Int playerTileIndex = GetPlayerTileIndex();
        if (playerTileIndex != playertile)
        {
            RelocateTiles(playerTileIndex);
            playertile = playerTileIndex;
        }
    }
    private Vector2Int GetPlayerTileIndex()
    {
        return new Vector2Int(Mathf.FloorToInt(playerTr.position.x / tileSize),
            Mathf.FloorToInt(playerTr.position.z / tileSize));
    }

    void RelocateTiles(Vector2Int playerTileIndex)
    {
        int half = gridSize / 2;
        foreach (var tile in tiles)
        {
            Vector2Int tileIndex = tile.Index;
            Vector2Int dif = tileIndex - playerTileIndex;

            // x축
            if (dif.x > half)
            {
                tileIndex.x -= gridSize;
                tile.SetIndex(tileIndex);
            }
            else if (dif.x < -half)
            {
                tileIndex.x += gridSize;
                tile.SetIndex(tileIndex);
            }
            // y(z)축
            if (dif.y > half)
            {
                tileIndex.y -= gridSize;
                tile.SetIndex(tileIndex);
            }
            else if (dif.y < -half)
            {
                tileIndex.y += gridSize;
                tile.SetIndex(tileIndex);
            }
            tile.SetMapType(currentMap);
            tile.UpdateTexture();
        }
    }

    //void RelocateTiles(Vector2 playerTileIndex)
    //{
    //    foreach (var tile in tiles)
    //    {
    //        Vector2 tileIndex = tile.Index;
    //        Vector2 diff = tileIndex - playerTileIndex;
    //        bool moved = false;

    //        // x축
    //        if (diff.x > 1)
    //        {
    //            tileIndex.x -= 3;
    //            moved = true;
    //        }
    //        else if (diff.x < -1)
    //        {
    //            tileIndex.x += 3;
    //            moved = true;
    //        }
    //        // z축
    //        if (diff.y > 1)
    //        {
    //            tileIndex.y -= 3;
    //            moved = true;
    //        }
    //        else if (diff.y < -1)
    //        {
    //            tileIndex.y += 3;
    //            moved = true;
    //        }

    //        if (moved)
    //        {
    //            tile.SetIndex(tileIndex);
    //            tile.UpdateTexture();
    //        }
    //    }
    //}

}