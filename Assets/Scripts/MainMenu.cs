using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
        {
            // Замінити "GameScene" на назву твоєї сцени з грою
            SceneManager.LoadScene("SampleScene");
            
        }
        public void QuitGame()
    {
        Debug.Log("Гра закрита!");
        Application.Quit();
    }
}
