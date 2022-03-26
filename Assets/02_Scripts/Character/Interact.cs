using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interact Panel")]
    public GameObject consultPanel;

    [Space(10f)]
    public float interactionDistance = 2f;
    private LayerMask interactionLayer;
    // 상호 작용 각도
    private float currentSearchAngle;

    private void Start() {
        interactionLayer = LayerMask.GetMask("Interaction");
        currentSearchAngle = 120f;
    }

    private void Update() 
    {
        
    }

    public void GetInteraction()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionDistance, transform.forward, 0, interactionLayer);
        if (hits.Length > 0 &&
            Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(hits[0].transform.position)) < currentSearchAngle / 2.0f)
        {
            var target = hits[0].collider;
            switch (target.tag)
            {
                case "Consult": // Consultant 가까이 가면 Consult 패널 활성화
                    consultPanel.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    break;
                
                default:
                    break;
            }
        }
    }
}
