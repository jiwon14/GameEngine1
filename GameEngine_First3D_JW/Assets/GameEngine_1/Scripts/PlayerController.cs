using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public string playerName="지원";
    
    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 디버그: 제대로 찾았는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }
        Debug.Log("안녕하세요,[" + playerName + "]님");
    }
    
    void Update()
{
    // 이동 벡터 계산
    Vector3 movement = Vector3.zero;
    
    if (Input.GetKey(KeyCode.A))
    {
        movement += Vector3.left;
            //transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
            spriteRenderer.flipX = true;
    }

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
            //transform.localScale = new Vector3(1, 1, 1); // 원래 방향
            spriteRenderer.flipX = false;
        
    }
    
    if (Input.GetKey(KeyCode.W))
    {
        movement += Vector3.up;
        
    }

    if (Input.GetKey(KeyCode.S))
    {
        movement += Vector3.down;
        
    }

     float currentMoveSpeed = moveSpeed;
    if (Input.GetKey(KeyCode.LeftShift))
    {
        currentMoveSpeed = moveSpeed * 2f;
        Debug.Log("달리기 모드 활성화!");
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
            if (animator != null)
            {
                animator.SetBool("Jump", true);
                Debug.Log("점프했습니다!");
            }
    }
   
        // 실제 이동 적용
        if (movement != Vector3.zero)
        {
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
        }
    
    // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
    float currentSpeed = movement != Vector3.zero ? currentMoveSpeed : 0f;
    
    // Animator에 속도 전달
    if (animator != null)
    {
        animator.SetFloat("speed", currentSpeed);
        Debug.Log("Current speed: " + currentSpeed);
    }
}
}