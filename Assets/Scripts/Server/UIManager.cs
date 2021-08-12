using UnityEngine;
using Battlehub.Dispatcher;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject mainTitle, touchStart, loginObject, customLoginObject, 
        signUpObject, errorObject, nicknameObject, backGroundObject;

    [SerializeField] private FadeOnOff fadeObject;
    private InputField[] loginField, signUpField;
    private InputField nicknameField;
    private Text errorText;
    

    private const byte ID_INDEX = 0;
    private const byte PW_INDEX = 1;
    private const string VERSION_STR = "Ver {0}";

    void Start()
    {
        mainTitle.SetActive(true);
        touchStart.SetActive(true);
        backGroundObject.SetActive(true);
        loginObject.SetActive(false);
        customLoginObject.SetActive(false);
        signUpObject.SetActive(false);
        errorObject.SetActive(false);
        nicknameObject.SetActive(false);

        loginField = customLoginObject.GetComponentsInChildren<InputField>();
        signUpField = signUpObject.GetComponentsInChildren<InputField>();
        nicknameField = nicknameObject.GetComponentInChildren<InputField>();
        errorText = errorObject.GetComponentInChildren<Text>();

        //loadingObject.SetActive(false);

        //mainTitle.GetComponentInChildren<Text>().text = string.Format(VERSION_STR, Application.version);

        //var google = loginObject.transform.GetChild(0).gameObject;
        //var apple = loginObject.transform.GetChild(1).gameObject;
#if UNITY_ANDROID
        //google.SetActive(true);
       // apple.SetActive(false);
#elif UNITY_IOS
        google.SetActive(false);
        apple.SetActive(true);
#endif
    }

    public void TouchStart()
    {
        //loadingObject.SetActive(true);
        BackEndServerManager.Inst.BackendTokenLogin((bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (result)
                {
                    ChangeLobbyScene();
                    return;
                }
                //loadingObject.SetActive(false);
                if (!error.Equals(string.Empty))
                {
                    errorText.text = "유저 정보 불러오기 실패\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                backGroundObject.SetActive(false);
                touchStart.SetActive(false);
                mainTitle.SetActive(false);
                loginObject.SetActive(true);
            });
        });

    }

    public void Login()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string id = loginField[ID_INDEX].text;
        string pw = loginField[PW_INDEX].text;

        if (id.Equals(string.Empty) || pw.Equals(string.Empty))
        {
            errorText.text = "ID 혹은 PW 를 먼저 입력해주세요.";
            errorObject.SetActive(true);
            return;
        }

        //loadingObject.SetActive(true);
        BackEndServerManager.Inst.CustomLogin(id, pw, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    //loadingObject.SetActive(false);
                    errorText.text = "로그인 에러\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void SignUp()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string id = signUpField[ID_INDEX].text;
        string pw = signUpField[PW_INDEX].text;

        if (id.Equals(string.Empty) || pw.Equals(string.Empty))
        {
            errorText.text = "ID 혹은 PW 를 먼저 입력해주세요.";
            errorObject.SetActive(true);
            return;
        }

        //loadingObject.SetActive(true);
        BackEndServerManager.Inst.CustomSignIn(id, pw, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    //loadingObject.SetActive(false);
                    errorText.text = "회원가입 에러\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void ActiveNickNameObject()
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            mainTitle.SetActive(false);
            touchStart.SetActive(false);
            loginObject.SetActive(false);
            customLoginObject.SetActive(false);
            signUpObject.SetActive(false);
            errorObject.SetActive(false);
            //loadingObject.SetActive(false);
            nicknameObject.SetActive(true);
        });
    }

    public void UpdateNickName()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string nickname = nicknameField.text;
        if (nickname.Equals(string.Empty))
        {
            errorText.text = "닉네임을 먼저 입력해주세요";
            errorObject.SetActive(true);
            return;
        }
        //loadingObject.SetActive(true);
        BackEndServerManager.Inst.UpdateNickname(nickname, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    //loadingObject.SetActive(false);
                    errorText.text = "닉네임 생성 오류\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void GuestLogin()
    {
        if (errorObject.activeSelf)
        {
            return;
        }

        //loadingObject.SetActive(true);
        BackEndServerManager.Inst.GuestLogin((bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    //loadingObject.SetActive(false);
                    errorText.text = "로그인 에러\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    void ChangeLobbyScene()
    {
        if (fadeObject != null)
        {
            GameManager.Inst.ChangeState(GameManager.GameState.MatchLobby, (bool isDone) =>
            {
                //Dispatcher.Current.BeginInvoke(() => loadingObject.transform.Rotate(0, 0, -10));
                if (isDone)
                {
                    fadeObject.gameObject.SetActive(true);
                    fadeObject.StartFadeOnOff();
                }
            });
        }
        else
        {
            GameManager.Inst.ChangeState(GameManager.GameState.MatchLobby);
        }
    }
}
