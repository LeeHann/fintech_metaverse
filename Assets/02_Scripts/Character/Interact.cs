using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interact Panel")]
    public MouseLock mouseLock;
    public GameObject consultPanel;
    public GameObject videoPanel;
    public GameObject exhibitPanel;
    public GameObject buildingPanel;
    public GameObject marketPanel;

    [Space(10f)]
    public float interactionDistance = 2f;
    private LayerMask interactionLayer;
    // 상호 작용 각도
    private float currentSearchAngle = 120f;
    bool state = false;

    private void Start() 
    {
        interactionLayer = LayerMask.GetMask("Interaction");
    }
    
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetInteraction();
        }
    }

    public bool GetInteraction()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionDistance, transform.forward, 0, interactionLayer);
        if (state == false && hits.Length > 0 &&
            Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(hits[0].transform.position)) < currentSearchAngle / 2.0f)
        {
            var target = hits[0].collider;
            Debug.Log(target.tag);
            switch (target.tag)
            {
                case "Consult":
                    consultPanel.SetActive(true);
                    break;
                case "VideoKiosk":
                    videoPanel.SetActive(true);
                    break;
                case "Exhibit":
                    exhibitPanel.SetActive(true);
                    break;
                case "BuildingKiosk":
                    buildingPanel.SetActive(true);
                    break;
                case "market":
                    marketPanel.SetActive(true);
                    break;
                default:
                    break;
            }
            mouseLock.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            state = true;
        }
        else
        {
            // interact panel inactivate
            if (consultPanel != null) consultPanel.SetActive(false);
            if (videoPanel != null) videoPanel.SetActive(false);
            if (exhibitPanel != null) exhibitPanel.SetActive(false);
            if (buildingPanel != null) buildingPanel.SetActive(false);
            if (marketPanel != null) marketPanel.SetActive(false);

            // mouse state
            mouseLock.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            state = false;
        } 
        return state;
    }
}
