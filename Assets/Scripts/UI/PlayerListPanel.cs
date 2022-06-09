using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListPanel : MonoBehaviour
{
    [SerializeField] private GameObject _playerList;

    [SerializeField] private GameObject _playerSlotPrefab;

    private void Update()
    {
        // Clear previous players
        for (int i = 0; i < _playerList.transform.childCount; i++)
        {
            Destroy(_playerList.transform.GetChild(i).gameObject);
        }

        foreach (Player player in App.Instance.Players)
        {
            var newPlayerSlot = Instantiate(_playerSlotPrefab);
            newPlayerSlot.transform.localScale = Vector3.one;

            newPlayerSlot.GetComponent<PlayerSlot>().Init(player.Name.Value, "Unknown", 0, player.Ready, "Unknown");

            newPlayerSlot.transform.SetParent(_playerList.transform);
        }
    }
}
