using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject InimigoPrefab;  // Tipo correto para o prefab do inimigo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnInimigo());
    }

    // Coroutine para spawnar inimigos
    private IEnumerator SpawnInimigo()
    {
        
        var InimigosSpawns = Random.Range(1, 4);

        for (int i = 0; i < InimigosSpawns; i++)
        {
// Gera uma posição X aleatória entre 33 e 66
        float x = Random.Range(33.50f , 66.50f);  
        var drag = Random.Range(0f, 2f);
         // Instancia o prefab do inimigo na posição gerada
        Instantiate(InimigoPrefab, new Vector3( x, 190, 145), Quaternion.identity);
        }
       

        // Espera um frame antes de continuar
        yield return new WaitForSeconds(0.6f);  

        yield return SpawnInimigo();
    }
}

