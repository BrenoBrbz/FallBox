using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo..."); // Só aparece no Editor
        Application.Quit();
    }
}
