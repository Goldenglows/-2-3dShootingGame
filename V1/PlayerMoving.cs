using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    //移动速度
    public float Speed = 6f;

    private Rigidbody rb;
    private Animator anim;
    private int groundLayerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundLayerMask = LayerMask.GetMask("Ground");

        // 验证组件是否为空
        if (rb == null) Debug.LogError("Rigidbody 未分配 on " + gameObject.name);
        if (anim == null) Debug.LogError("Animator 未分配 on " + gameObject.name);
        if (groundLayerMask == 0) Debug.LogError("Ground Layer 未找到");

    }

    private void FixedUpdate()
    {

        //暂停
        if (Time.timeScale == 0f) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);

        Turning();

        Animating(h, v);
    }

    void Move(float h,float v)
    {
        Vector3 movementV3 = new Vector3(h, 0, v);
        movementV3 = movementV3.normalized * Speed * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + movementV3);
    }

    void Turning()
    {
        //创建相机，根据鼠标的位置
        Ray cameraray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int groundLayer = LayerMask.GetMask("Ground");

        RaycastHit groundHit;
        //射线检测,用射线射100个单位，直到射中地面图层
        bool isTouchGround = Physics.Raycast(cameraray, out groundHit, 100, groundLayer);
        if (isTouchGround)
        {
            Vector3 v3 = groundHit.point - transform.position;
            v3.y = 0;

            Quaternion quaternion = Quaternion.LookRotation(v3);
            rb.MoveRotation(quaternion);    
        }
         
    }

    void Animating (float h, float v)
    {
        bool isWalking = false;
        if (h != 0||v != 0)
        {
            isWalking = true;
        }
        anim.SetBool("IsWalking",isWalking);
    }

}
