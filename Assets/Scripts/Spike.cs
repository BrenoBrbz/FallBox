using UnityEngine;

public class Spike : MonoBehaviour
{
    public float riseHeight = 2f;            // Altura do espinho
    public float riseSpeed = 2f;             // Velocidade da subida
    public GameObject tremorEffectPrefab;    // Prefab das partículas de tremor

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool rising = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * riseHeight;

        // Instanciar as partículas de tremor
        if (tremorEffectPrefab != null)
        {
            GameObject tremorEffect = Instantiate(tremorEffectPrefab, startPos, Quaternion.identity);
            Destroy(tremorEffect, 1f);  // Destroi o efeito de tremor após 1 segundo
        }

        // Começar a subida do espinho depois de um pequeno atraso
        Invoke(nameof(StartRising), 1f);  // Espera 1 segundo antes de começar a subir
    }

    void StartRising()
    {
        rising = true;
    }

    void Update()
    {
        if (rising)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, riseSpeed * Time.deltaTime);

            if (transform.position == targetPos)
                Destroy(gameObject);  // Destrói o espinho após atingir a altura
        }
    }
}
