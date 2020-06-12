using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを動かす、背景を動かす、地面・背景をループさせる
/// </summary>
public class BossBattleScroll : MonoBehaviour {

    private enum Direction {
        right,
        left,
    }
    [SerializeField] private Direction direction;
    [Space]
    [SerializeField] private GameObject first_Ground;
    [SerializeField] private GameObject second_Ground;
    [SerializeField] private float ground_Width;
    [Space]
    [SerializeField] private GameObject first_Background;
    [SerializeField] private GameObject second_Background;
    [SerializeField] private float back_Ground_Width;
    [Space]
    [SerializeField] private float scroll_Speed = 1.0f;
    [SerializeField] private float background_Speed = 0.5f;

    private GameObject main_Camera;
    private float CAMERA_WIDTH = 240f;

    private bool is_Scroll = false;
    private int direction_Int;


	void Start () {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        //初期設定
        direction_Int = direction == Direction.right ? 1 : -1;
	}
	
	
	void FixedUpdate () {
        if (!is_Scroll) 
            return;

        Scroll_Camera();
        Scroll_Background();
        Loop_To_Camera(first_Ground, second_Ground, 512f);
        Loop_To_Camera(first_Background, second_Background, 512f);
	}


    //カメラの移動
    private void Scroll_Camera() {
        main_Camera.transform.position += new Vector3(scroll_Speed * direction_Int, 0, 0);
    }


    //背景の移動
    private void Scroll_Background() {
        first_Background.transform.position += new Vector3(background_Speed * direction_Int, 0, 0);
        second_Background.transform.position += new Vector3(background_Speed * direction_Int, 0, 0);
    }


    //カメラから出たらループ
    private void Loop_To_Camera(GameObject first_Obj, GameObject second_Obj, float object_Width) {
        float diff = main_Camera.transform.position.x - first_Obj.transform.position.x;
        if(diff > CAMERA_WIDTH + object_Width / 2) {
            first_Obj.transform.position = second_Obj.transform.position + new Vector3(object_Width * direction_Int, 0, 0);
        }
        diff = main_Camera.transform.position.x - second_Obj.transform.position.x;
        if (diff > CAMERA_WIDTH + object_Width / 2) {
            second_Obj.transform.position = first_Obj.transform.position + new Vector3(object_Width * direction_Int, 0, 0);
        }
    }


    //スクロールの開始
    public void Start_Scroll() {
        is_Scroll = true;
        CameraController camera_Controller = main_Camera.GetComponent<CameraController>();
        if (camera_Controller != null)
            camera_Controller.enabled = false;
    }


    //スクロール終了
    public void Stop_Scroll() {
        is_Scroll = false;
    }
}
