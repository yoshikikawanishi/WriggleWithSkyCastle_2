using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChildCollision : BossCollisionDetection {

    [SerializeField] private BossEnemy Boss_Enemy_Class;

    private void Awake() {
        _boss_Enemy = Boss_Enemy_Class;
    }
}
