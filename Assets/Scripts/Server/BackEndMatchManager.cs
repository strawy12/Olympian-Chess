using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Battlehub.Dispatcher;
using System.Linq;


public partial class BackEndMatchManager : MonoBehaviour
{

    private bool isHost = false;    
    
    string serverAddress;
    ushort serverPort;

    Backend.Match.OnMatchMakingResponse += (args) => // serverAddress�� serverPort Ȯ���ϴ� ��

    string serverAddress = args.RoomInfo.m_inGameServerEndPoint.m_address;
    ushort serverPort = args.RoomInfo.m_inGameServerEndPoint.m_port;


void JoinInGameServer() // ������ �Լ�
{
    bool isReconnect = true;
    ErrorInfo errorInfo = null;

    if (Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out errorInfo) == false)
    {
        // ���� Ȯ��
        return;
    }
}
}


