using UnityEngine;
using UnityEngine.Tilemaps;

public class PreviewPiece : MonoBehaviour
{
    //public TetrominoData[] tetrominoes;
    public Board mainBoard;
    public Piece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    
    [SerializeField] private Vector3Int position = new Vector3Int(14, -4, 0);

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++) 
        {
            cells[i] = (Vector3Int) mainBoard.nextTetrominoData.cells[i];
        }
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, mainBoard.nextTetrominoData.tile);
        }
    }
}
