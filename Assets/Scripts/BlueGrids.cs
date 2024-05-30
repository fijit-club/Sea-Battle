using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class BlueGrids : MonoBehaviour, IOnEventCallback
{
    public int size;
    public GameObject gridPref;
    public int xOffset;
    public int yOffset;
    public List<GameObject> grids = new List<GameObject>();

    private void Start()
    {
        GenerateGrid();
        Gamemanager.Instance.mode = Gamemanager.Mode.Game;
        PhotonNetwork.AddCallbackTarget(this);
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
                grids.Add(grid);
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

            if (tileIndex >= 0 && tileIndex < grids.Count)
            {
                if (isOccupied)
                {
                    grids[tileIndex].transform.GetChild(0).gameObject.SetActive(true);
                    grids[tileIndex].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    grids[tileIndex].transform.GetChild(0).gameObject.SetActive(false);
                    grids[tileIndex].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
}

