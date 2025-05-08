using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private bool isMuted = false;

    void Start()
    {
        // Carrega o estado salvo, se existir
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;

        // Salva o estado
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
