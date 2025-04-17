using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPower : MonoBehaviour
{
    [SerializeField] private GameObject _elevator;
    [SerializeField] private Transform _destination;
    [SerializeField] private float speed = 2f;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            PortableBattery battery = other.GetComponent<PortableBattery>();
            if (battery != null && battery.isCharged && !activated)
            {
                Debug.Log(" Energ�a recibida, activando elevador...");
                activated = true;
                
            }
        }
    }
}
