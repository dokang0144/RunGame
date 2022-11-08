using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 700f; // 점프 힘

    public SpriteRenderer ShipSprite;

    public Sprite[] sprites = new Sprite[2];

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private bool isShip = false; // 비행기

    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트

    private void Start()
    {
        ShipSprite = GetComponent<SpriteRenderer>();
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead)
        {
            // 사망시 처리를 더 이상 진행하지 않고 종료
            return;
        }

        if (isShip)
        {
            if (Input.GetMouseButton(0))
            {
                // 리지드바디에 위쪽으로 힘을 주기
                if (playerRigidbody.velocity.y < 20)
                {
                    playerRigidbody.AddForce(new Vector2(0, jumpForce));
                }

            }
        }

        if (isShip == false)
        {
            // 마우스 왼쪽 버튼을 눌렀으며 && 최대 점프 횟수(1)에 도달하지 않았다면
            if (Input.GetMouseButton(0) && jumpCount < 1)
            {
                // 점프 횟수 증가
                jumpCount++;
                // 점프 직전에 속도를 순간적으로 제로(0, 0)로 변경
                playerRigidbody.velocity = Vector2.zero;
                // 리지드바디에 위쪽으로 힘을 주기
                playerRigidbody.AddForce(new Vector2(0, jumpForce));
            }
        }
    }

    private void Die()
    {

        // 속도를 제로(0, 0)로 변경
        playerRigidbody.velocity = Vector2.zero;
        // 사망 상태를 true로 변경
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Dead" && !isDead)
        {
            // 충돌한 상대방의 태그가 Dead이며 아직 사망하지 않았다면 Die() 실행
            Die();
        }
        if (other.tag == "Portal" && !isShip)
        {
            Ship();
        }
        if (other.tag == "PortalOff" && isShip)
        {
            ShipOff();
        }
    }
    private void Ship()
    {
        ShipSprite.sprite = sprites[1];
        jumpForce = 10f;
        // 비행을 true로 변경
        isShip = true;

    }

    private void ShipOff()
    {
        ShipSprite.sprite = sprites[0];
        jumpForce = 700f;
        // 비행을 false로 변경
        isShip = false;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0.7f)
        {
            // isGrounded를 true로 변경하고, 누적 점프 횟수를 0으로 리셋
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
        isGrounded = false;
    }
}