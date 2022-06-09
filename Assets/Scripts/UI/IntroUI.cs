using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    public void LoadStagingScene()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
