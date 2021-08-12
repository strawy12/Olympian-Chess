using BackEnd;
using static BackEnd.SendQueue;
using System;
using System.Collections.Generic;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using UnityEngine.SocialPlatforms;

public class BackEndServerManager : MonoBehaviour
{
    private static BackEndServerManager inst;
    public static BackEndServerManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<BackEndServerManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<BackEndServerManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }
    public bool isLogin { get; private set; }   // 로그인 여부

    private string tempNickName;                        // 설정할 닉네임 (id와 동일)
    public string myNickName { get; private set; } = string.Empty;  // 로그인한 계정의 닉네임
    public string myIndate { get; private set; } = string.Empty;    // 로그인한 계정의 inDate

    private Action<bool, string> loginSuccessFunc = null;

    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    public string appleToken = ""; // SignInWithApple.cs에서 토큰값을 받을 문자열
    private void Awake()
    {
        // 모든 씬에서 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        /*
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
          .RequestServerAuthCode(false)
           .RequestIdToken()
          .Build();
      PlayGamesPlatform.InitializeInstance(config);
      PlayGamesPlatform.DebugLogEnabled = true;

      PlayGamesPlatform.Activate();
//#endif*/
        isLogin = false;
        try
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
                    //#if UNITY_ANDROID
                    //                    Debug.Log("GoogleHash - " + Backend.Utils.GetGoogleHash());
                    //#endif
                    // 비동기 함수 큐 초기화
                    StartSendQueue(true);
                    Debug.Log("뒤끝 초기화 성공");

                }
                else
                {
                    Debug.Log("뒤끝 초기화 실패");
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log("[예외]뒤끝 초기화 실패\n" + e.ToString());
        }
    }
    void Update()
    {
        Poll();
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        StopSendQueue();
    }

    void OnApplicationPause(bool isPause)
    {
        Debug.Log("OnApplicationPause : " + isPause);
        if (isPause == false)
        {
            ResumeSendQueue();
        }
        else
        {
            PauseSendQueue();
        }
    }

    public void BackendTokenLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.LoginWithTheBackendToken, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 로그인 성공");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("토큰 로그인 실패\n" + callback.ToString());
            func(false, string.Empty);
        });
    }
    public void CustomLogin(string id, string pw, Action<bool, string> func)
    {
        Enqueue(Backend.BMember.CustomLogin, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 로그인 성공");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("커스텀 로그인 실패\n" + callback);
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    public void CustomSignIn(string id, string pw, Action<bool, string> func)
    {
        tempNickName = id;
        Enqueue(Backend.BMember.CustomSignUp, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 회원가입 성공");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.LogError("커스텀 회원가입 실패\n" + callback.ToString());
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }
    public void UpdateNickname(string nickname, Action<bool, string> func)
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, bro =>
        {
            // 닉네임이 없으면 매치서버 접속이 안됨
            if (!bro.IsSuccess())
            {
                Debug.LogError("닉네임 생성 실패\n" + bro.ToString());
                func(false, string.Format(BackendError,
                    bro.GetStatusCode(), bro.GetErrorCode(), bro.GetMessage()));
                return;
            }
            loginSuccessFunc = func;
            OnBackendAuthorized();
        });
    }

    // 유저 정보 불러오기 사전작업
    private void OnPrevBackendAuthorized()
    {
        isLogin = true;

        OnBackendAuthorized();
    }

    // 실제 유저 정보 불러오기
    private void OnBackendAuthorized()
    {
        Enqueue(Backend.BMember.GetUserInfo, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("유저 정보 불러오기 실패\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }
            Debug.Log("유저정보\n" + callback);
            var info = callback.GetReturnValuetoJSON()["row"];
            if (info["nickname"] == null)
            {
                
                UIManager ui = FindObjectOfType<UIManager>();
                ui.ActiveNickNameObject();
                return;
            }
            myNickName = info["nickname"].ToString();
            myIndate = info["inDate"].ToString();

            if (loginSuccessFunc != null)
            {
                BackEndMatchManager.Inst.GetMatchList(loginSuccessFunc);
                loginSuccessFunc(true, string.Empty);
            }
        });
    }
    public void GuestLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("게스트 로그인 성공");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("게스트 로그인 실패\n" + callback);
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

}
