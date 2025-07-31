using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [Header("Detección de proximidad")]
    public float activationRadius = 3f;

    [Header("Tecla de interacción")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Visuales")]
    public ParticleSystem Particles;
    public GameObject Keyword;
    public GameObject uiPrompt;

    [Header("Lore")]
    public GameObject lorePanel;

    Transform player;
    GameObject worldPrompt;
    bool inRange = false;
    bool loreOpen = false;
    int loreLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        loreLayer = LayerMask.NameToLayer("Lore");

        // Instancio la letra flotante como hijo y la apago
        if (Keyword != null)
        {
            worldPrompt = Instantiate(Keyword, transform);
            worldPrompt.SetActive(false);
        }

        if (uiPrompt != null)
            uiPrompt.SetActive(false);

        if (lorePanel != null)
            lorePanel.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        // Check rango
        float dist = Vector3.Distance(player.position, transform.position);
        bool shouldBeIn = dist <= activationRadius;

        if (!inRange && shouldBeIn) EnterRange();
        else if (inRange && !shouldBeIn) ExitRange();

        // Solo objetos en layer “Lore” responden a E para abrir/cerrar lore
        if (inRange
            && gameObject.layer == loreLayer
            && Input.GetKeyDown(interactKey))
        {
            ToggleLore();
        }
    }

    void EnterRange()
    {
        inRange = true;
        Particles?.Play();
        worldPrompt?.SetActive(true);
        uiPrompt?.SetActive(true);
    }

    void ExitRange()
    {
        inRange = false;
        Particles?.Stop();
        worldPrompt?.SetActive(false);
        uiPrompt?.SetActive(false);
        CloseLore();
    }

    void ToggleLore()
    {
        if (lorePanel == null) return;
        loreOpen = !loreOpen;
        lorePanel.SetActive(loreOpen);

        // mientras el lore esté abierto, oculto prompts
        worldPrompt?.SetActive(!loreOpen);
        uiPrompt?.SetActive(!loreOpen);
    }

    void CloseLore()
    {
        if (!loreOpen) return;
        loreOpen = false;
        lorePanel.SetActive(false);
        worldPrompt?.SetActive(true);
        uiPrompt?.SetActive(true);
    }
}
