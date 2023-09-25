using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Weapon weapon;

    protected Weapon Weapon => weapon;
}
