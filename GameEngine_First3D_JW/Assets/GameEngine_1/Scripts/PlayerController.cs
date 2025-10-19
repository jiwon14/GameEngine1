using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- 설정 변수 (Inspector에 노출) ---
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;
    [Header("점프 설정")]
    public float jumpForce = 10.0f;
    [Header("플레이어 정보")]
    public string playerName = "여행자"; // 이름 변수 추가

    // --- 비공개 변수 (스크립트 내부 사용) ---
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;
    private int score = 0;
    private Vector3 startPosition; // 리스폰용 시작 위치

    // --- Unity 생명 주기 함수 ---

    void Awake()
    {
        // Rigidbody2D, Animator, SpriteRenderer 컴포넌트 참조는 Awake에서 미리 확보
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null) Debug.LogError("Rigidbody2D 컴포넌트가 없습니다!");
    }

    void Start()
    {
        // 게임 시작 시 위치를 저장
        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);

        // Animator와 SpriteRenderer 컴포넌트 참조 확인 및 디버그
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogWarning("Animator 컴포넌트가 없습니다! 애니메이션 로직이 작동하지 않습니다.");
        }
        
        Debug.Log("안녕하세요, [" + playerName + "]님");
    }

    void Update()
    {
        // 1. 입력 감지 및 이동 방향 계산
        float moveX = Input.GetAxisRaw("Horizontal"); // -1 (A/왼쪽) 또는 1 (D/오른쪽)
        
        // 2. 물리 기반 이동 (FixedUpdate에서 처리하는 것이 더 좋으나, Input은 Update에서 처리)
        // Y축 속도는 그대로 유지하고, X축 속도만 제어
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        
        // 3. 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            // 애니메이션: 점프 트리거 설정
            if (animator != null)
            {
                // SetBool("Jump", true) 대신 SetTrigger("Jump")를 사용하여 애니메이션 전환이 더 일반적입니다.
                // 기존 코드의 SetBool을 유지하려면 애니메이션 상태 머신에서 "Jump"가 나가는 트랜지션이 필요합니다.
                animator.SetBool("Jump", true); 
                Debug.Log("점프했습니다!");
            }
        }

        // 4. 애니메이션 및 방향 설정
        if (animator != null)
        {
            // 이동 속도 (절대값)를 애니메이터에 전달 (뛰는 애니메이션 등)
            // Rigidbody의 X축 속도를 사용하거나, Input.GetAxisRaw("Horizontal")의 절대값을 사용
            float currentSpeed = Mathf.Abs(moveX * moveSpeed); 
            animator.SetFloat("Speed", currentSpeed); // "Speed" 파라미터로 가정

            // 현재 움직이는 방향에 따라 스프라이트 뒤집기 (렌더러 사용)
            if (moveX < 0) // 왼쪽으로 이동
            {
                spriteRenderer.flipX = true;
            }
            else if (moveX > 0) // 오른쪽으로 이동
            {
                spriteRenderer.flipX = false;
            }
        }
        
        // **Shift 키를 이용한 달리기/속도 조절 로직은 Rigidbody 이동 방식에서는 velocity를 조절해야 합니다.
        // 현재는 단순화하여 moveSpeed를 고정 사용합니다.**
        // (참고: 러닝 로직을 추가하려면 moveSpeed 변수를 임시로 조절하거나, 별도의 힘을 가해야 합니다.)
    }

    // 바닥 충돌 감지 및 착지 시 애니메이션 초기화
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // 착지 시 점프 애니메이션 상태 해제
            if (animator != null)
            {
                animator.SetBool("Jump", false);
            }
        }

        // 장애물 충돌 감지 (리스폰)
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");
            transform.position = startPosition; // 시작 위치로 순간이동
            rb.velocity = Vector2.zero; // 속도 초기화
            // 추가적으로 애니메이션 상태도 초기화할 수 있습니다.
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // 아이템 수집 감지 (Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++; // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject); // 코인 제거
        } 
    }
}