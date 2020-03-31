using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogShrine : Enemy {

    public override void Damaged(int damage, string attacked_Tag) {
        if (attacked_Tag == "PlayerBodyTag" || attacked_Tag == "PlayerTag")
            return;
        base.Damaged(damage, attacked_Tag);
    }

    //消滅時大蝦蟇戦開始
    public override void Vanish() {
        BigFrogMovie.Instance.Start_Battle_Movie();
        base.Vanish();
    }

}
