using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameGridGenerator : MonoBehaviour, IOnEventCallback
{
    public static GameGridGenerator Instance;
    public int size;
    public GameObject gridPrefBlue,gridPrefGreen;
    public int xOffset;
    public int yOffset;
    public List<GameObject> blueGrids = new List<GameObject>();
    public List<Vector2> enemyOccupiedPositions = new List<Vector2>();
    private int num = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateBlueGrids();
        GenerateGreenGrids();
        Gamemanager.Instance.mode = Gamemanager.Mode.Game;
        PhotonNetwork.AddCallbackTarget(this);
        FetchOtherPlayerOccupiedPositions();
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    private void GenerateGreenGrids()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject grid = Instantiate(gridPrefGreen, new Vector2(i + xOffset, j + yOffset), Quaternion.identity);
                grid.GetComponent<Tiles>().posX = i;
                grid.GetComponent<Tiles>().posY = j;
                grid.GetComponent<Tiles>().selectedTile = num;
                num++;
            }

        }
    }

    private void GenerateBlueGrids()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject grid = Instantiate(gridPrefBlue, new Vector2(i, j), Quaternion.identity);
                grid.GetComponent<Tiles>().posX = i;
                grid.GetComponent<Tiles>().posY = j;
                blueGrids.Add(grid);
            }
        }
    }

    private void FetchOtherPlayerOccupiedPositions()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                if (player.CustomProperties.TryGetValue("EnemyOccupiedPos", out object positions))
                {
                    float[] positionsArray = (float[])positions;
                    for (int i = 0; i < positionsArray.Length; i += 2)
                    {
                        Vector2 pos = new Vector2(positionsArray[i], positionsArray[i + 1]);
                        enemyOccupiedPositions.Add(pos);
                    }
                }
            }
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            object[] data = (object[])photonEvent.CustomData;
            int tileIndex = (int)data[0];
            bool isOccupied = (bool)data[1];

            if (tileIndex >= 0 && tileIndex < blueGrids.Count)
            {
                if (isOccupied)
                {
                    blueGrids[tileIndex].transform.GetChild(0).gameObject.SetActive(true);
                    blueGrids[tileIndex].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    blueGrids[tileIndex].transform.GetChild(0).gameObject.SetActive(false);
                    blueGrids[tileIndex].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
}

