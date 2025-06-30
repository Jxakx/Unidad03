using UnityEngine;
using System.Collections;

public class Show3DOnToggle : MonoBehaviour
{
    [Header("Referencia al Objeto 3D (en Canvas)")]
    [Tooltip("Arrastra aquí tu GameObject 3D que está en el canvas")]
    public GameObject ui3DObject;

    // Guardamos posición y rotación local originales:
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    void Awake()
    {
        if (ui3DObject != null)
        {
            // Al inicio guardamos su estado “inactivo” y posicionamiento
            originalLocalPos = ui3DObject.transform.localPosition;
            originalLocalRot = ui3DObject.transform.localRotation;
            ui3DObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        // Cuando ESTE GameObject se activa, activamos el 3D y lanzamos la animación
        if (ui3DObject != null)
        {
            ui3DObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        // Cuando ESTE GameObject se desactiva, escondemos el 3D
        if (ui3DObject != null)
        {
            ui3DObject.SetActive(false);
        }
    }
}
