using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Tiles : MonoBehaviour
{
    public int posX, posY, selectedTile;

    private void OnMouseDown()
    {
        if (Gamemanager.Instance.mode == Gamemanager.Mode.Game)
        {
            bool isOccupied = false;

            foreach (Vector2 i in GameGridGenerator.Instance.enemyOccupiedPositions)
            {
                if (new Vector2(posX, posY) == i)
                {
                    isOccupied = true;
                    break;
                }
            }

            if (isOccupied)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                Gamemanager.Instance.selectedPos.Add(new Vector2(posX, posY));
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }

            // Send custom event to other player
            object[] content = new object[] { selectedTile, isOccupied };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, SendOptions.SendReliable);

            GameUi.Instance.GameWon();
        }
    }
}

