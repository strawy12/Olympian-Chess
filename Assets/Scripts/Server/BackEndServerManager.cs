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
    public bool isLogin { get; private set; }   // �α��� ����

    private string tempNickName;                        // ������ �г��� (id�� ����)
    public string myNickName { get; private set; } = string.Empty;  // �α����� ������ �г���
    public string myIndate { get; private set; } = string.Empty;    // �α����� ������ inDate

    private Action<bool, string> loginSuccessFunc = null;

    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    public string appleToken = ""; // SignInWithApple.cs���� ��ū���� ���� ���ڿ�
    private void Awake()
    {
        // ��� ������ ����
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
                    // �񵿱� �Լ� ť �ʱ�ȭ
                    StartSendQueue(true);
                    Debug.Log("�ڳ� �ʱ�ȭ ����");

                }
                else
                {
                    Debug.Log("�ڳ� �ʱ�ȭ ����");
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log("[����]�ڳ� �ʱ�ȭ ����\n" + e.ToString());
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
                Debug.Log("��ū �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("��ū �α��� ����\n" + callback.ToString());
            func(false, string.Empty);
        });
    }
    public void CustomLogin(string id, string pw, Action<bool, string> func)
    {
        Enqueue(Backend.BMember.CustomLogin, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("Ŀ���� �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("Ŀ���� �α��� ����\n" + callback);
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
                Debug.Log("Ŀ���� ȸ������ ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.LogError("Ŀ���� ȸ������ ����\n" + callback.ToString());
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }
    public void UpdateNickname(string nickname, Action<bool, string> func)
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, bro =>
        {
            // �г����� ������ ��ġ���� ������ �ȵ�
            if (!bro.IsSuccess())
            {
                Debug.LogError("�г��� ���� ����\n" + bro.ToString());
                func(false, string.Format(BackendError,
                    bro.GetStatusCode(), bro.GetErrorCode(), bro.GetMessage()));
                return;
            }
            loginSuccessFunc = func;
            OnBackendAuthorized();
        });
    }

    // ���� ���� �ҷ����� �����۾�
    private void OnPrevBackendAuthorized()
    {
        isLogin = true;

        OnBackendAuthorized();
    }

    // ���� ���� ���� �ҷ�����
    private void OnBackendAuthorized()
    {
        Enqueue(Backend.BMember.GetUserInfo, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("���� ���� �ҷ����� ����\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }
            Debug.Log("��������\n" + callback);
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
                Debug.Log("�Խ�Ʈ �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("�Խ�Ʈ �α��� ����\n" + callback);
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

}
