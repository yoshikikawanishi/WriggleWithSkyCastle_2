using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnShoot : MonoBehaviour {

    [SerializeField] private ShootSystem short_Curve_Laser;

    public void Shoot_Short_Curve_Laser() {
        short_Curve_Laser.Shoot();
    }
}
