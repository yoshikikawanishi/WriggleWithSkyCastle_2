using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiBlockBarrier : MonoBehaviour {

    [SerializeField] private GameObject block_Prefab;
    
    //ブロックのオブジェクトプール
    private ObjectPool block_Pool;

    //回転するブロック
    private List<GameObject> blocks = new List<GameObject>();
    private int num = 0;                        //数
    private float radius = 32f;                 //半径
    private float angle_Velocity_Rad = 0.1f;    //回転速度
    private float center_Angle_Rad = 0;         //0番目のブロックの角度
    private float inter_Angle_Rad;              //ブロック間の角度

	
	void Start () {
        block_Pool = gameObject.AddComponent<ObjectPool>();
        block_Pool.CreatePool(block_Prefab, 10);

        //Create_Barrier(12, 64f, 0.05f, -1);
	}


    void FixedUpdate() {
        Rotate_Blocks();        
    }


    /// <summary>
    /// ブロックバリア生成、本体中心に回転させる
    /// </summary>
    /// <param name="num">ブロック数</param>
    /// <param name="radius">回転の半径</param>
    /// <param name="angle_Velocity_Rad">回転速度</param>
    /// <param name="life_Time">寿命</param>
    public void Create_Barrier(int num, float radius, float angle_Velocity_Rad, float life_Time) {
        //存在しているブロックを消す
        Delete_Barrier();

        //数値設定
        this.num = num;
        this.radius = radius;
        this.angle_Velocity_Rad = angle_Velocity_Rad;
        inter_Angle_Rad = 2 * Mathf.PI / num;
        
        //オブジェクトの生成        
        GameObject obj;
        float angle;
        for (int i = 0; i < num; i++) {
            obj = block_Pool.GetObject();
            obj.transform.SetParent(transform);
            angle = center_Angle_Rad + inter_Angle_Rad * i;
            obj.transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            blocks.Add(obj);
        }

        //寿命
        if(life_Time > 0)
            Invoke("Delete_Barrier", life_Time);
    }


    //回転中のブロックバリアをすべて消す
    public void Delete_Barrier() {
        if (blocks.Count == 0)
            return;

        for(int i = 0; i < blocks.Count; i++) {
            blocks[i].SetActive(false);
        }
        blocks.Clear();
        center_Angle_Rad = 0;
    }


    //ブロックを回転させる(FixedUpdateで呼ぶこと)
    private void Rotate_Blocks() {
        if (blocks.Count == 0) {
            return;
        }
        for (int i = 0; i < num; i++) {
            if (!blocks[i].activeSelf) {
                continue;
            }
            float angle = center_Angle_Rad + inter_Angle_Rad * i;
            blocks[i].transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }
        center_Angle_Rad += angle_Velocity_Rad;
    }
}
