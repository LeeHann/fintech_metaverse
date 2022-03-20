using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class Login : MonoBehaviour
{
	void Start()
	{
		Screen.SetResolution(1920, 1080, true);

		if (!Backend.IsInitialized)
		{
			Backend.Initialize(true);
		}

		InitializeCallback();
	}

	private void InitializeCallback()
	{
		if (Backend.IsInitialized)
		{
			Debug.Log(Backend.Utils.GetServerTime());
			Debug.Log("구글 해시 : " + Backend.Utils.GetGoogleHash());
		}
	}

	public InputField idInputField;
	public InputField pwInputField;
	public InputField nicknameInputField;
	public GameObject nicknamePanel;
	public GameObject logPanel;
	public Text logText;

    public void CustomLogin()
    {
        string id = idInputField.text; // 원하는 아이디
		string pw = pwInputField.text; // 원하는 비밀번호

        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pw))
        {
            BackendReturnObject loginResult = Backend.BMember.CustomLogin(id, pw);

            // 로그인 성공시 메인으로 이동
            if (loginResult.IsSuccess())
            {
                if (DoesNicknameExist())
                {
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    nicknamePanel.SetActive(true);
                }
            }
            else
            {
                Error(loginResult.GetMessage());
            }
        }
        else
        {
            Error("ID 혹은 PW를 입력해주세요.");
        }
    }

    private bool DoesNicknameExist()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        if (bro.IsSuccess())
        {
            JsonData nicknameJson = bro.GetReturnValuetoJSON()["row"]["nickname"];

            // 닉네임 여부를 확인
            return nicknameJson != null;
        }

        return false;
    }

	public void CustomSignUp()
	{   // 회원가입 버튼을 눌렀을 때
		string id = idInputField.text; // 원하는 아이디
		string pw = pwInputField.text; // 원하는 비밀번호

		if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pw))
		{
			var bro = Backend.BMember.CustomSignUp(id, pw);

			if (bro.IsSuccess())
			{
				// 닉네임 패널 띄우기
				nicknamePanel.SetActive(true);
			}
			else
			{
				Error(bro.GetErrorCode());
			}
		}
	}

	public void SetNickname()
	{   // NicknamePanel의 GO 버튼
		string nick = nicknameInputField.text;
		BackendReturnObject nicknameCreated = Backend.BMember.CreateNickname(nick);
		if (nicknameCreated.IsSuccess())
		{
			SceneManager.LoadScene("Main");
		}
		else
		{
            Error(nicknameCreated.GetErrorCode());
		}
	}

	void Error(string errorCode)
	{
		switch (errorCode)
		{
			case "DuplicatedParameterException":
				logText.text = "중복된 사용자 아이디입니다.";
				break;
			case "BadUnauthorizedException":
				logText.text = "잘못된 사용자 아이디 혹은 비밀번호입니다.";
				break;
			default:
				logText.text = errorCode;
				break;
		}
        logPanel.SetActive(true);
	}
}
