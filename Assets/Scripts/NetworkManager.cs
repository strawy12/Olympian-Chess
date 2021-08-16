using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.IO;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private Text statusText = null;
    [SerializeField] private Text playerText = null;
    [SerializeField] private Text nicknameText = null;
    [SerializeField] private InputField roomInput, nicknameInput;
    [SerializeField] private InputField EmailInput;
    [SerializeField] private InputField PasswordInput;
    [SerializeField] private GameObject nicknamePanal = null;
    [SerializeField] private GameObject friendlyMatchPanal = null;

    private string roomname;
    private string user_ID;
    private string nickname;
    private string player = "black";


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
        DontDestroyOnLoad(gameObject);
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
        if (nicknameInput.text == "") return;

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
        PhotonNetwork.AutomaticallySyncScene = true;
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

            int i;
            i = Random.Range(0, 2); 
            Debug.Log(i);
            photonView.RPC("SetPlayer", RpcTarget.All, i);
            
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
    public void Click()
    {
        SceneManager.LoadScene("Game");
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



    public GameObject SpawnObject(GameObject gobj/*, string player*/)
    {
        //if (player != this.player) return null;
        GameObject obj = PhotonNetwork.Instantiate(gobj.name, new Vector3(0, 0, -1), Quaternion.identity);
        

        return obj;
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { GetLeaderboard(result.PlayFabId); PhotonNetwork.ConnectUsingSettings(); }, (error) => print("로그인 실패"));
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = EmailInput.text, Password = PasswordInput.text, Username = nicknameInput.text, DisplayName = nicknameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공"); SetStat(); SetData("default"); }, (error) => print("회원가입 실패"));
    }



    void SetStat()
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => print("값 저장실패"));
    }

    void GetLeaderboard(string myID)
    {
        PlayFabUserList.Clear();

        for (int i = 0; i < 10; i++)
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = i * 100,
                StatisticName = "IDInfo",
                MaxResultsCount = 100,
                ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true }
            };
            PlayFabClientAPI.GetLeaderboard(request, (result) =>
            {
                if (result.Leaderboard.Count == 0) return;
                for (int j = 0; j < result.Leaderboard.Count; j++)
                {
                    PlayFabUserList.Add(result.Leaderboard[j]);
                    if (result.Leaderboard[j].PlayFabId == myID) MyPlayFabInfo = result.Leaderboard[j];
                }
            },
            (error) => { });
        }
    }



    void SetData(string curData)
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "Data", curData } },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => { }, (error) => print("데이터 저장 실패"));
    }

    string GetData(string curID)
    {
        string path = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curID }, (result) =>
        path = curID + "\n" + result.Data["Data"].Value,
        (error) => print("데이터 불러오기 실패"));

        return path;
    }
    public void SaveDataToJson<T>(T data)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, typeof(T).ToString() + ".Json");
        File.WriteAllText(path, jsonData);
    }

    public T LoadDataFromJson<T>()
    {
        GetData();
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonData);
    }

    [PunRPC]
    private void SetPlayer(int i)
    {
        nicknameText.text = PhotonNetwork.PlayerList[0].NickName;

        if (PhotonNetwork.PlayerList[i].NickName == nickname)
        {
            player = "white";
        }
        else
        {
            player = "black";
        }
        playerText.text = player;
        //SceneManager.LoadScene("Game");
    }

    [PunRPC]
    private void Rename()
    {
        Debug.Log("응애");
        LeaveRoom();
        nicknamePanal.SetActive(true);
    }

}
    


