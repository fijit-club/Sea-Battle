using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUi : MonoBehaviour
{
    public static GameUi Instance;
    public GameObject gameWonTxt;

    private void Start()
    {
        Instance = this;
        gameWonTxt = GameObject.Find("GameWon");
    }
    public void GameWon()
    {
        if (Gamemanager.Instance.mode ==Gamemanager. Mode.Game && Gamemanager.Instance.selectedPos.Count == Gamemanager.Instance.occupiedPos.Count)
        {
            gameWonTxt.GetComponent<TextMeshProUGUI>().enabled = true;
            Time.timeScale = 0;
        }
    }
}
