using UnityEngine;

public class BossWeaponRelay : MonoBehaviour
{
    [SerializeField] private BossWeaponHitDespair rightHandWeapon;
    [SerializeField] private BossWeaponHitDespair leftHandWeapon;

    public void EnableRightWeaponCollider()
    {
        if (rightHandWeapon != null)
            rightHandWeapon.EnableWeaponCollider();
    }

    public void DisableRightWeaponCollider()
    {
        if (rightHandWeapon != null)
            rightHandWeapon.DisableWeaponCollider();
    }

    public void EnableLeftWeaponCollider()
    {
        if (leftHandWeapon != null)
            leftHandWeapon.EnableWeaponCollider();
    }

    public void DisableLeftWeaponCollider()
    {
        if (leftHandWeapon != null)
            leftHandWeapon.DisableWeaponCollider();
    }
}
