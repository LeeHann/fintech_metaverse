using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class ConsultItemGenerator : MonoBehaviour   // 버튼 생성기
{
	[Header("Admin")]
	[SerializeField] private GameObject Btns;    // 운영자일 때 버튼을 생성할 수 있게 하는 패널 
	// [SerializeField] private GameObject Consult_ScrollView; // 편집 시 활성화

	[Header("Instantiate")]
	[SerializeField] private Transform consultContent;
	[SerializeField] private GameObject generateBtnPrefab;    // 버튼 생성 프리팹

	[Header("Set FAQ Value")]
	[SerializeField] private InputField inputFieldContent;
	[SerializeField] private InputField inputFieldUrl;
	private string btnContent;
	private string btnUrl;

	[HideInInspector] public bool deleteMode;
	public List<ConsultBtn> delList = new List<ConsultBtn>();

	private void Start()
	{
		// 운영자(admin)라면 Btns 활성화
		Backend.BMember.GetUserInfo((callback) =>
		{
			string game_id = callback.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
			if (game_id == "46fa2cf0-aced-11ec-b9a3-15234b658fd0")
			{
				Btns.SetActive(true);
			}
		});

		// 서버에서 FAQ 내용 읽어오기
		Backend.GameData.Get("FAQ_catalog", new Where(), (callback) =>
		{
			// 읽은 내용 개수(count) 만큼 (버튼을 생성하고 스크롤 뷰 자식으로 넣어놓기)=>BtnGenerate()
			JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
			// Debug.Log(jsonData.Count);
			for (int i = jsonData.Count - 1; i >= 0; i--)
			{
				// Debug.Log(jsonData[i]["content"][0].ToString());
				btnContent = jsonData[i]["content"][0].ToString();
				// Debug.Log(jsonData[i]["url"][0].ToString());
				btnUrl = jsonData[i]["url"][0].ToString();
				BtnGenerate();
			}
		});
	}

	public void BtnGenerate()
	{   // 1. FAQ 정보를 새로 생성(DB에도 넣어야함) : UrlServerInsert -> BtnGenerate
		// 2. 기존 정보를 읽어와 버튼을 생성(DB 넣을 필요 없음) : BtnGenerate
		string content = (btnContent == string.Empty) ? inputFieldContent.text : btnContent;
		string url = (btnUrl == string.Empty) ? inputFieldUrl.text : btnUrl;

		// ConsultBtn 오브젝트 생성 및 consultContent의 자식으로 넣기
		var item = Instantiate(generateBtnPrefab, consultContent).GetComponent<ConsultBtn>();
		// btnContent는 ConsultBtn의 Content로 Text.text 에 넣기
		item.content.text = content;
		// btnUrl은 ConsultBtn의 url에 넣기
		item.url = url;
		item.CG = this;

		// 서버에서 읽은 inDate값 ConsultBtn에 할당하기 - 삭제할 때를 위해
		Where where = new Where();
		where.Equal("content", content);
		Backend.GameData.Get("FAQ_catalog", where, (callback) =>
		{
			item.inDate = callback.GetInDate();
			// Debug.Log("inDate 삽입 완료 : " + item.inDate);
		});

		// 참조값 초기화
		btnContent = string.Empty;
		btnUrl = string.Empty;
		inputFieldContent.text = string.Empty;
		inputFieldUrl.text = string.Empty;
	}

	public void UrlServerInsert()
	{// 에디터 혹은 빌드에서 FAQ 버튼을 생성해서 서버에 정보를 넣으려 한다.
	 // UrlServerInsert -> BtnGenerate
		string content = (btnContent == string.Empty) ? inputFieldContent.text : btnContent;
		string url = (btnUrl == string.Empty) ? inputFieldUrl.text : btnUrl;

		if (content == string.Empty || url == string.Empty)
			return;

		// 서버에 현재 btnContent, btnUrl을 디비에 넣기
		Param consult = new Param();
		consult.Add("content", content);
		consult.Add("url", url);
		Backend.GameData.Insert("FAQ_catalog", consult);
		BtnGenerate();
	}

#region Delete
    public void OnClickClose(Image delBtn)
    {
        deleteMode = false;
        foreach (var item in delList)
        {
            item.img.color = new Color(1, 0.8f, 0);
        }
        delList.RemoveAll(delList.Contains);
        delBtn.color = new Color(1, 0.8f, 0);
    }

	public void OnClickDeleteMode(Image btn)
	{
		deleteMode = !deleteMode;
		if (deleteMode == false)
		{
			UrlServerDelete();
            btn.color = new Color(1, 0.8f, 0);
		}
		else    // 삭제 모드
		{
			btn.color = new Color(1, 0.3596304f, 0f);
		}
	}

	public void UrlServerDelete()
	{
		for (int i = delList.Count - 1; i >= 0; i--)
		{
			var item = delList[i];
			// item의 inDate를 이용해 디비에서 정보 삭제
			Backend.GameData.DeleteV2("FAQ_catalog", item.inDate, Backend.UserInDate, (callback) =>
			{
				if (callback.IsSuccess())
				{
					// 오브젝트 파괴
					delList.Remove(item);
					Destroy(item.gameObject);
				}
				else
				{
					deleteMode = true;
                }
			});
		}
	}
#endregion
    public void OnClickCloseView(Button FAQGen_CloseBtn)
    {
        FAQGen_CloseBtn.onClick.Invoke();
        Backend.BMember.GetUserInfo((callback) =>
		{
			string game_id = callback.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
			if (game_id != "46fa2cf0-aced-11ec-b9a3-15234b658fd0")
			{
				Btns.SetActive(false);
			}
		});
    }
}