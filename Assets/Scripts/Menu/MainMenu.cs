using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to load the next scene (Play)
    private int _sceneIndex = 1;
    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Method to load the settings scene
    public void LoadSettings()
    {
        int settingsSceneIndex = SceneManager.GetActiveScene().buildIndex + 2;
        SceneManager.LoadScene(settingsSceneIndex);
    }
    public void OpenScene()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}

