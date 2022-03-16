using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private Vector3 vel;
    private bool isRun;

    private Rigidbody characterRigidbody;
    private Animator animator;
    
    void Start(){
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
 
    void Update(){
        vel.x = Input.GetAxis("Horizontal");                // 입력
        vel.z = Input.GetAxis("Vertical");
        isRun = Input.GetKey(KeyCode.LeftShift);
        if (isRun)  speed = 4f;
        else speed = 3f;

        animator.SetBool("isWalk", vel != Vector3.zero);    // 걷기 애니메이션
        animator.SetBool("isRun", isRun);                   // 뛰기 애니메이션
    }
    
    private void FixedUpdate() {
        // vel.y = 0;
        // transform.LookAt(transform.position + vel);         // 방향

        vel = vel * speed;
        vel.y = characterRigidbody.velocity.y;
        characterRigidbody.velocity = vel;                  // 위치 이동
    }
}
