using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [Header("Rangos de proximidad")]
    public float uiRadius = 3f;
    public float particleRadius = 8f;  // Punto donde comienza a prenderse

    [Header("Tecla de interacción")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Visuales")]
    public ParticleSystem Particles;
    public GameObject InteractuableUI;

    [Header("Lore")]
    public GameObject lorePanel;

    Transform player;
    GameObject worldPrompt;
    bool uiInRange = false;
    bool loreOpen = false;
    int loreLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        loreLayer = LayerMask.NameToLayer("Lore");

        if (InteractuableUI != null)
            InteractuableUI.SetActive(false);

        if (lorePanel != null)
            lorePanel.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        float stopDistance = uiRadius + 2f;  // apagado automático

        // — Partículas: si estamos ENTRE (stopDistance, particleRadius]
        if (Particles != null)
        {
            if (dist > stopDistance && dist <= particleRadius)
            {
                if (!Particles.isPlaying)
                    Particles.Play();
            }
            else
            {
                if (Particles.isPlaying)
                    Particles.Stop();
            }
        }

        // — UI y prompts dentro de uiRadius
        bool shouldHaveUI = dist <= uiRadius;
        if (!uiInRange && shouldHaveUI)
            EnterUI();
        else if (uiInRange && !shouldHaveUI)
            ExitUI();

        // — Interact para lore
        if (uiInRange
            && gameObject.layer == loreLayer
            && Input.GetKeyDown(interactKey))
        {
            ToggleLore();
        }
    }

    void EnterUI()
    {
        uiInRange = true;
        worldPrompt?.SetActive(true);
        InteractuableUI?.SetActive(true);
    }

    void ExitUI()
    {
        uiInRange = false;
        worldPrompt?.SetActive(false);
        InteractuableUI?.SetActive(false);
        CloseLore();
    }

    void ToggleLore()
    {
        if (lorePanel == null) return;
        loreOpen = !loreOpen;
        lorePanel.SetActive(loreOpen);
        worldPrompt?.SetActive(!loreOpen);
        InteractuableUI?.SetActive(!loreOpen);
    }

    void CloseLore()
    {
        if (!loreOpen) return;
        loreOpen = false;
        lorePanel.SetActive(false);
        worldPrompt?.SetActive(true);
        InteractuableUI?.SetActive(true);
    }
}
