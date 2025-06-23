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
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            lorePanelUI.SetActive(true);
            loreText.text = diaryEntry;
            interactTextUI.SetActive(false); 
        }

        if (lorePanelUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            lorePanelUI.SetActive(false);

            // Mostrar nuevamente el cartel si aún estás en rango
            if (isPlayerInRange)
            {
                interactTextUI.SetActive(true);
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
