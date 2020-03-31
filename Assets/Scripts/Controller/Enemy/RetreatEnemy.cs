using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatEnemy : MonoBehaviour {

    [SerializeField] private AnimationCurve x_Move = AnimationCurve.EaseInOut(0, 0, 3.0f, 0);
    [SerializeField] private AnimationCurve y_Move = AnimationCurve.EaseInOut(0, 0, 3.0f, 100);

    private PlayerController player_Controller;
    private bool is_Active = true;


    private void OnEnable() {
        is_Active = true;
    }

    // Use this for initialization
    void Start () {
        //取得
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (player_Controller.Get_Is_Ride_Beetle()) {
            return;
        }
        if (is_Active) {
            StartCoroutine("Retreate_Cor");
            is_Active = false;
        }	
	}


    //退場する
    private IEnumerator Retreate_Cor() {
        float end_Time = x_Move.keys[x_Move.length - 1].time;

        yield return new WaitForSeconds(1.0f);

        Vector2 start_Pos = transform.localPosition;

        for (float t = 0; t < end_Time; t += Time.deltaTime) {       
            transform.localPosition = start_Pos + new Vector2(x_Move.Evaluate(t), y_Move.Evaluate(t));
            yield return null;
        }        

        //最後まで移動したら消す        
        gameObject.SetActive(false);
    }
}
