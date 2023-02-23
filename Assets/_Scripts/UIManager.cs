using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startUI, inGameUI;
    private AudioSource _audioSource;
    public AudioClip sfx_Button;
    public static bool isGameStart, isEasy, isMedium, isHard;
    private Board _board;
    private PreviewPiece _previewPiece;
    private Ghost _ghostBoard;
    private Piece _piece;

    private void Awake()
    {
        _board = FindObjectOfType<Board>();
        _previewPiece = FindObjectOfType<PreviewPiece>();
        _ghostBoard = FindObjectOfType<Ghost>();
        _piece = FindObjectOfType<Piece>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _board.enabled = false;
        _previewPiece.enabled = false;
        _ghostBoard.enabled = false;
        _piece.enabled = false;
        Time.timeScale = 0;
        
        isGameStart = false;
        startUI.SetActive(true);
        inGameUI.SetActive(false);
        isEasy = false;
        isMedium = false;
        isHard = false;
    }

    public void Button_Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        _audioSource.PlayOneShot(sfx_Button);
    }

    public void Button_Start()
    {
        isGameStart = true;
        startUI.SetActive(false);
        inGameUI.SetActive(true);
        _audioSource.PlayOneShot(sfx_Button);
        _board.enabled = true;
        _piece.enabled = true;
        _previewPiece.enabled = true;
        _ghostBoard.enabled = true;
        Time.timeScale = 1;
    }

    public void Button_Easy()
    {
        isEasy = true;
        isMedium = false;
        isHard = false;
    }

    public void Button_Medium()
    {
        isEasy = false;
        isMedium = true;
        isHard = false;
    }

    public void Button_Hard()
    {
        isEasy = false;
        isMedium = false;
        isHard = true;
    }
}
