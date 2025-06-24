using UnityEngine;

public class Flashlights : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private Light linternaIzquierda;
    [SerializeField] private Light linternaDerecha;

    [Header("Head Bones")]
    [SerializeField] private Transform[] targetHuesos; // Array de huesos de cabeza

    [Header("Position Adjustments")]
    [SerializeField] private float alturaLinternaIzquierda = 0.2f;
    [SerializeField] private float alturaLinternaDerecha = 0.2f;
    [SerializeField] private float desplazamientoHorizontalIzquierda = -0.1f;
    [SerializeField] private float desplazamientoHorizontalDerecha = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoEncender;
    [SerializeField] private AudioClip sonidoApagar;

    private AudioSource audioSource;
    private bool lucesEncendidas;
    private Transform activeBone; // Hueso activo actual

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lucesEncendidas = linternaIzquierda.enabled;
        FindActiveBone(); // Buscar hueso activo inicial
    }

    void LateUpdate()
    {
        if (lucesEncendidas)
        {
            // Actualizar si el hueso activo cambió
            if (activeBone == null || !activeBone.gameObject.activeInHierarchy)
            {
                FindActiveBone();
            }

            if (activeBone != null)
            {
                UpdateLightTransforms();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLuces();
        }
    }

    // Busca el primer hueso activo en el array
    private void FindActiveBone()
    {
        foreach (Transform bone in targetHuesos)
        {
            if (bone != null && bone.gameObject.activeInHierarchy)
            {
                activeBone = bone;
                return;
            }
        }
        activeBone = null;
    }

    private void UpdateLightTransforms()
    {
        // Mantenemos posición original del hueso
        Vector3 posicionHueso = activeBone.position;

        // Aplicar ajustes de posición a cada linterna
        linternaIzquierda.transform.position = new Vector3(
            posicionHueso.x + desplazamientoHorizontalIzquierda,
            posicionHueso.y + alturaLinternaIzquierda,
            posicionHueso.z
        );

        linternaDerecha.transform.position = new Vector3(
            posicionHueso.x + desplazamientoHorizontalDerecha,
            posicionHueso.y + alturaLinternaDerecha,
            posicionHueso.z
        );

        // Rotación sin cambios (igual al hueso)
        linternaIzquierda.transform.rotation = activeBone.rotation;
        linternaDerecha.transform.rotation = activeBone.rotation;
    }

    private void ToggleLuces()
    {
        lucesEncendidas = !lucesEncendidas;

        linternaIzquierda.enabled = lucesEncendidas;
        linternaDerecha.enabled = lucesEncendidas;

        audioSource.PlayOneShot(lucesEncendidas ? sonidoEncender : sonidoApagar);

        // Buscar hueso activo al encender
        if (lucesEncendidas) FindActiveBone();
    }

    // Método para forzar actualización de huesos
    public void RefreshBones()
    {
        FindActiveBone();
        if (lucesEncendidas) UpdateLightTransforms();
    }

    // Métodos para ajustar posición en tiempo real
    public void SetDesplazamientoHorizontal(float izquierda, float derecha)
    {
        desplazamientoHorizontalIzquierda = izquierda;
        desplazamientoHorizontalDerecha = derecha;
        if (lucesEncendidas) UpdateLightTransforms();
    }

    public void SetAlturaLinternas(float altura)
    {
        alturaLinternaIzquierda = altura;
        alturaLinternaDerecha = altura;
        if (lucesEncendidas) UpdateLightTransforms();
    }
}