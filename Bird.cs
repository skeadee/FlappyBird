using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bird : MonoBehaviour
{
    public float upForce = 200f; // 위로 올라갈 힘의 양
    bool Delay = false;

    private Rigidbody2D _rb2d;
    private PolygonCollider2D pol;
    private Animator ani;

    FlappyGameManager flappy;


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


  
}
