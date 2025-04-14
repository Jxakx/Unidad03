using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform _target; // Arrastra el jugador aqu� desde el Inspector

    [Header("Ajustes")]
    [SerializeField] private float _smoothSpeed = 5f; // Suavizado del movimiento
    [SerializeField] private float _xOffset = 2f; // Offset en X para no centrar completamente la c�mara
    [SerializeField] private float _fixedY = 5f; // Altura fija de la c�mara (ajusta seg�n tu escena)
    [SerializeField] private float _fixedZ = -10f; // Posici�n Z fija (distancia desde el jugador)

    void LateUpdate()
    {
        if (_target == null) return;

        // Calcula la posici�n deseada (solo sigue en X, mantiene Y y Z fijas)
        Vector3 desiredPosition = new Vector3(
            _target.position.x + _xOffset,
            _fixedY,
            _fixedZ
        );

        // Interpola suavemente hacia la posici�n deseada
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}
