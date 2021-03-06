using UnityEngine;
using BackEnd;

public class SdkInit : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            Debug.Log("초기화 성공!");
            CustomSignIn();
        }
        else
        {
            // 초기화 실패 시 로직
            Debug.LogError("초기화 실패!");
        }
    }

    private void Update() 
    {
        Backend.AsyncPoll();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //초기화 성공 이후 버튼 등을 통해 함수 실행
    public void CustomSignUp()
    {
        string id = "user1"; // 원하는 아이디
        string password = "1234"; // 원하는 비밀번호

        var bro = Backend.BMember.CustomSignUp(id, password);
        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 성공!");
        }
        else
        {
            Debug.LogError("회원가입 실패!");
            Debug.LogError(bro); // 뒤끝의 리턴케이스를 로그로 보여줍니다.
        }
    }

    public void CustomSignIn()
    {
        string id = "admin";
        string password = "admin";
        // string id = "hann";
        // string password = "0000";
        
        var bro = Backend.BMember.CustomLogin(id, password);
        if (bro.IsSuccess())
        {
            Debug.Log("login");
        }
        else
        {
            Debug.LogError("login failed");
        }
    }
}