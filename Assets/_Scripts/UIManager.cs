using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Button_Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
