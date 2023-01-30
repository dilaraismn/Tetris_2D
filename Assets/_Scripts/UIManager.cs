using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    private AudioSource _audioSource;
    public AudioClip sfx_Button;
    public static bool isGameStart;

    private void Start()
    {
        isGameStart = false;
        startUI.SetActive(true);
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.PlayOneShot(sfx_Button);
    }
}
