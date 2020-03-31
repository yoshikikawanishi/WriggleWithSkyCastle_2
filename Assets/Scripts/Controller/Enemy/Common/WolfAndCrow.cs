using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAndCrow : MonoBehaviour {

    [SerializeField] private bool gen_Crow_With_Wolf = true;
    [SerializeField] private int move_Length = 32;
    [SerializeField] private float move_Speed = 1;

    private GameObject player;
    private int move_Count = 0;
    private int direction = 1;    


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
	}
	
	// Update is called once per frame
	void Update () {        
        //自機が上に来た時
        if (Is_Player_Above_This()) {
            //横移動中止
            this.enabled = false;
            //狼とカラスの生成
            StartCoroutine("Gen_Wolf_And_Crow");
        }
	}


    private void FixedUpdate() {
        //横移動
        Move_Horizon();
    }


    //横移動
    private void Move_Horizon() {
        if (move_Count * move_Speed < move_Length) {            
            transform.position += new Vector3(move_Speed * direction, 0) * Time.timeScale;
            move_Count++;
        }
        else {
            move_Count = 0;
            direction *= -1;
        }
    }


    //自機が上に来た時trueを返す
    private bool Is_Player_Above_This() {
        if (player == null)
            return false;

        if(Mathf.Abs(player.transform.position.x - transform.position.x) < 16f) {
            if(player.transform.position.y < transform.position.y + 64f)
                return true;
        }
        return false;
    }


    //狼とカラスの生成
    private IEnumerator Gen_Wolf_And_Crow() {
        var wolf = transform.GetChild(0).gameObject;
        var crow = transform.GetChild(1).gameObject;

        yield return new WaitForSeconds(0.8f);

        wolf.SetActive(true);
        wolf.transform.SetParent(null);

        GetComponent<Renderer>().enabled = false;

        if (gen_Crow_With_Wolf) {
            yield return new WaitForSeconds(0.3f);
            crow.SetActive(true);
            crow.transform.SetParent(null);
        }

        gameObject.SetActive(false);
    }
}
