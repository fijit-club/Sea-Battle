using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    [SerializeField] private GameObject startGameBtn;
    [SerializeField] private GameObject waitingToStartTxt;
    [SerializeField] private GameObject otherPlayerNotReadyTxt;
    public GameObject startPannel;
    public GameObject setUptPannel;
    private string avatarURL;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = Bridge.GetInstance().thisPlayerInfo.data.multiplayer.username;
        avatarURL = Bridge.GetInstance().thisPlayerInfo.data.multiplayer.avatar;
    }

    public void TryCreateRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = Bridge.GetInstance().thisPlayerInfo.data.multiplayer.username;

        // Set the avatar URL as a custom property
        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties.Add("AvatarURL", avatarURL);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

        CreateRoom();
    }

    private void CreateRoom()
    {
        PhotonNetwork.JoinRoom(Bridge.GetInstance().thisPlayerInfo.data.multiplayer.chatLobbyId);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.EmptyRoomTtl = 0;
        PhotonNetwork.CreateRoom(Bridge.GetInstance().thisPlayerInfo.data.multiplayer.chatLobbyId, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.JoinRoom(Bridge.GetInstance().thisPlayerInfo.data.multiplayer.chatLobbyId);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.SetActive(true);
            waitingToStartTxt.SetActive(false);
        }
        else
        {
            startGameBtn.SetActive(false);
            waitingToStartTxt.SetActive(true);
        }
        setUptPannel.SetActive(true);
        startPannel.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.SetActive(true);
            waitingToStartTxt.SetActive(false);
        }
        else
        {
            startGameBtn.SetActive(false);
            waitingToStartTxt.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && AreAllPlayersReady())
        {
            Debug.Log("Master client is starting the game.");
            PhotonNetwork.LoadLevel("Game");
        }
        else
        {
            Debug.LogWarning("Only the master client can start the game.");
        }
    }

    private IEnumerator OtherPlayerNotReady()
    {
        otherPlayerNotReadyTxt.SetActive(true);
        yield return new WaitForSeconds(2f);
        otherPlayerNotReadyTxt.SetActive(false);
    }

    private bool AreAllPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("AllGridsFilled") || !(bool)player.CustomProperties["AllGridsFilled"])
            {
                StartCoroutine(OtherPlayerNotReady());
                return false;
            }
        }
        return true;
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        setUptPannel.SetActive(false);
        startPannel.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.SetActive(true);
            waitingToStartTxt.SetActive(false);
        }
        else
        {
            startGameBtn.SetActive(false);
            waitingToStartTxt.SetActive(true);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public IEnumerator DownloadImage(string MediaUrl, Image profilePic)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite profileSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
            profilePic.sprite = profileSprite;
        }
    }
}
