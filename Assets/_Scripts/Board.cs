using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEditor;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winScreenUI, failScreenUI;   
    
    public TetrominoData[] tetrominoes;
    public Tilemap tilemap;
    public Piece currentPiece;
    public TetrominoData nextTetrominoData;
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public Vector2Int boardSize = new Vector2Int(10, 20);
    
    public int score = 0;

    private AudioSource _audioSource;
    public AudioClip sfx_LineClear;
    public AudioClip sfx_BlockPlaced;
    public AudioClip sfx_Win;
    public AudioClip sfx_Fail;

    public RectInt Bounds 
    { 
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        currentPiece = GetComponentInChildren<Piece>();
        _audioSource = GetComponent<AudioSource>();
        
        for (int i = 0; i < tetrominoes.Length; i++) 
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
       _audioSource.Play();
        score = 0;
        TetrominoData initialTetrominoData = tetrominoes[Random.Range(0, tetrominoes.Length)];
        SpawnPiece(initialTetrominoData);
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public bool IsWin()
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position)) 
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnPiece(TetrominoData tetrominoData)
    {
        if (UIManager.isGameStart = false) return;
        nextTetrominoData = tetrominoes[Random.Range(0, tetrominoes.Length)];
        currentPiece.Initialize(this, spawnPosition, tetrominoData);

        if (IsValidPosition(currentPiece, spawnPosition))
        {
            Set(currentPiece);
        } 
        else 
        {
            GameOver();
        }
    }
    
    public void GameOver()
    {
        tilemap.ClearAllTiles();
        failScreenUI.SetActive(true);
        Time.timeScale = 0f;
        _audioSource.Stop();
        _audioSource.PlayOneShot(sfx_Fail);
    }
    
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }
        return true;
    }
    
    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row)) 
            {
                LineClear(row);
            } 
            else 
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position)) 
            {
                return false;
            }
        }
        return true;
    }

    public void LineClear(int row)
    {
        score += 100;
        _audioSource.PlayOneShot(sfx_LineClear);
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }
            row++;
        }
        
        if (IsWin())
        {
            PauseGame.isGamePaused = true;
            winScreenUI.SetActive(true);
            _audioSource.Stop();
            _audioSource.PlayOneShot(sfx_Win);
        }
    }

    public void BlockPlacedSFX()
    {
        _audioSource.PlayOneShot(sfx_BlockPlaced);
    }
}
