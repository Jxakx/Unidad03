using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonController))]
public class PlayerDash : MonoBehaviour
{
    private ThirdPersonController playerScript;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;   // Velocidad del dash
    public float dashTime = 0.2f;  // Duraci�n en segundos

    private void Awake()
    {
        playerScript = GetComponent<ThirdPersonController>();
    }

    /// <summary>
    /// Llamar desde ComboGarras para ejecutar el dash sincronizado.
    /// </summary>
    public void TriggerDash()
    {
        if (playerScript.IsGrounded && !playerScript.IsDashing)
            StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        playerScript.EnableMovement = false;
        playerScript.IsDashing = true;

        float end = Time.time + dashTime;
        while (Time.time < end)
        {
            playerScript.Controller.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }

        playerScript.IsDashing = false;
        playerScript.EnableMovement = true;
    }
}
