using UnityEngine;

public class FadeEndLevel : MonoBehaviour
{
    [SerializeField] private FadingScript fadingScript;
    [SerializeField] private AudioSource ambientAudio; // referencia al audio source
    [SerializeField] private AudioSource EndTheme; // referencia al audio source

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadingScript != null)
                fadingScript.FadeOut();

            if (ambientAudio != null && ambientAudio.isPlaying)
                ambientAudio.Pause(); // pausa el sonido de ambiente

            if (EndTheme != null && !EndTheme.isPlaying)
                EndTheme.Play(); // Reproducir nuevo sonido
        }
    }
}
