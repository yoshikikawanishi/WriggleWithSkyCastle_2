using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetlePowerManager : SingletonMonoBehaviour<BeetlePowerManager> {


    public int beetle_Power = 0;
    private int MAX = 100;

    private float increase_Time = 0;
    private float decrease_Time = 0;


    //Getter
    public int Get_Beetle_Power() {
        return beetle_Power;
    }

    //Setter
    public void Set_Beetle_Power(int beetle_Power) {
        if (beetle_Power < 0) {
            beetle_Power = 0;
        }
        else if (beetle_Power > MAX) {
            beetle_Power = MAX;
        }
        this.beetle_Power = beetle_Power;
    }

    
    //グレイズ時の増加
    public void Increase_In_Update(float rate) {
        if(increase_Time < 1 / rate) {
            increase_Time += Time.deltaTime;
        }
        else if(beetle_Power < MAX) {
            increase_Time = 0;
            beetle_Power++;            
        }
    }


    //飛行時の減少
    public void Decrease_In_Update(float rate) {
        if(decrease_Time < 1f / rate) {
            decrease_Time += Time.deltaTime;
        }
        else if(beetle_Power > 0) {
            decrease_Time = 0;
            beetle_Power--;            
        }
    }


    //近接攻撃時の増加
    public IEnumerator Increase_Cor(int amount) {
        for(int i = 0; i < amount; i++) {
            beetle_Power++;
            if(beetle_Power >= MAX) {
                beetle_Power = MAX;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    //チャージショット時の減少
    public void Decrease(int amount) {
        beetle_Power -= amount;
    }
    

}
