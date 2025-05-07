using UnityEngine;

public class Box : MonoBehaviour
{
    public float rotationSpeed = 360f;
    public float fallSpeed = 2f;
    private bool isFalling = false;
    public GameObject breakEffect; // Partícula de destruição
    public float delayBeforeEffect = 0.1f;
    
    private void Update()
    {
        if (isFalling)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void StartFalling()
    {
        isFalling = true;
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


