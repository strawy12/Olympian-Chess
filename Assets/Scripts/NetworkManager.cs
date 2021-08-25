using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public Text statusText = null;
    public InputField roomInput, nicknameInput;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
    }

    public void DisConnect()
    {
            
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비접속완료");
    }

}
