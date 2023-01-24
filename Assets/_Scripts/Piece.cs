using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour
{
   public Board board { get; private set; }
   public TetrominoData data { get; private set; }
   public Vector3Int[] cells { get; private set; }
   public Vector3Int position { get; private set; }
   public int rotationIndex { get; private set; }

   public float stepDelay = 1f;
   public float moveDelay = 0.1f;
   public float lockDelay = 0.5f;

   private float stepTime;
   private float moveTime;
   private float lockTime;
   
   [SerializeField] private TMP_Text timeText;

   
   public void Initialize(Board board, Vector3Int position, TetrominoData data)
   {
      this.data = data;
      this.board = board;
      this.position = position;

      rotationIndex = 0;
      stepTime = Time.time + stepDelay;
      moveTime = Time.time + moveDelay;
      lockTime = 0f;

      if (cells == null) {
         cells = new Vector3Int[data.cells.Length];
      }

      for (int i = 0; i < cells.Length; i++) {
         cells[i] = (Vector3Int)data.cells[i];
      }
   }

   public void InitializePreview(Board board, Vector3Int piecePosition, TetrominoData data)
   {
      this.board = board;
      this.position = piecePosition;
      this.data = data;
      //this.cells = data.cells;
      this.gameObject.SetActive(false);
   }

   private void Update()
   {
      //this.board.Clear(this);
      this.lockTime += Time.deltaTime;
      
      if (Input.GetKeyDown(KeyCode.Q)) Rotate(-1);
      
      else if (Input.GetKeyDown(KeyCode.E)) Rotate(1);
      
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl)) HardDrop();

      if (Time.time > moveTime) HandleMoveInputs();

      if (Time.time > stepTime) Step();
      
      this.board.Set(this);
   }

   private void HandleMoveInputs()
   {
      if (Input.GetKey(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
      {
         if (Move(Vector2Int.down)) 
         {
            stepTime = Time.time + stepDelay;
         }
      }
      
      if (Input.GetKey(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow)) 
      {
         Move(Vector2Int.left);
      } 
      else if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
      {
         Move(Vector2Int.right);
      }
   }
   
   private void FixedUpdate()
   {
      DifficultyManagement();
   }

   private void DifficultyManagement()
   {
      float timer = 0.0f;
      timer += Time.time;
      int seconds = Convert.ToInt32(timer);
      timeText.text = "TIME: " + seconds;
      
      if (seconds > 9 && ((seconds % 10) == 0))
      {
         if (stepDelay < 0.1) return;
         stepDelay -= 0.002f;
      }
   }
   
   private void Step()
   {
      stepTime = Time.time + stepDelay;

      Move(Vector2Int.down);

      if (lockTime >= lockDelay) 
      {
         Lock();
      }
   }

   private void Lock()
   {
      board.Set(this);
      board.ClearLines();
      board.SpawnPiece();
   }
   
   private void HardDrop()
   {
      while (Move(Vector2Int.down))
      {
         continue;
      }
      Lock();
   }
   
   private bool Move(Vector2Int translation)
   {
      Vector3Int newPosition = position;
      newPosition.x += translation.x;
      newPosition.y += translation.y;

      bool valid = board.IsValidPosition(this, newPosition);

      if (valid)
      {
         position = newPosition;
         moveTime = Time.time + moveDelay;
         lockTime = 0f;
      }
      return valid;
   }

   private void Rotate(int direction)
   {
      int originalRotation = rotationIndex;

      rotationIndex = Wrap(rotationIndex + direction, 0, 4);
      ApplyRotationMatrix(direction);

      if (!TestWallKicks(rotationIndex, direction))
      {
         rotationIndex = originalRotation;
         ApplyRotationMatrix(-direction);
      }
   }

   private void ApplyRotationMatrix(int direction)
   {
      float[] matrix = Data.RotationMatrix;

      for (int i = 0; i < cells.Length; i++)
      {
         Vector3 cell = cells[i];

         int x, y;

         switch (data.tetromino)
         {
            case Tetromino.I:
            case Tetromino.O:
               cell.x -= 0.5f;
               cell.y -= 0.5f;
               x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
               y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
               break;

            default:
               x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
               y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
               break;
         }

         cells[i] = new Vector3Int(x, y, 0);
      }
   }
   private bool TestWallKicks(int rotationIndex, int rotationDirection)
   {
      int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

      for (int i = 0; i < data.wallKicks.GetLength(1); i++)
      {
         Vector2Int translation = data.wallKicks[wallKickIndex, i];

         if (Move(translation)) {
            return true;
         }
      }
      return false;
   }

   private int GetWallKickIndex(int rotationIndex, int rotationDirection)
   {
      int wallKickIndex = rotationIndex * 2;

      if (rotationDirection < 0) {
         wallKickIndex--;
      }

      return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
   }

   private int Wrap(int input, int min, int max)
   {
      if (input < min) {
         return max - (min - input) % (max - min);
      } else {
         return min + (input - min) % (max - min);
      }
   }
}
