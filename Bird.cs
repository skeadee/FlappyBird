using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Bird : MonoBehaviour
{
    public float upForce = 200f; // ���� ��
    public bool crash;
    bool Delay = false;

    private Rigidbody2D _rb2d;
    private PolygonCollider2D pol;
    private Animator ani;

    FlappyGameManager flappy;

    bool uiCheck = false; // ���� ���콺�� UI�� �ö� �ִ��� üũ�ϴ� �Լ�

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
       

        if(Input.GetMouseButtonDown(0) && !Delay && flappy.GameMode != 0) Jump(); // ���� �������̰� , ���콺�� Ŭ���� ���� ��
        if (flappy.GameMode == 1) _rb2d.simulated = true; // ������ �������̸� ������ �ٵ��� Ȱ��ȭ �ϱ� 
    }

    

    void Die() // �÷��̾ �׾������� ����ϴ� �Լ�
    {
        pol.isTrigger = false;
        ani.SetBool("Die", true);
    }

   
    void Jump() // ������ ����ϴ� �Լ�
    {
        if (uiCheck) return; // ���� ���콺�� UI���� �ִٸ� ���� ����

        Delay = true; 
        _rb2d.velocity = Vector2.zero; // ���� ���� ���� �ʱ�ȭ
        _rb2d.AddForce(new Vector2(0, upForce)); 
        ani.SetTrigger("Nor");
        StartCoroutine(JumpDelay());
    }

    IEnumerator JumpDelay() // ���� ��� �ð�
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

    void OnCollisionEnter2D(Collision2D col) // Player�� ���� �浹 �� 
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
