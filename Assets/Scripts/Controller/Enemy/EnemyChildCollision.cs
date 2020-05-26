using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChildCollision : EnemyCollisionDetection {

    [SerializeField] Enemy enemy_Class;

    void Awake() {
        _enemy = enemy_Class;
    }
}
