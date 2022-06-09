using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;

    [SerializeField] private TMP_Text _class;

    [SerializeField] private TMP_Text _ping;

    [SerializeField] private TMP_Text _ready;

    [SerializeField] private TMP_Text _team;

    public void Init(string playerName, string magicClass, int ping, bool ready, string team)
    {
        _playerName.text = playerName;
        _class.text = magicClass;
        _ping.text = ping.ToString();
        _ready.text = ready ? "Yes" : "No";
        _team.text = team;
    }
}
