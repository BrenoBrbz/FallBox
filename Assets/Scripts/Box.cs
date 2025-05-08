using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Movimento")]
    public float rotationSpeed = 360f;
    public float fallSpeed = 2f;
    private bool isFalling = false;

    [Header("Efeito de destruição")]
    public GameObject breakEffect; // Partícula de destruição
    public float delayBeforeEffect = 0.1f;

    [Header("Ajustes dinâmicos")]
    public bool acelerarComTempo = false;
    public float aceleracao = 0.5f;

    public Transform visual;

    void Start()
{
    StartFalling(); // Faz a box começar a cair assim que for ativada
}

private void Update()
{
    if (isFalling)
    {
        if (visual != null)
            visual.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}



    public void StartFalling()
    {
        isFalling = true;
    }

    public void SetFallSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Esconde o visual e o collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Aguarda e instancia o efeito, depois destrói o objeto
        Invoke(nameof(SpawnEffectAndDestroy), delayBeforeEffect);
    }

    private void SpawnEffectAndDestroy()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
