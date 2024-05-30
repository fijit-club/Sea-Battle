using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    public static ConnectServer Instance;
    [SerializeField] private GameObject playBtcs;
    public GameObject loadingTxt;
    private void Start()
    {
        Instance = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        playBtcs.SetActive(true);
        //loadingTxt.SetActive(false);
        print("ServerConnected");
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
