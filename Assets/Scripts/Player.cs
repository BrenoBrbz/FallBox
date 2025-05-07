using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 3f;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

 void Update()
{
  // Verifica se houve um toque na tela
if (Input.touchCount > 0)
{
    Touch touch = Input.GetTouch(0); // A primeira tela de toque

    if (touch.phase == TouchPhase.Began)
    {
        touchStartPos = touch.position; // Armazena a posição inicial do toque
    }

    if (touch.phase == TouchPhase.Ended)
    {
        touchEndPos = touch.position; // Armazena a posição final

        // Calcula a diferença entre as posições
        float deltaX = touchEndPos.x - touchStartPos.x;

        // Desloca o jogador com base na diferença
        float movement = deltaX * swipeSpeed;

        // Aplica a movimentação no personagem
        Vector3 move = new Vector3(movement, 0f, 0f);
        transform.Translate(move * Time.deltaTime);
    }
}


    // Movimento horizontal (Teclado)
    if (!isDashing)
        horizontalInput = -Input.GetAxisRaw("Horizontal");

    // Verifica se está no chão
    isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

    // Pulo
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDashing)
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    // Dash
    if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= lastDashTime + dashCooldown && horizontalInput != 0)
    {
        StartDash();
    }

    // Timer de dash
    if (isDashing)
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            EndDash();
        }
    }
}


    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector3 move = new Vector3(horizontalInput * speed, rb.velocity.y, 0f);
            rb.MovePosition(rb.position + new Vector3(horizontalInput, 0f, 0f) * speed * Time.fixedDeltaTime);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        rb.velocity = new Vector3(horizontalInput * dashForce, 0f, 0f);
        trail.emitting = true; // ativa o rastro
    }

    void EndDash()
    {
        isDashing = false;
        trail.emitting = false; // desativa o rastro
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
