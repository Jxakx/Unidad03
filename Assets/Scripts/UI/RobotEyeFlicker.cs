using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RobotEyesFlickerObjects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Luces")]
    public Light eyeLightLeft;
    public Light eyeLightRight;

    [Header("GameObjects de los ojos")]
    public GameObject eyeObjLeft;
    public GameObject eyeObjRight;

    [Header("Flicker settings")]
    [Tooltip("Duración total del parpadeo (segundos)")]
    public float flickerDuration = 0.5f;
    [Tooltip("Intervalo entre cada cambio de estado (segundos)")]
    public float flickerSpeed = 0.05f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSfx;

    private Coroutine flickerRoutine;
    private bool originalLeftActive;
    private bool originalRightActive;

    void Start()
    {
        // Guardamos el estado original de los eye GameObjects
        originalLeftActive = eyeObjLeft.activeSelf;
        originalRightActive = eyeObjRight.activeSelf;

        // Empiezan todos apagados
        eyeLightLeft.enabled = false;
        eyeLightRight.enabled = false;
        eyeObjLeft.SetActive(false);
        eyeObjRight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Arranca el parpadeo
        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        flickerRoutine = StartCoroutine(FlickerAndHold());

        // Toca sonido de hover
        if (hoverSfx != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(hoverSfx);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Detiene el parpadeo y apaga todo al instante
        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        eyeLightLeft.enabled = false;
        eyeLightRight.enabled = false;
        eyeObjLeft.SetActive(false);
        eyeObjRight.SetActive(false);
    }

    private IEnumerator FlickerAndHold()
    {
        float elapsed = 0f;
        bool state = false;

        while (elapsed < flickerDuration)
        {
            // Alterna luces
            eyeLightLeft.enabled = state;
            eyeLightRight.enabled = state;
            // Alterna GameObjects de ojos
            eyeObjLeft.SetActive(state);
            eyeObjRight.SetActive(state);

            state = !state;
            elapsed += flickerSpeed;
            yield return new WaitForSeconds(flickerSpeed);
        }

        // Deja todo ENCENDIDO al final
        eyeLightLeft.enabled = true;
        eyeLightRight.enabled = true;
        eyeObjLeft.SetActive(originalLeftActive);
        eyeObjRight.SetActive(originalRightActive);
    }
}
