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

    Backend.Match.OnMatchMakingResponse += (args) => // serverAddress와 serverPort 확인하는 곳

    string serverAddress = args.RoomInfo.m_inGameServerEndPoint.m_address;
    ushort serverPort = args.RoomInfo.m_inGameServerEndPoint.m_port;


void JoinInGameServer() // 임의의 함수
{
    bool isReconnect = true;
    ErrorInfo errorInfo = null;

    if (Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out errorInfo) == false)
    {
        // 에러 확인
        return;
    }
}
}


