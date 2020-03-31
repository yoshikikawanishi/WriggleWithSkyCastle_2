using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyEnemy : Enemy {

    public override void Vanish() {
        var ghost = Instantiate(Resources.Load("Effect/FairyGhost") as GameObject);
        ghost.transform.position = transform.position;
        Destroy(ghost, 1.5f);

        base.Vanish();
    }
}
