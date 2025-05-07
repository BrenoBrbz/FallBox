using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScenes : MonoBehaviour
{
    public string sceneToOpen;
    public float delayInSeconds = 2f; // Tempo de espera antes de trocar a cena

    public void OpenScene()
    {
        StartCoroutine(DelayedSceneLoad());
    }

    private System.Collections.IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneToOpen);
    }
}

