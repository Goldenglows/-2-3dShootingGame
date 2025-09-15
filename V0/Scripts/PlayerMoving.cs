using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    //�ƶ��ٶ�
    public float Speed = 6f;

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
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
        //�����������������λ��
        Ray cameraray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int groundLayer = LayerMask.GetMask("Ground");

        RaycastHit groundHit;
        //���߼��,��������100����λ��ֱ�����е���ͼ��
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
