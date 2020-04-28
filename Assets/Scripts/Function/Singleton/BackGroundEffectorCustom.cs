using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//タイルマップに使う用
public class BackGroundEffectorCustom : BackGroundEffector {

    [SerializeField] private List<Tilemap> back_Grounds;    

    new void Awake() {
        if(back_Grounds.Count == 0) {
            Debug.Log("Set_BackGround_BackGroundEffecter");
            return;
        }        
    }
    

    //背景の色遷移
    protected override IEnumerator Change_Color_Cor(Color next_Color, float change_Speed_Rate) {
        float rate = 0;
        Color difference = next_Color - back_Grounds[0].color;
        Color delta_Color = difference * change_Speed_Rate;
        while (rate < 1) {
            rate += change_Speed_Rate;
            for (int i = 0; i < back_Grounds.Count; i++) {
                back_Grounds[i].color += delta_Color;
            }
            yield return null;
        }
    }

}
