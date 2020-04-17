using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour {

    [SerializeField] private Renderer[] back_Grounds;

    [SerializeField][Range(0.1f, 1)] private float speed_Rate;
    [SerializeField] private float back_Ground_Width;    

    private GameObject main_Camera;
    private float camera_Position;

    private int visible_Back_Ground_Index = 0;


    void Awake() {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Position = main_Camera.transform.position.x;        
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
        for(int i = 0; i < back_Grounds.Length; i++) {
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

            visible_Back_Ground_Index = visible_Back_Ground_Index % back_Grounds.Length;            
        }
    }
}
