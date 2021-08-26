using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text statusText;
    [SerializeField] private InputField roomInput;
    [SerializeField] private GameObject friendlyMatchPanal = null;

    private string roomname;
    private string user_ID;
    private string player = "white";
    private bool isLoaded;
    private string SAVE_PATH;
    private int nicknameCnt = 0;

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

        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public string SaveDataToJson<T>(T data, bool isSave)
    {
        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        string jsonData = JsonUtility.ToJson(data, true);
        if (isSave)
        {
            string path = Path.Combine(SAVE_PATH, typeof(T).ToString() + ".json");
            File.WriteAllText(path, jsonData);
        }

        return jsonData;
    }

    public T LoadDataFromJson<T>(string jsonData = null)
    {
        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        if (jsonData == null)
        {
            string path = Path.Combine(SAVE_PATH, typeof(T).ToString() + ".json");
            if (File.Exists(path))
                jsonData = File.ReadAllText(path);

        }
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void ActiveFriendlyMatchPanal()
    {
        friendlyMatchPanal.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        TurnManager.Instance.StartGame();
        GameManager.Inst.SetCamera();

    }

    public void DisConnect()
    {

        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
    }
    public override void OnJoinedLobby()
    {

    }

    public void CreateRoom()
    {
        roomname = PhotonNetwork.LocalPlayer.NickName + "'s room.";
        PhotonNetwork.CreateRoom(roomname, new RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCrerateRoom()
    {
        Rename();
        roomname = roomInput.text;
        if (roomname == "") return;
        PhotonNetwork.JoinOrCreateRoom(roomname, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRandomRoom()
    {
        Rename();
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
        PhotonNetwork.AutomaticallySyncScene = true;
        print("방 참가 완료");
    }

    [PunRPC]
    private void Rename()
    {
        PhotonNetwork.LocalPlayer.NickName = "player" + nicknameCnt.ToString();
        nicknameCnt++;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (newPlayer.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                Rename();
            }

            int i;
            i = Random.Range(0, 2);
            photonView.RPC("SetPlayer", RpcTarget.AllBuffered, i);

            PhotonNetwork.LoadLevel("Game");

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


    public GameObject SpawnObject(GameObject gobj/*, string player*/)
    {
        //if (player != this.player) return null;
        //GameObject obj = PhotonNetwork.Instantiate(gobj.name, new Vector3(0, 0, -1), Quaternion.identity);
        GameObject obj = Instantiate(gobj, new Vector3(0, 0, -1), Quaternion.identity);


        return obj;
    }




    [PunRPC]
    private void SetPlayer(int i)
    {
        if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
        {
            player = "white";
        }
        else
        {
            player = "black";
        }
    }



}



