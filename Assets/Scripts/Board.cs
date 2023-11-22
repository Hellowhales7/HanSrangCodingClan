using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TileBase tileBase;
    public TileBase StaticTile;
    public Piece activePiece { get; private set; }
    public StaticPiece stPiece { get; private set; }
    public StaticPiece1 stPiece1 { get; private set; }
    public StaticPiece2 stPiece2 { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector3Int[] ExpectSpawnPos;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    private int BlockIndex = 0;
    private int BlockMaxIndex = 7;
    private List<int> TetrominoShuffer = new List<int>();
    private List<TetrominoData> tetrominoDatas = new List<TetrominoData>();

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);

            return new RectInt(position, this.boardSize);
        }
    }

    private static void ShuffleList<T>(IList<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        stPiece = GetComponent<StaticPiece>();
        stPiece1 = GetComponent<StaticPiece1>();
        stPiece2 = GetComponent<StaticPiece2>();
        
        for (var i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }

        for (var i = 0; i < BlockMaxIndex; i++)
        {
            TetrominoShuffer.Add(i);
        }

        ShuffleList(TetrominoShuffer);

        RandomMap();
    }

    private void RandomMap()
    {
        var startPos = new Vector3Int(-5, -10, 0);
        var list = new List<int>();

        for (var i = 0; i < 10; i++)
        {
            list.Add(i);
        }

        for (var i = 0; i < 5; i++)
        {
            ShuffleList(list);

            for (var j = 0; j < 6; j++)
            {
                var index = new Vector3Int(list[j], i, 0);

                tilemap.SetTile(startPos + index, StaticTile);
            }
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayBgm(true);
        SpawnPiece();
        Timer.TimerON();
    }

    private void Update()
    {
        if (!Timer.TimeOver())
        {
            return;
        }
        
        StageMgr.StageClear();
        tilemap.ClearAllTiles();
    }

    public void SpawnPiece()
    {
        while (tetrominoDatas.Count < 4)
        {
            // int random = Random.Range(0, tetrominoes.Length);

            if (BlockIndex >= BlockMaxIndex)
            {
                ShuffleList(TetrominoShuffer);
                BlockIndex = 0;
            }

            var data = tetrominoes[TetrominoShuffer[BlockIndex]];

            BlockIndex++;
            tetrominoDatas.Add(data);

            for (var i = 0; i < 3; i++)
            {
                HardClear(ExpectSpawnPos[i]);
            }
        }

        // int random = Random.Range(0, tetrominoes.Length);
        // TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, tetrominoDatas[0]);

        stPiece.Initialize(this, ExpectSpawnPos[0], tetrominoDatas[1]);
        stPiece1.Initialize(this, ExpectSpawnPos[1], tetrominoDatas[2]);
        stPiece2.Initialize(this, ExpectSpawnPos[2], tetrominoDatas[3]);
        tetrominoDatas.RemoveAt(0);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        tilemap.ClearAllTiles();
        LogicValue.ScoreReset();
        SceneManager.LoadScene("GameOver");
    }

    public void Set(Piece piece)
    {
        foreach (var t in piece.cells)
        {
            var tilePosition = t + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void StaticSet(StaticPiece piece)
    {
        foreach (var t in piece.cells)
        {
            var tilePosition = t + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void StaticSet(StaticPiece1 piece)
    {
        foreach (var t in piece.cells)
        {
            var tilePosition = t + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void StaticSet(StaticPiece2 piece)
    {
        foreach (var t in piece.cells)
        {
            Vector3Int tilePosition = t + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        foreach (var t in piece.cells)
        {
            var tilePosition = t + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    private void HardClear(Vector3Int position)
    {
        var tilePosition = position + new Vector3Int(-1, 2, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(-1, 1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(-1, 0, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(-1, -1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(0, 2, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(0, 1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(0, 0, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(0, -1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(1, 2, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(1, 1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(1, 0, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(1, -1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(2, 2, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(2, 1, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(2, 0, 0);
        tilemap.SetTile(tilePosition, tileBase);
        
        tilePosition = position + new Vector3Int(2, -1, 0);
        tilemap.SetTile(tilePosition, tileBase);
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        var bounds = Bounds;

        foreach (var t in piece.cells)
        {
            var tilePosition = t + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ClearLines()
    {
        var bounds = Bounds;
        var row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                LogicValue.Score += LogicValue.GetScore;
                Timer.TimeComsume(LogicValue.TimeConsume);
                Debug.Log(LogicValue.BlockSpeed);

                // Debug.Log(StageMgr.StageArr[0].SpeedUp);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        var bounds = Bounds;

        for (var col = bounds.xMin; col < bounds.xMax; col++)
        {
            var position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void LineClear(int row)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.BreakBlock);

        var bounds = Bounds;

        for (var col = bounds.xMin; col < bounds.xMax; col++)
        {
            var position = new Vector3Int(col, row, 0);

            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (var col = bounds.xMin; col < bounds.xMax; col++)
            {
                var position = new Vector3Int(col, row + 1, 0);
                var above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }

        var json = JsonUtility.ToJson(new LetterData());

        WebSocketManager.Instance.SendGameLogic(json);
        // Debug.Log(json);
    }
}