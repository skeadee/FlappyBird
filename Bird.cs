using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bird : MonoBehaviour
{
    public float upForce = 200f; // 힘을 양
    public bool crash;
    bool Delay = false;

    private Rigidbody2D _rb2d;
    private PolygonCollider2D pol;
    private Animator ani;

    FlappyGameManager flappy;

    bool uiCheck = false; // 현재 마우스가 UI에 올라가 있는지 체크하는 함수

    private void Awake()
    {
        flappy = GameObject.Find("FlappyGameManager").GetComponent<FlappyGameManager>();
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        pol = GetComponent<PolygonCollider2D>();
        crash = false;
    }

    void Update()
    {
        if(flappy.GameMode == 4)
        {
            Die();
            return;
        }

        
        GameMode();
    }

    

    void GameMode()
    {
       

        if(Input.GetMouseButtonDown(0) && !Delay && flappy.GameMode != 0) Jump(); // 게임 진행중이고 , 마우스로 클릭을 했을 때
        if (flappy.GameMode == 1) _rb2d.simulated = true; // 게임이 진행중이면 리지드 바디기능 활성화 하기 
    }

    

    void Die() // 플레이어가 죽었을때를 담당하는 함수
    {
        pol.isTrigger = false;
        ani.SetBool("Die", true);
    }

   
    void Jump() // 점프를 담당하는 함수
    {
        if (uiCheck) return; // 만약 마우스가 UI위에 있다면 실행 중지

        Delay = true; 
        _rb2d.velocity = Vector2.zero; // 현재 힘의 방향 초기화
        _rb2d.AddForce(new Vector2(0, upForce)); 
        ani.SetTrigger("Nor");
        StartCoroutine(JumpDelay());
    }

    IEnumerator JumpDelay() // 점프 대기 시간
    {
        yield return new WaitForSeconds(0.3f);
        Delay = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.gameObject.tag == "Wall" && crash == false)
        {
            ani.SetTrigger("Crash");
            flappy.TakeDamage(1);
            crash = true;
        }


        if (col.gameObject.name == "Goal" && crash == false) flappy.AddScore(100);
       
    }

    void OnCollisionEnter2D(Collision2D col) // Player가 땅에 충돌 시 
    {
        if (col.gameObject.tag == "Ground")
        {
            flappy.TakeDamage(99);
            ani.SetBool("Die", true);
        }
        
    }

    public void crash_off()
    {
        crash = false;
    }



    public void uiOn()
    {
        uiCheck = true;
    }

    public void uiOut()
    {
        uiCheck = false;
    }
}
