using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private Text statusText = null;
    [SerializeField] private Text playerText = null;
    [SerializeField] private InputField roomInput, nicknameInput;
    [SerializeField] private GameObject nicknamePanal = null;
    [SerializeField] private GameObject friendlyMatchPanal = null;

    private string roomname;
    private string user_ID;
    private string nickname;
    private string player = "white";


    private static NetworkManager inst;
    public static NetworkManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<NetworkManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject("NetworkManager").AddComponent<NetworkManager>();

                    inst = newObj;
                }
            }
            return inst;
        }
    }

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (statusText != null)
        {
            statusText.text = PhotonNetwork.NetworkClientState.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
            {
                if (PhotonNetwork.PlayerListOthers[i].NickName == nickname)
                {
                    nicknamePanal.SetActive(true);
                }
            }

        }
    }

    public void Connect()
    {
        if (nickname == null)
        {
            nicknamePanal.SetActive(true);
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ActiveFriendlyMatchPanal()
    {
        friendlyMatchPanal.SetActive(true);
    }

    public void SetNickname()
    {
        if (nicknameInput.text == null) return;

        nickname = nicknameInput.text;
        PhotonNetwork.LocalPlayer.NickName = nickname;
        nicknamePanal.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
    }

    public void DisConnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
    }

    public void CreateRoom()
    {
        roomname = nickname + "'s room.";
        Debug.Log(roomname);
        PhotonNetwork.CreateRoom(roomname, new RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCrerateRoom()
    {
        roomname = roomInput.text;
        PhotonNetwork.JoinOrCreateRoom(roomname, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRandomRoom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            CreateRoom();
        }

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        print("방 만들기 완료");
    }

    public override void OnJoinedRoom()
    {
        print("방 참가 완료");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (newPlayer.NickName == nickname)
            {
                photonView.RPC("Rename", RpcTarget.Others);
                return;
            }
            int i = Random.Range(0, 2);
            photonView.RPC("SettingPlayer", RpcTarget.All, i);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 만들기 실패");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 참가 실패");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
    public string GetPlayer()
    {
        return player;
    }

    public bool CheckSameNickname()
    {
        Debug.Log(PhotonNetwork.PlayerListOthers.Length);
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (PhotonNetwork.PlayerListOthers[i].NickName == nickname)
            {

                return true;
            }
        }
        return false;
    }

    public void StartEndTurn()
    {
        photonView.RPC("EndTurn", RpcTarget.All);
    }

    public GameObject SpawnChessPiece(GameObject chessPiece/*, string player*/)
    {
        //if (player != this.player) return null;
        GameObject obj = PhotonNetwork.Instantiate(chessPiece.name, new Vector3(0, 0, -1), Quaternion.identity);
        obj.name = chessPiece.name;

        return obj;
    }

    [PunRPC]
    

    private void SettingPlayer(int i)
    {
        if (PhotonNetwork.PlayerList[i].NickName == nickname)
        {
            player = "white";
        }
        else if (PhotonNetwork.PlayerList[i].NickName != nickname)
        {
            player = "black";
        }
        playerText.text = player;
        SceneManager.LoadScene("Game");
    }
    [PunRPC]
    private void Rename()
    {
        Debug.Log("응애");
        LeaveRoom();
        nicknamePanal.SetActive(true);
    }

    [PunRPC]
    private void EndTurn()
    {
        TurnManager.Instance.EndTurn();
    }
}
    


