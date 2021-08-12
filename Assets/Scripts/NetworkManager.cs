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

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCrerateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
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
        print("방 랜덤참가 실패");
    }
}
