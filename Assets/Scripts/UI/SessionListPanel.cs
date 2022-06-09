using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionListPanel : MonoBehaviour
{
    [SerializeField] private GameObject _sessionList;

    [SerializeField] private GameObject _newSessionPanel;

    [SerializeField] private GameObject _sessionSlotPrefab;

    private void Start()
    {
        _newSessionPanel.SetActive(false);

        ConnectToLobby();
    }

    private async void ConnectToLobby()
    {
        OnSessionListUpdated(new List<SessionInfo>());
        await App.Instance.EnterLobby("", OnSessionListUpdated);
    }

    private void OnSessionListUpdated(List<SessionInfo> sessions)
    {
        if (sessions != null)
        {
            foreach (var sessionInfo in sessions)
            {
                var newSessionSlot = Instantiate(_sessionSlotPrefab);
                // To fix the weird unity bug
                newSessionSlot.transform.localScale = Vector3.one;

                var gameMode = (WizardDuelGameMode)(int)sessionInfo.Properties["Game Mode"];
                int maxPlayers = sessionInfo.MaxPlayers;
                bool enableSpectators = (int)sessionInfo.Properties["Enable Spectators"] == 1;
                string hostName = (string)sessionInfo.Properties["Host Name"];
                newSessionSlot.GetComponent<SessionSlot>().Init(
                    sessionInfo.Name,
                    gameMode,
                    maxPlayers,
                    enableSpectators,
                    hostName,
                    () => { App.Instance.JoinSession(); }
                );

                newSessionSlot.transform.SetParent(_sessionList.transform);
            }
        }
        else
        {
            Debug.Log("Failed to join lobby");
        }
    }

    public void ShowNewSessionPanel()
    {
        _newSessionPanel.SetActive(true);
    }
}
