using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour {

    [SerializeField] private BossEnemy boss;

    private Slider life_Bar;

    private int now_Phase = 1;


	// Use this for initialization
	void Start () {
        life_Bar = GetComponentInChildren<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        //フェーズの取得
        if (now_Phase != boss.Get_Now_Phase()) {
            now_Phase = boss.Get_Now_Phase();
        }
        //最大値の変更
        if (life_Bar.maxValue != boss.Get_Default_Life(now_Phase)) {
            life_Bar.maxValue = boss.Get_Default_Life(now_Phase);
        }
        //値の変更
        if (life_Bar.value != boss.life[now_Phase - 1]) {
            life_Bar.value = boss.life[now_Phase - 1];
        }
    }
}
