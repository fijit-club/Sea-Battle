using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GreenGrids : MonoBehaviourPunCallbacks
{
    public static GreenGrids Instance;
    public int size;
    public GameObject gridPref;
    public int xOffset;
    public int yOffset;
    public List<Vector2> enemyOccupiedPositions = new List<Vector2>();
    private int num = 0;

    private void Start()
    {
        Instance = this;
        GenerateGrid();
        Gamemanager.Instance.mode = Gamemanager.Mode.Game;
        FetchOtherPlayerOccupiedPositions();
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject grid = Instantiate(gridPref, new Vector2(i + xOffset, j + yOffset), Quaternion.identity);
                grid.GetComponent<Tiles>().posX = i;
                grid.GetComponent<Tiles>().posY = j;
                grid.GetComponent<Tiles>().selectedTile = num;
                num++;
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
}

