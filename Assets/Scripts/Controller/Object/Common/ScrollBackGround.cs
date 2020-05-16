using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour {

    [SerializeField] private Renderer center;
    [SerializeField] private Renderer left;
    [SerializeField] private Renderer right;    

    [SerializeField][Range(0.1f, 1)] private float speed_Rate;
    [SerializeField] private float back_Ground_Width;

    private Renderer[] back_Grounds = new Renderer[3];
    private GameObject main_Camera;
    private float camera_Position;

    private int visible_Back_Ground_Index = 0;

    //速度差の値保存用
    private float SPEED_RATE;


    void Awake() {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Position = main_Camera.transform.position.x;

        //代入
        back_Grounds[0] = left;
        back_Grounds[1] = center;
        back_Grounds[2] = right;
        SPEED_RATE = speed_Rate;
    }    


    void Start() {
        this.enabled = false;
        Invoke("Set_Enabled", Time.deltaTime);
    }


    void Update () {
        Scroll_Back_Ground();
        Loop_Back_Ground();
	}
    

    void Set_Enabled() {
        this.enabled = true;
    }


    //カメラの移動距離を返す
    private float Camera_Move_Distance() {
        float distance = main_Camera.transform.position.x - camera_Position;
        camera_Position = main_Camera.transform.position.x;
        return distance;
    }


    //背景スクロール
    private void Scroll_Back_Ground() {
        float distance = Camera_Move_Distance();
        if (distance > 16f)
            speed_Rate = 1;
        else
            speed_Rate = SPEED_RATE;

        for (int i = 0; i < back_Grounds.Length; i++) {            
            back_Grounds[i].transform.position += new Vector3(distance * speed_Rate, 0, 0);
        }
    }


    //背景ﾙｰﾌﾟ用
    private void Loop_Back_Ground() {
        if (!back_Grounds[visible_Back_Ground_Index].isVisible) {            
            int next = (visible_Back_Ground_Index + 1) % back_Grounds.Length;
            float loop_Pos;

            if (back_Grounds[visible_Back_Ground_Index].transform.position.x < back_Grounds[next].transform.position.x) {
                loop_Pos = back_Grounds[next].transform.position.x + back_Ground_Width / 2;
            }
            else {
                loop_Pos = back_Grounds[next].transform.position.x - back_Ground_Width / 2;
            }

            back_Grounds[visible_Back_Ground_Index].transform.position = new Vector3(loop_Pos, 0, 0);

            visible_Back_Ground_Index = next;            
        }
    }
}
