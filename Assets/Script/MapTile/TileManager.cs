using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("�÷��̾� Ʈ������")]
    public Transform playerTr;
    [Header("Ÿ�� ������")]
    public GameObject tilePrefab;
    [Header("Ÿ�� ������")]
    public int tileSize = 1;
    [Header("�ٴ� ������ 7�� ���� ����"), Tooltip("Ȧ���� �����ϱ� (ĳ���͸� �߾ӿ� �α� ����)"), Range(3, 11)]
    public int gridSize = 3;//Ȧ���� ����Ұ�

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

            // x��
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
            // y(z)��
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

    //        // x��
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
    //        // z��
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