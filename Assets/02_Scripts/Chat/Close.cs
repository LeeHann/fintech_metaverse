using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour
{
    [Header("Interact Panel")]
    public MouseLock mouseLock;
    public GameObject consultPanel;
    public GameObject videoPanel;
    public GameObject exhibitPanel;
    public GameObject buildingPanel;
    public GameObject marketPanel;

    public void OnClickClose()
    {
        if (consultPanel != null) consultPanel.SetActive(false);
        if (videoPanel != null) videoPanel.SetActive(false);
        if (exhibitPanel != null) exhibitPanel.SetActive(false);
        if (buildingPanel != null) buildingPanel.SetActive(false);
        if (marketPanel != null) marketPanel.SetActive(false);

        // mouse state
        mouseLock.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
