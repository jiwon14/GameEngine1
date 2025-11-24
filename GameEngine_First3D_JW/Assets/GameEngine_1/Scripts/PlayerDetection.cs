using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;  // 추적 속도
    
    private Transform player;                          // 플레이어 Transform
    private bool isChasing = false;                    // 추적 중 여부
    private EnemyPatrol patrolScript;                  // 순찰 스크립트 참조
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        patrolScript = GetComponent<EnemyPatrol>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
    }
    
    void ChasePlayer()
    {
        // 플레이어를 향해 이동
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );
        
        // 이동 방향에 맞춰 스프라이트 반전
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;   // 왼쪽
        }
        else
        {
            spriteRenderer.flipX = false;  // 오른쪽
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
            patrolScript.enabled = false;  // 순찰 중지
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            patrolScript.enabled = true;   // 순찰 재개
            player = null;
        }
    }
}