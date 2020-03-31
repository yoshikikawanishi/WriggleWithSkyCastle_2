using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseAndConvergePlayer : MonoBehaviour {

    
    [SerializeField] private GameObject use_Object_Prefab;
    [Space]
    //拡散、収束のパラメータ
    [SerializeField] private float spread_Speed = 120f;
    [SerializeField] private float start_Converge_Time = 0.3f;
    [SerializeField] private float converge_Speed = 6f;
    [SerializeField] private float finish_Converge_Range = 16f;

    private ObjectPoolManager pool_Manager;
    

	// Use this for initialization
	void Awake () {
        //オブジェクトプール
        pool_Manager = ObjectPoolManager.Instance;
        pool_Manager.Create_New_Pool(use_Object_Prefab, 16);
	}


    /// <summary>
    /// シリアライズで代入したオブジェクトを円形に生成、特定の座標に収束
    /// </summary>
    /// <param name="num">オブジェクトの生成数</param>
    /// <param name="start_Pos">オブジェクトの生成位置</param>
    /// <param name="aim_Pos">収束する座標</param>
    /// <param name="is_Abs_Pos">収束する座標がカメラからの相対座標かどうか</param>
    public void Play_Release_And_Converge(int num, Vector2 start_Pos, Vector2 aim_Pos, GameObject parent) {
       
        //生成、発射
        List<GameObject> objects = new List<GameObject>();        
        for(int i = 0; i < num; i++) {

            var obj = pool_Manager.Get_Pool(use_Object_Prefab).GetObject();            
            obj.transform.position = start_Pos;
            obj.transform.SetParent(parent.transform);

            float angle = 2 * Mathf.PI / num * i;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spread_Speed;

            objects.Add(obj);
        }

        //目標座標に収束
        StartCoroutine(Converge_Cor(objects, aim_Pos, parent));
    }


    /// <summary>
    /// 特定の座標に収束させる
    /// </summary>
    /// <param name="objects">収束させるオブジェクトリスト</param>
    /// <param name="aim_Pos">目標座標</param>
    /// <param name="parent">収束させるオブジェクトの親オブジェクト</param>
    /// <returns></returns>
    public IEnumerator Converge_Cor(List<GameObject> objects, Vector2 aim_Pos, GameObject parent) {
        yield return new WaitForSeconds(start_Converge_Time);

        foreach (GameObject obj in objects)
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        
        Vector2 aim_Pos_Abs = aim_Pos;
        List<GameObject> remove_List = new List<GameObject>();

        while (objects.Count > 0) {           
            
            //目標座標
            if(parent != null) {
                aim_Pos_Abs = (Vector2)parent.transform.position + aim_Pos;
            }

            foreach (GameObject obj in objects) {
                //目標座標に吸い寄せる
                Rigidbody2D obj_Rigid = obj.GetComponent<Rigidbody2D>();
                obj.transform.LookAt2D(aim_Pos_Abs, Vector2.right);
                obj_Rigid.velocity += (Vector2)obj.transform.right * converge_Speed;
                float dirVelocity = Mathf.Atan2(obj_Rigid.velocity.y, obj_Rigid.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));

                //収束したら消す
                if (Vector2.Distance(obj.transform.position, aim_Pos_Abs) < finish_Converge_Range) {
                    remove_List.Add(obj);
                }
            }

            //リストから削除
            foreach(GameObject remove_Obj in remove_List) {
                objects.Remove(remove_Obj);                
                remove_Obj.SetActive(false);
            }
            remove_List.Clear();

            yield return new WaitForSeconds(0.016f);

        }
    }


    /// <summary>
    /// 拡散、収束の設定
    /// </summary>
    /// <param name="spread_Speed">広がる速度(120f)</param>
    /// <param name="start_Converge_Time">収束開始時間(0.3f)</param>
    /// <param name="converge_Speed">収束速度(6f)</param>
    /// <param name="finish_Converge_Range">収束完了座標の許容範囲(16f)</param>
    public void Do_Details_Setting(float spread_Speed, float start_Converge_Time, float converge_Speed, float finish_Converge_Range) {
        this.spread_Speed = spread_Speed;
        this.start_Converge_Time = start_Converge_Time;
        this.converge_Speed = converge_Speed;
        this.finish_Converge_Range = finish_Converge_Range;
    }

    
}
