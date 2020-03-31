using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一定間隔で毛玉ザコ敵を生成する
public class HinaDisaster : MonoBehaviour {

    //カメラ
    private GameObject main_Camera;
    private Vector3 old_Camera_Pos;
    //毛玉ザコ敵生成用
    private ShootSystem[] _shoots = new ShootSystem[3];
    //カメラが動けばカウントを増やす
    private int camera_Move_Count = 0;
    private readonly int CAMERA_COUNT_SPAN = 250;
    //生成した回数
    private int generate_Count = 0;
    //生成位置の乱数範囲
    private readonly Vector2 GEN_POS_RANGE = new Vector2(64f, 64f);


	// Use this for initialization
	void Start () {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        _shoots = GetComponentsInChildren<ShootSystem>();
        //はじめは動かなくする
        this.enabled = false;
	}


    private void FixedUpdate() {
        if (main_Camera == null)
            return;
        //カメラが移動したときカウントする
        if (!Mathf.Approximately(main_Camera.transform.position.x, old_Camera_Pos.x)) {
            old_Camera_Pos = main_Camera.transform.position;
            camera_Move_Count++;
        }
        //生成
        if(camera_Move_Count > CAMERA_COUNT_SPAN) {
            camera_Move_Count = 0;
            generate_Count++;
            Generate();
        }
    }



    //生成開始
    public void Start_Generate() {
        camera_Move_Count = CAMERA_COUNT_SPAN - 1;
        this.enabled = true;
    }

    //生成終了
    public void Stop_Generate() {
        this.enabled = false;
    }


    //生成
    public void Generate() {
        _shoots[generate_Count % 3].offset = Random_Vector2(-GEN_POS_RANGE, GEN_POS_RANGE);
        _shoots[generate_Count % 3].Shoot();
    }

    //Vector2のランダム
    private Vector2 Random_Vector2(Vector2 left_Bottom, Vector2 right_Top) {
        return new Vector2(Random.Range(left_Bottom.x, right_Top.x), Random.Range(left_Bottom.y, right_Top.y));
    }
}
