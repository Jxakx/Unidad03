using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickup : MonoBehaviour
{
    private GameObject _moduleReference; // Referencia al módulo real
    [SerializeField] private float _rotationSpeed = 50f;

    public void SetModule(GameObject module)
    {
        _moduleReference = module;  // Guarda la referencia al módulo robado

        // Copia la apariencia del módulo real
        CopyVisualsFromModule(module);
    }

    private void CopyVisualsFromModule(GameObject module)
    {
        // Copiar mesh
        MeshFilter moduleMesh = module.GetComponent<MeshFilter>();
        if (moduleMesh != null)
        {
            MeshFilter pickupMesh = GetComponent<MeshFilter>();
            if (pickupMesh == null) pickupMesh = gameObject.AddComponent<MeshFilter>();
            pickupMesh.mesh = moduleMesh.sharedMesh;
        }

        // Copiar materiales
        MeshRenderer moduleRenderer = module.GetComponent<MeshRenderer>();
        if (moduleRenderer != null)
        {
            MeshRenderer pickupRenderer = GetComponent<MeshRenderer>();
            if (pickupRenderer == null) pickupRenderer = gameObject.AddComponent<MeshRenderer>();
            pickupRenderer.materials = moduleRenderer.sharedMaterials;
        }

        // Opcional: copiar escala
        transform.localScale = module.transform.lossyScale;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && _moduleReference != null)
            {
                // Añade el módulo original al inventario
                player.AddModule(_moduleReference);
                Destroy(gameObject);
            }
        }
    }
}
