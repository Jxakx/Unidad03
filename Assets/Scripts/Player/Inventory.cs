using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField] private int _limiteInventory;
    [SerializeField] private List<GameObject> _inventory = new List<GameObject>();
    [SerializeField] private GameObject _element0;
    [SerializeField] public int _weaponSelected; public int WeaponSelected { get { return _weaponSelected; } }
    [SerializeField] public int _lastWeaponSelected; public int LastWeaponSelected { get { return _lastWeaponSelected; } }

    public Inventory(int limiteInventory, GameObject element0)
    {
        _limiteInventory = limiteInventory;
        _element0 = element0;
        AddWeapon(element0);
        _weaponSelected = 0;
    }

    public void AddWeapon(GameObject weapon)
    {
        if (_inventory.Count >= _limiteInventory)
        {
            Debug.Log("Inventario lleno");
        }
        if (!_inventory.Contains(weapon))
        {
            _inventory.Add(weapon);
            SelectWeapon(_inventory.Count - 1);
            _weaponSelected = _inventory.Count - 1;
        } else
        {
            Debug.Log("La arma ya esta en el inventario");
        }
    }

    public void ReAddModule(GameObject module)
    {
        if (!_inventory.Contains(module))
        {
            _inventory.Add(module);
            module.SetActive(false); // Se activará cuando se seleccione
        }
    }

    public bool ContainsModule(GameObject module)
    {
        return _inventory.Contains(module);
    }
    public void RemoveWeapon(GameObject weapon)
    {
        if (weapon == _element0) return;

        int index = _inventory.IndexOf(weapon);
        if (index != -1)
        {
            // Limpiar estado visual
            Weapon weaponComp = weapon.GetComponent<Weapon>();
            if (weaponComp != null)
            {
                weaponComp.ResetWeaponState();
            }

            // Guardar si era el arma seleccionada
            bool wasSelected = (_weaponSelected == index);

            _inventory.RemoveAt(index);

            // Actualizar selección
            if (wasSelected)
            {
                // Si era la seleccionada, cambiar a la primera arma
                _weaponSelected = 0;
                SelectWeapon(_weaponSelected);
            }
            else if (_weaponSelected > index)
            {
                // Ajustar índice si estaba después de la removida
                _weaponSelected--;
            }

            // Forzar actualización de selección
            UpdateWeaponSelection();
        }
    }

    // Nuevo método para actualizar la selección
    public void UpdateWeaponSelection()
    {
        // Verificar que la selección actual sea válida
        if (_weaponSelected >= _inventory.Count)
        {
            _weaponSelected = _inventory.Count - 1;
        }

        // Reactivar solo la selección actual
        for (int i = 0; i < _inventory.Count; i++)
        {
            GameObject weaponObj = _inventory[i];
            Weapon weapon = weaponObj.GetComponent<Weapon>();

            if (weapon != null)
            {
                bool isActive = (i == _weaponSelected);
                weaponObj.SetActive(isActive);

                if (isActive)
                {
                    weapon.MyBodyFBX.SetActive(true);
                }
                else
                {
                    weapon.MyBodyFBX.SetActive(false);
                }
            }
        }
    }

    public GameObject SelectWeapon(int index)
    {
        if (_weaponSelected != 0) 
        {
            _inventory[_weaponSelected].SetActive(false); //Oculto arma anterior
        }

        _inventory[_weaponSelected].GetComponent<Weapon>().MyBodyFBX.SetActive(false); //Oculto el cuerpo anterior
        
        _weaponSelected = index;
        _inventory[_weaponSelected].SetActive(true); //Muestro arma nueva
        _inventory[_weaponSelected].GetComponent<Weapon>().MyBodyFBX.SetActive(true); //Oculto el cuerpo anterior
        return _inventory[_weaponSelected];
    }

    //método para obtener un módulo por índice
    public GameObject GetModuleAtIndex(int index)
    {
        if (index < 0 || index >= _inventory.Count) return null;
        return _inventory[index];
    }
    public Animator MyCurrentAnimator()
    {
        return _inventory[_weaponSelected].GetComponent<Weapon>().MyAnimator;
    }

    public int MyItemsCount()
    {
        return _inventory.Count;
    }
}
