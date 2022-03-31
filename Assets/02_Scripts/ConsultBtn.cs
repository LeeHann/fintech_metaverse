using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsultBtn : MonoBehaviour
{
    public Image img;
    public Text content;
    public string url;
    [HideInInspector] public ConsultItemGenerator CG;
    public string inDate;
    bool isSelected = false;

    public void OnClickHyperLink()
    {
        if (CG.deleteMode == true)
        {
            isSelected = !isSelected;
            if (isSelected == true)
            {
                // 삭제 모드, 선택되었을 때 색 변경
                CG.delList.Add(this);
                img.color = new Color(1, 0.3596304f, 0f);
            }
            else
            {
                // 선택 해제 시 색 되돌리기
                CG.delList.Remove(this);
                img.color = new Color(1, 0.8f, 0);
            }
        }
        else
        {
            Application.OpenURL(url);
        }
    }
}
