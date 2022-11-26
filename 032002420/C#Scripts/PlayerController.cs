using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;     //私人，简化界面
    private Animator anim;      //动画
    public Collider2D coll;     //碰撞体
    public float jumpforce;     //跳跃力
    public float speed = 10f;   //初速
    public LayerMask ground;    
    public int Score = 0;
    public Text Bonus;          //改变UI得分

    private bool IsHurt;         //是否受伤,默认false

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//获取实例-刚体
        anim = GetComponent<Animator>();
    }

    void Update()
    {
    //    if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
    //     {
    //         rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    //         //跳跃动画,参数jumping为true
    //         anim.SetBool("jumping",true);
    //     }
    }
    //电脑性能不足，平衡帧率,要参数*Time.deltaTime
    void FixedUpdate()
    {
        //若未受伤，可执行移动
        if(!IsHurt)
        {
            Movement();
        }
        switchAnim ();
    }
    //
    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");//-1~1
        float facedirection = Input.GetAxisRaw("Horizontal");//-1,0,1

        //角色移动
        if(horizontalmove != 0)
        {
            //rb刚体的速度=二维矢量（x按键*速度，y不变）
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            //给动画效果参数‘running’设定值
            anim.SetFloat("running",Mathf.Abs(horizontalmove));
        }
        //人物朝向
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection,1,1);
        }
        //按下跳跃键且接触ground图层
        
        if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            //跳跃动画,参数jumping为true
            anim.SetBool("jumping",true);
        }
    } 
    //切换动画
    void switchAnim ()
    {
        //防止idleing保持true
        anim.SetBool("idleing",false);
        //未向上且未触地
        if(rb.velocity.y < 0.1f &&!coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",true);
        }
        //判断是否跳跃
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        //如果受伤
        else if(IsHurt)
        {   
            //受伤
            anim.SetBool("hurting",true);
            anim.SetFloat("running",0);
            //x速度<0.1f时，不再处于受伤状态
            if(Mathf.Abs(rb.velocity.x)<0.1f)
            {
                anim.SetBool("hurting",false);
                anim.SetBool("idleing",true);
                IsHurt = false;
            }
        }
        //如果接触ground图层
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
            anim.SetBool("idleing",true);
        }

    }
    //收集
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Collection")
        {
            Destroy(other.gameObject);
            Score +=1;
            Bonus.text = Score.ToString();
        }
    }
    //消灭敌人与受伤
    private void OnCollisionEnter2D(Collision2D other) 
    {
        //碰到敌人
        if(other.gameObject.tag == "Enemy")
        {
            //生成实体frog，类是Enemt_frog
            Enemy_Frog frog = other.gameObject.GetComponent<Enemy_Frog>();
            //跳跃时，消灭并跳起
            if(anim.GetBool("falling"))
            {
                frog.JumpOn();
                
                 rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                anim.SetBool("jumping",true);
            }
            //在左侧碰到敌人，向左移动
            else if(transform.position.x < other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10,rb.velocity.y);
                IsHurt = true;
            }
            //在右侧碰到敌人，向右移动
            else if(transform.position.x > other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10,rb.velocity.y);
                IsHurt = true;
            }
        }
        
    }

}
