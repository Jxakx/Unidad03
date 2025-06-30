using UnityEngine;
using System.Collections;

[ExecuteAlways]
public class IrisController : MonoBehaviour
{
    [Header("Material y Parámetros del Shader")]
    public Material irisMaterial;

    [Header("Movimiento")]
    [Range(0f, 0.5f)]
    public float maxOffset = 0.2f;        // Máximo desplazamiento deseado
    public float minMoveTime = 0.5f;
    public float maxMoveTime = 2f;

    [Header("Parpadeo")]
    public float blinkCloseTime = 0.1f;
    public float blinkOpenTime = 0.2f;
    public Vector2 blinkInterval = new Vector2(3f, 7f);
    public float openRadius = 0.2f;
    public float closedRadius = 0.01f;

    void OnEnable()
    {
        if (irisMaterial == null)
            irisMaterial = GetComponent<Renderer>()?.material;

        // Empieza con iris centrado
        irisMaterial.SetVector("_IrisOffset", Vector2.zero);
        irisMaterial.SetFloat("_IrisRadius", openRadius);

        StopAllCoroutines();
        StartCoroutine(IrisRoutine());
    }

    IEnumerator IrisRoutine()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        while (true)
        {
            // Calcula el límite real de Offset basado en el radio
            float irisRad = irisMaterial.GetFloat("_IrisRadius");
            float limit = Mathf.Min(maxOffset, 0.5f - irisRad);

            // Movimiento: de la posición actual a una dentro del círculo permitido
            Vector2 start = irisMaterial.GetVector("_IrisOffset");
            Vector2 target = Random.insideUnitCircle * limit;
            float moveTime = Random.Range(minMoveTime, maxMoveTime);
            yield return StartCoroutine(LerpOffset(start, target, moveTime));

            // Espera antes de parpadear
            yield return new WaitForSeconds(Random.Range(blinkInterval.x, blinkInterval.y));

            // Parpadeo
            yield return StartCoroutine(LerpRadius(openRadius, closedRadius, blinkCloseTime));
            yield return StartCoroutine(LerpRadius(closedRadius, openRadius, blinkOpenTime));
        }
    }

    IEnumerator LerpOffset(Vector2 from, Vector2 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float f = Mathf.SmoothStep(0f, 1f, t / duration);
            irisMaterial.SetVector("_IrisOffset", Vector2.Lerp(from, to, f));
            yield return null;
        }
        irisMaterial.SetVector("_IrisOffset", to);
    }

    IEnumerator LerpRadius(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float f = t / duration;
            irisMaterial.SetFloat("_IrisRadius", Mathf.Lerp(from, to, f));
            yield return null;
        }
        irisMaterial.SetFloat("_IrisRadius", to);
    }

}
