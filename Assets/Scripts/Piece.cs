using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    private float _stepTime;
    private float _lockTime;
    private float _blockMoveTime;
    private const float BlockMoveDelay = 0.25f;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.data = data;
        this.position = position;
        rotationIndex = 0;
        _stepTime = Time.time + LogicValue.BlockSpeed;
        _lockTime = 0f;
        _blockMoveTime = 0f;

        cells ??= new Vector3Int[data.cells.Length];

        for (var i = 0; i < data.cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        board.Clear(this);
        _lockTime += Time.deltaTime;
        _blockMoveTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.RCR);
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.CR);
            Rotate(1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (_blockMoveTime > BlockMoveDelay)
            {
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Left);
                Move(Vector2Int.left);
                _blockMoveTime = 0f;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_blockMoveTime > BlockMoveDelay)
            {
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Right);
                Move(Vector2Int.right);
                _blockMoveTime = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HardDrop();
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.HardDrop);
        }

        if (Time.time >= this._stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this._stepTime = Time.time + LogicValue.BlockSpeed;
        Move(Vector2Int.down);

        if (this._lockTime >= LogicValue.BlockLockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
            continue;
        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this._lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        var originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (TestWallKicks(rotationIndex, direction))
        {
            return;
        }

        rotationIndex = originalRotation;
        ApplyRotationMatrix(-direction);
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (var i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                        (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                        (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                         (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                         (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        var wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (var i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            var translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        var wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private static int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }

        return min + (input - min) % (max - min);
    }
}