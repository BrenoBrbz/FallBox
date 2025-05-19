using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 5f;
    private float moveInput;
    public float jumpForce = 5;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Dash")]
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTimer;
    private float lastDashTime;
    private float dashTimeLeft;

    [Header("Efeitos")]
    public GameObject deathEffect;

    public float swipeSpeed = 0.1f; // Sensibilidade do toque

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Rigidbody rb;
    private bool isDead = false;
    private float horizontalInput = 0f;
    private bool isGrounded;
    private TrailRenderer trail;
    private float lastTapTimeLeft = 0f;
    private float lastTapTimeRight = 0f;
    private float doubleTapThreshold = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

    void Update()
    {
        HandleTouchControls();

        // Movimento via teclado
        if (!isDashing)
            horizontalInput = -Input.GetAxisRaw("Horizontal"); // "-" pois você estava invertendo antes

        // Verifica se está no chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Pulo com teclado
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDashing)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        // Dash com teclado
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= lastDashTime + dashCooldown && horizontalInput != 0)
        {
            StartDash();
        }

        // Timer do dash
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
    }

    void HandleTouchControls()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPos = touch.position;
                bool isLeft = touchPos.x < Screen.width / 2;
                bool isRight = touchPos.x >= Screen.width / 2;

                // Movimento por toque
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    if (isLeft) horizontalInput = -1;
                    else if (isRight) horizontalInput = 1;
                }

                // Pulo (arrasto para cima)
                if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 delta = touch.position - touch.rawPosition;
                    if (delta.y > 100 && isGrounded && !isDashing)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    }

                    // Dash com dois toques no mesmo lado
                    if (isLeft)
                    {
                        if (Time.time - lastTapTimeLeft < doubleTapThreshold && !isDashing)
                        {
                            horizontalInput = -1;
                            StartDash();
                        }
                        lastTapTimeLeft = Time.time;
                    }
                    else if (isRight)
                    {
                        if (Time.time - lastTapTimeRight < doubleTapThreshold && !isDashing)
                        {
                            horizontalInput = 1;
                            StartDash();
                        }
                        lastTapTimeRight = Time.time;
                    }
                }
            }
        }
    }

void FixedUpdate()
{
    if (!isDashing)
    {
        float moveForce = 20f;           // menor força, porque ForceMode.Force é contínuo
        float torqueAmount = 1f;         // rotação suave
        float maxSpeed = 3f;             // limite de velocidade

        // Limita a velocidade máxima
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector3(horizontalInput * moveForce, 0f, 0f), ForceMode.Force);
        }

        // Rotação controlada
        rb.AddTorque(new Vector3(0f, 0f, -horizontalInput * torqueAmount), ForceMode.Force);
    }
}



    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        rb.velocity = new Vector3(horizontalInput * dashForce, 0f, 0f);
        trail.emitting = true;
    }

    void EndDash()
    {
        isDashing = false;
        trail.emitting = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Inimigo") && !isDead)
        {
            isDead = true;

            if (deathEffect != null)
                deathEffect.SetActive(true);

            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;

            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = false;

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.SaveHighScore();

            Invoke(nameof(ReloadScene), 0.5f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
