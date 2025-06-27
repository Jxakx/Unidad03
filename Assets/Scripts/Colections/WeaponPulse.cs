using UnityEngine;

public class WeaponPulse : Weapon
{
    [SerializeField] public GameObject _myBulletPrebaf;
    [SerializeField] public Transform _instancePoint;

    public override void Initialized(PlayerMovement player)
    {
        base.Initialized(player);
    }

    public override void PowerElement()
    {
        if (CurrentState != WeaponState.InInventory) return;
        base.PowerElement();
        Instantiate(_myBulletPrebaf, _instancePoint.position, transform.rotation);
        _player.CanWeaponChange = true;
    }

    public override void ResetWeaponState()
    {
        base.ResetWeaponState();
    }
}