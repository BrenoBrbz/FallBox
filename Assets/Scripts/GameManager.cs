using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject InimigoPrefab;

    void Start()
    {
        StartCoroutine(SpawnInimigo());
    }

    private IEnumerator SpawnInimigo()
    {
        int InimigosSpawns = Random.Range(1, 4);

        for (int i = 0; i < InimigosSpawns; i++)
        {
            float x = Random.Range(33.50f, 66.50f);
            float drag = Random.Range(0f, 2f);

            GameObject box = Instantiate(InimigoPrefab, new Vector3(x, 190, 145), Quaternion.identity);
            box.GetComponent<Box>().StartFalling(); // <- Faz a box começar a cair
        }

        yield return new WaitForSeconds(0.6f);
        yield return SpawnInimigo();
    }
}
