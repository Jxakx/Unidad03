using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Nota1Interactuable : MonoBehaviour
{
    public GameObject interactTextUI; // Texto "Presiona E para interactuar"
    public GameObject lorePanelUI;    // Panel con el texto del diario
    public TextMeshProUGUI loreText;  // Texto del diario

    [TextArea(5, 15)]
    public string diaryEntry; // Entrada personalizada de diario

    private bool isPlayerInRange = false;

    void Start()
    {
        interactTextUI.SetActive(false);
        lorePanelUI.SetActive(false);
    }

    void Update()
    {
        // Al presionar E dentro del rango, alterna la visibilidad del panel
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (lorePanelUI.activeSelf)
            {
                // Si ya está visible, lo cierra y muestra el texto de interactuar
                lorePanelUI.SetActive(false);
                interactTextUI.SetActive(true);
            }
            else
            {
                // Si está cerrado, lo abre y oculta el texto de interactuar
                lorePanelUI.SetActive(true);
                loreText.text = diaryEntry;
                interactTextUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactTextUI.SetActive(false);
            lorePanelUI.SetActive(false);
        }
    }
}
