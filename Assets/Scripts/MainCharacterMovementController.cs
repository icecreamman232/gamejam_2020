using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovementController : MonoBehaviour
{
    public float speed = 1.0f;
    public Rigidbody2D m_rigid;
    private Vector3 movement;
    private float rotateAngle = 0.0f;
    public float rotateSpd = 1.0f;
    public bool isDeath = false;
    public Vector3 last_pos;
    public Animator animator;
    public enum state
    {
        IDLE    = 1,
        GO      = 2,
    }
    public state m_state;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if(!isDeath)
        {
            movement = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0f);
            if (last_pos != movement)
            {
                if (movement != Vector3.zero)
                {
                    rotateAngle = Vector3.Angle(Vector3.up, movement);
                    Quaternion rotation = Quaternion.Euler(0, 0, rotateAngle);
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, movement);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpd);
                    rotateAngle -= rotateSpd;
                    this.animator.Play("idle");
                    this.animator.Update(0f);
                    //this.GetComponent<Animation>().Play("Go");
                }
                m_rigid.velocity = movement;
                last_pos = this.transform.position;
            }
            else
            {
                
               // this.animator.Play("idle");
                //this.animator.Update(0f);
            }
        }        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Trap_Pitfall")
        {
            isDeath = true;
            //Tăng dần thì đẹp hơn
            StartCoroutine(ScaleDown(0.009f));
            
        }
       
    }
    IEnumerator ScaleDown(float scale_spd)
    {
        while (this.transform.localScale.x >= 0)
        {
            Vector3 tmp = this.transform.localScale;
            tmp.x -= scale_spd+0.001f;
            tmp.y -= scale_spd+ 0.001f;
            this.transform.localScale = tmp;
            //this.transform.Rotate(Vector3.back,0.01f);
            //this.transform.RotateAroundLocal(Vector3.back, 0.01f);
            yield return null;
        }
        
    }

}
