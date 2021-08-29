using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text statusText;
    [SerializeField] private InputField roomInput;
    [SerializeField] private GameObject friendlyMatchPanal = null;
    [SerializeField] private GameObject lodingDisplay = null;

    private string roomname;
    private string user_ID;
    private string player = "qwer";
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
        NetworkManager[] nms = FindObjectsOfType<NetworkManager>();

        if (nms.Length != 1)
        {
            for(int i = 0; i < nms.Length; i++)
            {
                if(nms[i] == this)
                {
                    Destroy(nms[i].gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);

        Debug.Log("wdaad");

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        lodingDisplay.SetActive(true);
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
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        lodingDisplay.SetActive(false);
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
        PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCrerateRoom()
    {
        roomname = roomInput.text;
        if (roomname == "") return;
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

    public override void OnLeftRoom()
    {
        
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Rename();
        print("방 참가 완료");
    }

    private void Rename()
    {
        if (PhotonNetwork.PlayerListOthers.Length != 0)
        {
            while (PhotonNetwork.PlayerListOthers[0].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                PhotonNetwork.LocalPlayer.NickName = "player" + nicknameCnt.ToString();
                nicknameCnt++;
            }
        }

        else
        {
            PhotonNetwork.LocalPlayer.NickName = "player" + nicknameCnt.ToString();
            nicknameCnt++;
        }

        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LoadLevel("Lobby");
        LeaveRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            int i;

            i = Random.Range(0, 2);
            SetPlayer(i);
            StartCoroutine(Startgame());
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
        GameObject obj = PhotonNetwork.Instantiate(gobj.name, new Vector3(0, 0, -1), Quaternion.identity);
        //GameObject obj = Instantiate(gobj, new Vector3(0, 0, -1), Quaternion.identity);

        return obj;
    }

    private void SetPlayer(int i)
    {
        Debug.Log("실러");

        if (i == 0)
        {
            player = "white";
            photonView.RPC("SetPlayer", RpcTarget.OthersBuffered, "black");
        }
        else if (i == 1)
        {
            player = "black";
            photonView.RPC("SetPlayer", RpcTarget.OthersBuffered, "white");
        }
    }

    [PunRPC]
    private void SetPlayer(string player)
    {
        Debug.Log("실러");
        this.player = player;
    }
    private IEnumerator Startgame()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LoadLevel("Game");
    }
}