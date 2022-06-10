using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListPanel : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;

    [SerializeField] private GameObject _playerSlotPrefab;

    [SerializeField] private TMP_Text _readyButtonText;

    [SerializeField] private Button _startButton;

    private void Update()
    {
        // Clear previous players
        for (int i = 0; i < _playerList.transform.childCount; i++)
        {
            Destroy(_playerList.transform.GetChild(i).gameObject);
        }

        int playerCount = 0;
        int readyCount = 0;
        foreach (Player player in App.Instance.Players)
        {
            playerCount++;
            var newPlayerSlot = Instantiate(_playerSlotPrefab);
            newPlayerSlot.transform.localScale = Vector3.one;

            newPlayerSlot.GetComponent<PlayerSlot>().Init(player.Name.Value, "Unknown", 0, player.Ready, "Unknown");
            if (player.Ready)
                readyCount++;

            newPlayerSlot.transform.SetParent(_playerList.transform);
        }
        if (playerCount == readyCount && App.Instance.IsMaster)
            _startButton.interactable = true;
        else
            _startButton.interactable = false;
    }

    public void ToggleReady()
    {
        if (App.Instance.GetPlayer().Ready)
        {
            App.Instance.GetPlayer().Ready = false;
            _readyButtonText.text = "Ready";
        }
        else
        {
            App.Instance.GetPlayer().Ready = true;
            _readyButtonText.text = "Cancel";
        }
    }
}
