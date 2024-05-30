using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    public enum Mode
    {
        SetUp,
        Game
    }

    public Mode mode;
    public List<Vector2> occupiedPos = new List<Vector2>();
    public List<Vector2> selectedPos = new List<Vector2>();
    public GameObject[] ships;
    public int shipIndex;
    private int totalGridsToFill=10;
    public bool allGridsFilled;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
            Destroy(this);
}

    private void Start()
    {
        mode = Mode.SetUp;
    }

    private void Update()
    {
        UpdateGridStatus();
        SetOccupiedPos(occupiedPos);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        mode = Mode.Game;
    }

    public void Rotate()
    {
        ships[shipIndex].transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 90));
    }

    private void UpdateGridStatus()
    {
        allGridsFilled = occupiedPos.Count >= totalGridsToFill;
        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties["AllGridsFilled"] = allGridsFilled;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
    }

    public void SetOccupiedPos(List<Vector2> positions)
    {
        float[] positionsArray = new float[positions.Count * 2];
        for (int i = 0; i < positions.Count; i++)
        {
            positionsArray[i * 2] = positions[i].x;
            positionsArray[i * 2 + 1] = positions[i].y;
        }

        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties.Add("EnemyOccupiedPos", positionsArray);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
    }
}
