using UnityEngine;
using UnityEngine.SceneManagement;

public class Intros : MonoBehaviour
{
    public string nomeDaCena;         // Nome da cena que será carregada
    public float tempoParaTrocar = 5f; // Tempo em segundos até trocar de cena

    void Start()
    {
        Invoke("TrocarCena", tempoParaTrocar);
    }

    void TrocarCena()
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}

