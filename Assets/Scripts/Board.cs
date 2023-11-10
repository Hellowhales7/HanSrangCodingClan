
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting.FullSerializer.Internal.Converters;

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
    private List<T> ShuffleList<T>(List<T> list)
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

        return list;
    }
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        stPiece = GetComponent<StaticPiece>();
        stPiece1 = GetComponent<StaticPiece1>();
        stPiece2 = GetComponent<StaticPiece2>();
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
        for(int i=0;i< BlockMaxIndex;i++)
        {
            TetrominoShuffer.Add(i);
        }
        ShuffleList(TetrominoShuffer);

        RandomMap();
    }

    private void RandomMap()
    {
        Vector3Int startPos = new Vector3Int(-5, -10, 0);
        List<int> list = new List<int>();
        for(int i=0;i<10;i++)
        {
            list.Add(i);
        }
        for(int i=0;i<5;i++)
        {
            ShuffleList(list);
            for(int j= 0;j<6;j++)
            {
                Vector3Int index = new Vector3Int(list[j], i, 0);
                this.tilemap.SetTile(startPos + index, StaticTile);
            }
        }
    }

    private void Start()
    {
        Timer.TimerON();
        SpawnPiece();
        AudioManager.instance.PlayBgm(true);
    }
    private void Update()
    {
        if (Timer.TimeOver())
        {
            StageMgr.StageClear();
            this.tilemap.ClearAllTiles();
        }
    }
    public void SpawnPiece()
    {
        for(; tetrominoDatas.Count < 4;) {
            //int random = Random.Range(0, tetrominoes.Length);
            if(BlockIndex>=BlockMaxIndex)
            {
                ShuffleList(TetrominoShuffer);
                BlockIndex = 0;
            }
            TetrominoData data = tetrominoes[TetrominoShuffer[BlockIndex]];
            BlockIndex++;
            tetrominoDatas.Add(data);
            for(int i = 0; i < 3; i++)
                HardClear(ExpectSpawnPos[i]);
        }
        //int random = Random.Range(0, tetrominoes.Length);
        //TetrominoData data = tetrominoes[random];
        this.activePiece.Initialize(this, this.spawnPosition, tetrominoDatas[0]);

        stPiece.Initialize(this, this.ExpectSpawnPos[0], tetrominoDatas[1]);
        stPiece1.Initialize(this, this.ExpectSpawnPos[1], tetrominoDatas[2]);
        stPiece2.Initialize(this, this.ExpectSpawnPos[2], tetrominoDatas[3]);
        tetrominoDatas.RemoveAt(0);


        if (IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
        LogicValue.ScoreReset();
        SceneManager.LoadScene("GameOver");
    }
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void StaticSet(StaticPiece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void StaticSet(StaticPiece1 piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void StaticSet(StaticPiece2 piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    public void HardClear(Vector3Int Position)
    {
        Vector3Int tilePosition = Position + new Vector3Int(-1, 2, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(-1, 1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(-1, 0, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(-1, -1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(0, 2, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(0, 1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(0, 0, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(0, -1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(1, 2, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(1, 1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(1, 0, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(1, -1, 0);
        this.tilemap.SetTile(tilePosition, tileBase); 
        tilePosition = Position + new Vector3Int(2, 2, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(2, 1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(2, 0, 0);
        this.tilemap.SetTile(tilePosition, tileBase);
        tilePosition = Position + new Vector3Int(2, -1, 0);
        this.tilemap.SetTile(tilePosition, tileBase);

    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

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

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

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
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }
    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BreakBlock);
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
        LetterData SendData = new LetterData();
        string json = JsonUtility.ToJson(SendData);
        Debug.Log(json);

    }
}
