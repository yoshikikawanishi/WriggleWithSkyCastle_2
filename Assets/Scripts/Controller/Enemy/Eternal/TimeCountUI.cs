using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountUI : MonoBehaviour {

    [SerializeField] private EternalLastAttack eternal_Last_Attack;
    [SerializeField] private Text time_Count_Text;
    private int time_Count_Value = 100;


    void Start() {
        time_Count_Text.text = eternal_Last_Attack.time_Count.ToString();
    }


    void Update() {
        if(time_Count_Value != eternal_Last_Attack.time_Count) {
            time_Count_Value = eternal_Last_Attack.time_Count;
            time_Count_Text.text = time_Count_Value.ToString();
        }
    }
}
