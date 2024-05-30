using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance;
    public int size;
    public GameObject gridPref;
    public int xOffset;
    public int yOffset;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                GameObject grid = Instantiate(gridPref, new Vector2(i+xOffset, j+yOffset), Quaternion.identity);
                grid.GetComponent<Tiles>().posX = i;
                grid.GetComponent<Tiles>().posY = j;
            }
        }
    }
}
