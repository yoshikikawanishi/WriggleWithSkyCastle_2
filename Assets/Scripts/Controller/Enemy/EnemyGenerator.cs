using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子要素の敵オブジェクトを生成
/// </summary>
public class EnemyGenerator : MonoBehaviour {

    public bool gen_Only_Flying = true;    

    public Transform parent = null;
    public Transform generator_Parent = null;
    public float start_Gen_Distance_From_Camera = 240;    
    public int num = 1;
    public float span = 1.0f;
    public Vector2 position_Noise;

    public bool is_Controlle_Move = false;
    public AnimationCurve x_Move = AnimationCurve.EaseInOut(0, 0, 10.0f, -100);
    public AnimationCurve y_Move = AnimationCurve.EaseInOut(0, 0, 10.0f, -100);
    public bool is_End_And_Delete = true;

    private ObjectPoolManager pool_Manager;
    private ObjectPool pool;
    private GameObject main_Camera;
    private PlayerController player_Controller;

    private bool is_Enable_Generator = true;
    private float distance;


	// Use this for initialization
	void Start () {
        //取得
        pool_Manager = ObjectPoolManager.Instance;
        main_Camera = GameObject.FindWithTag("MainCamera");
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();

        //敵のオブジェクトプール
        pool = pool_Manager.Get_Pool(transform.GetChild(0).gameObject);        
    }
	

	// Update is called once per frame
	void Update () {        
	    if(is_Enable_Generator) {
            if(gen_Only_Flying && !player_Controller.Get_Is_Ride_Beetle()) {
                return;
            }
            distance = transform.position.x - main_Camera.transform.position.x;
            if(Mathf.Abs(distance - start_Gen_Distance_From_Camera) < 8f) {
                is_Enable_Generator = false;
                transform.SetParent(generator_Parent);
                StartCoroutine("Generate_Enemy_Cor");   
            }
        }
	}


    //敵の生成
    private IEnumerator Generate_Enemy_Cor() {
        for(int i = 0; i < num; i++) {
            var enemy = pool.GetObject();
            enemy.transform.position = transform.position;            
            enemy.transform.position += (Vector3)Random_Vector2(-position_Noise, position_Noise);
            enemy.transform.SetParent(parent);            
            if (is_Controlle_Move) 
                StartCoroutine("Enemy_Move_Cor", enemy);

            yield return new WaitForSeconds(span);
        }        
    }


    //敵の移動
    private IEnumerator Enemy_Move_Cor(GameObject enemy) {
        float end_Time = x_Move.keys[x_Move.length - 1].time;
        Vector2 start_Pos = enemy.transform.localPosition;

        for(float t = 0; t < end_Time; t += Time.deltaTime) {
            //消滅時抜ける
            if (!enemy.activeSelf)
                yield break;

            enemy.transform.localPosition = start_Pos + new Vector2(x_Move.Evaluate(t), y_Move.Evaluate(t));
            yield return null;
        }        

        //最後まで移動したら消す
        if(is_End_And_Delete)
            enemy.SetActive(false);
    }


    //敵を生成可能にする
    private void Enealbe_Enemy_Gen() {
        is_Enable_Generator = true;
    }


    /// <summary>
    /// Vector2の乱数
    /// </summary>
    /// <returns></returns>
    private Vector2 Random_Vector2(Vector2 start, Vector2 end) {
        float x = Random.Range(start.x, end.x);
        float y = Random.Range(start.y, end.y);
        return new Vector2(x, y);
    }


}
