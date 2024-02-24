using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bird : MonoBehaviour
{
    public float upForce = 200f; // 위로 올라갈 힘의 양
    bool Delay = false; // 점프 딜레이를 감지하는 변수  
    bool crash = false; // 충돌을 감지하는 변수 

    private Rigidbody2D _rb2d;
    private PolygonCollider2D pol;
    private Animator ani;

    FlappyGameManager flappy; // 게임을 관리하는 매니저


ㅁㄴㅇㅁㄴㅇ

    private void Awake()
    {
        flappy = GameObject.Find("FlappyGameManager").GetComponent<FlappyGameManager>();
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        pol = GetComponent<PolygonCollider2D>();
        _rb2d.simulated = true;
        crash = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Jump();
    }

    private void Jump() // 점프를 담당하는 함수
    {
        if (Delay) return; // 딜레이가 아직 안끝났다면 점프를 하지 않는다

        Delay = true;
        _rb2d.velocity = Vector2.zero; // 현재 힘의 방향 초기화
        _rb2d.AddForce(new Vector2(0, upForce)); // 위쪽으로 upForce만큼 힘을 준다
        ani.SetTrigger("Nor"); // 애니메이션 전환 
        StartCoroutine(JumpDelay()); //  다음 점프까지 걸리는 딜레이를 계산하는 코루틴
    }

    IEnumerator JumpDelay() // 점프 대기 시간
    {
        yield return new WaitForSeconds(0.3f);
        Delay = false;
    }


    private void OnTriggerEnter2D(Collider2D col) // player가 is Trigger = true인 Collider에 닿았을 때 
    {
        // crash == false -> 현재 충돌이 가능한 상태
        // crash == true -> 현재 충돌이 불가능한 상태(무적 상태)

        if (col.gameObject.tag == "Wall" && crash == false) // 충돌한 게임 오브젝트의 tag가 Wall이라면
        {
            ani.SetTrigger("Crash"); // 충돌 애니메이션으로 전환
            flappy.TakeDamage(1); // 게임매니저에 있는 데미지 추가 함수를 실행
            crash = true; // 연속 충돌을 방지를 위해 crash를 true로 변경
        }

        if (col.gameObject.name == "Goal" && crash == false) flappy.AddScore(100); // 충돌한 게임 오브젝트의 tag가 Goal이라면 게임매니저에 있는 점수 추가 함수 실행

    }

    void OnCollisionEnter2D(Collision2D col) // Player가 is Trigger = false인 Collider에 닿았을 때 
    {
        if (col.gameObject.tag == "Ground") // 충돌한 tag가 Ground라면
        {
            flappy.TakeDamage(99); // 최대 HP가 넘어가는 변수를 전달
            ani.SetBool("Die", true); // 죽는 애니메이션 실행
        }

    }

    public void crash_off() // 충돌 애니메이션 안에 있는 함수(애니메이션이 끝날 때쯤에 실행)
    {
        crash = false; // 다시 충돌이 가능한 상태로 변환 
    }


}
