using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _sessionName;

    [SerializeField] private TMP_Text _gameMode;

    [SerializeField] private TMP_Text _maxPlayers;

    [SerializeField] private TMP_Text _enableSpectators;

    [SerializeField] private TMP_Text _hostName;

    [SerializeField] private Button _joinButton;

    public void Init(string sessionName, WizardDuelGameMode gameMode, int maxPlayers, bool enableSpectators, string hostName, Action jointCallback)
    {
        _sessionName.text = sessionName;
        _gameMode.text = gameMode.ToString();
        _maxPlayers.text = maxPlayers.ToString();
        _enableSpectators.text = enableSpectators ? "Yes" : "No";
        _hostName.text = hostName;
        _joinButton.onClick.AddListener(() => { jointCallback(); });
    }
}
