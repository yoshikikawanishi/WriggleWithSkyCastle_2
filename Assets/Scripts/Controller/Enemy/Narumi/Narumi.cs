using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narumi : BossEnemy {

    public override void Start_Battle() {
        base.Start_Battle();
        GetComponentInChildren<BGMMelody>().Start_Time_Count();        
    }
}
