using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PreviewPiece : MonoBehaviour
{
    public TetrominoData[] tetrominoes;
    public Board mainBoard;
    public Piece activePiece;
    
    [SerializeField] 
    private Vector3Int position = new Vector3Int(12, -2, 0);

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Copy();
        InitializePreview(mainBoard, position, mainBoard.currentPiece.data);
        //Set(this.activePiece);
        //Clear();
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++) 
        {
            cells[i] = activePiece.cells[i];
        }
        activePiece.Initialize(mainBoard, position, mainBoard.currentPiece.data);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    
    private void InitializePreview(Board board, Vector3Int position, TetrominoData data)
    {
        if (cells == null) 
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            tilemap.SetTile(tilePosition, null);
        }
    }
}
