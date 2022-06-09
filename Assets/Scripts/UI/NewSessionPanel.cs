using UnityEngine;

public class NewSessionPanel : MonoBehaviour
{
    public void ShowSessionListPanel()
    {
        gameObject.SetActive(false);
    }

    public void CreateGame()
    {
        App.Instance.CreateSession();
    }
}
