using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody rb;
    private bool isDead = false;

    public GameObject deathEffect; // Prefab do efeito de morte

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDead) return;

        float horizontalInput = 0f;

        // Teclado (INVERTIDO)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            horizontalInput = -1f;

        // Toque na tela (INVERTIDO)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float screenCenterX = Screen.width / 2f;

            if (touch.position.x < screenCenterX)
                horizontalInput = 1f;
            else
                horizontalInput = -1f;
        }

        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, rb.velocity.z);
        rb.velocity = movement;
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Inimigo") && !isDead)
    {
        isDead = true;

        // Ativa o efeito de morte
        if (deathEffect != null)
        {
            deathEffect.SetActive(true);
        }

        // Desativa todos os renderers do player (inclusive em objetos filhos)
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        // Desativa colisores
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        // Para movimento do player
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Salva HighScore
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.SaveHighScore();
        }

        // Recarrega a cena depois de um pequeno atraso
        Invoke(nameof(ReloadScene), 0.5f);
    }
}

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
