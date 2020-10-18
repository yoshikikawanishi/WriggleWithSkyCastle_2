using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour {

    [SerializeField] private BossEnemy boss;
    [SerializeField] private Image[] stocks_Images;

    private Slider life_Bar;

    private int now_Phase = 1;
    private int max_Stock;
    private int now_Stock;


	// Use this for initialization
	void Start () {
        life_Bar = GetComponentInChildren<Slider>();
        max_Stock = boss.life.Count;
        now_Stock = boss.life.Count;
        Display_Stock_Image();
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
        //ストックの変更
        if(now_Stock != max_Stock - boss.Get_Now_Phase() + 1) {
            now_Stock = max_Stock - boss.Get_Now_Phase() + 1;
            Display_Stock_Image();
        }
        if(boss.state == BossEnemy.State.cleared) {
            Delete_Stock_Image();
            this.enabled = false;
        }
    }

    private void Display_Stock_Image() {
        foreach(var image in stocks_Images) { image.gameObject.SetActive(false); }
        if(now_Stock == 0) {

        }
        else if(now_Stock == 1) {
            stocks_Images[1].gameObject.SetActive(true);
        }
        else if(now_Stock == 2) {
            stocks_Images[0].gameObject.SetActive(true);
            stocks_Images[2].gameObject.SetActive(true);
        }
        else {
            foreach (var image in stocks_Images) { image.gameObject.SetActive(true); }
        }
    }


    private void Delete_Stock_Image() {
        foreach (var image in stocks_Images) { image.gameObject.SetActive(false); }
    }
}
