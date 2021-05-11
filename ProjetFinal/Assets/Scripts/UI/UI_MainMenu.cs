using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{

    public string sceneGame;
    public string sceneOptions;



    public void LoadGame()
    {
        SceneManager.LoadScene(sceneGame);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene(sceneOptions);
    }





}
