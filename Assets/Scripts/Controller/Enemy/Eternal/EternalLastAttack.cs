using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EternalLastAttack : MonoBehaviour {

    [SerializeField] private MovieSystem last_Battle_Movie;
    [SerializeField] private GameObject time_Count_UI;
    [SerializeField] private Text stock_UI_Text;
    [SerializeField] private Text stock_UI_Text_Flying;
    [SerializeField] private GameObject mini_Larva;

    private Eternal _eternal;
    private EternalLastShoot _last_Shoot;
    private SEManager _se;
    private EternalEffect _effect;

    public int time_Count = 100;


    void Start() {
        _eternal = GetComponent<Eternal>();
        _last_Shoot = GetComponentInChildren<EternalLastShoot>();
        _se = GetComponentInChildren<SEManager>();
        _effect = GetComponentInChildren<EternalEffect>();
    }


    #region Movie
    //戦闘開始前ムービー、EternalAttackで呼ぶ
    public void Start_Last_Battle_Movie() {
        last_Battle_Movie.Start_Movie();
    }

    //タイムカウントUI表示、last_Battle_Movieで呼ぶ、
    public void Enable_Time_Count_UI() {
        time_Count_UI.SetActive(true);
    }

    //ちびラルバの生成、last_Battle_Movieで呼ぶ
    public void Enable_Mini_Larva() {
        mini_Larva.SetActive(true);
    }

    //タイムカウントを自機のストックに応じて増減、last_Battle_Movieで呼ぶ
    public void Increase_Time_Count() {
        StartCoroutine("Increase_Time_Count_Cor");
    }

    private IEnumerator Increase_Time_Count_Cor() {
        int stock = PlayerManager.Instance.Get_Stock();
        if(stock >= 0) {
            for(int i = 0; i < stock; i++) {
                time_Count--;
                stock_UI_Text.text = "× " + (stock - i - 1);
                stock_UI_Text_Flying.text = "× " + (stock - i - 1);
                _se.Play("TimeCount");
                yield return new WaitForSeconds(0.1f);
            }
        }
        else {
            for(int i = 0; i < -stock; i++) {
                time_Count += 5;
                stock_UI_Text.text = "× " + (stock + i + 1);
                stock_UI_Text_Flying.text = "× " + (stock + i + 1);
                _se.Play("TimeCount");
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }

    //戦闘開始、last_Battle_Movieで呼ぶ
    public void Start_Battle() {
        StartCoroutine("Last_Attack_Cor");
        StartCoroutine("Time_Count_Cor");
    }
    #endregion


    private IEnumerator Time_Count_Cor() {
        while(time_Count > 0) {
            yield return new WaitForSeconds(1.0f);
            time_Count--;
            if(time_Count < 6)
                _se.Play("TimeCount");
        }
    }


    private IEnumerator Last_Attack_Cor() {
        float max_Time_Count = time_Count;
        float[] time_Border = { time_Count * 0.8f, time_Count * 0.55f, time_Count * 0.3f, 0 };

        _effect.Play_Roaring_Effect();        
        yield return new WaitForSeconds(2.0f);

        _last_Shoot.Start_Wing_Shoot();
        _last_Shoot.Start_First_Shoot();
        _effect.Play_Burst_Effect(Color.green);

        while(time_Count > time_Border[0]) { yield return null; }

        _last_Shoot.Start_Second_Shoot();
        _effect.Play_Burst_Effect(Color.yellow);

        while (time_Count > time_Border[1]) { yield return null; }

        _last_Shoot.Start_Third_Shoot();
        _effect.Play_Burst_Effect(Color.red);

        while (time_Count > time_Border[2]) { yield return null; }

        _last_Shoot.Start_Forth_Shoot();
        _effect.Play_Burst_Effect(new Color(1f, 0.2f, 1f));

        while(time_Count > 0) { yield return null; }

        _last_Shoot.Stop_Shoot();
        _eternal.Damaged(1000, "");

    }

    
}
