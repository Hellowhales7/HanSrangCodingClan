using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPiece2 : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.data = data;
        this.position = position;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
    private void Update()
    {
        this.board.StaticSet(this);
    }
}
