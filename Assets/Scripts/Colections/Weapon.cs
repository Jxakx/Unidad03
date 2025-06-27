using UnityEngine;

public class Weapon : MonoBehaviour, IModules
{
    private MeshRenderer _render;
    protected PlayerMovement _player;

    [SerializeField] private GameObject _myBodyFBX; public GameObject MyBodyFBX { get { return _myBodyFBX; } }
    [SerializeField] private Animator _myAnimator; public Animator MyAnimator { get { return _myAnimator; } set { _myAnimator = value; } }

    public enum WeaponState { InInventory, Dropped }
    public WeaponState CurrentState = WeaponState.InInventory;

    private void Awake()
    {
        _render = GetComponent<MeshRenderer>();
    }
    public virtual void Initialized(PlayerMovement player)
    {
       _player = player;
        _render.enabled = false;
    }
    public virtual void PowerElement()
    {
        if (CurrentState != WeaponState.InInventory)
        {
            Debug.Log("No se puede usar el módulo porque no está en el inventario");
            return;
        }

    }

    public void SetDroppedState()
    {
        CurrentState = WeaponState.Dropped;
        if (MyBodyFBX != null) MyBodyFBX.SetActive(false);
    }

    public void SetInventoryState()
    {
        CurrentState = WeaponState.InInventory;
        // No activar MyBodyFBX aquí - se activará al seleccionar
    }
    public virtual void ResetWeaponState()
    {
        // Método para limpiar estado cuando se roba el arma
        if (MyBodyFBX != null) MyBodyFBX.SetActive(false);
    }
}
