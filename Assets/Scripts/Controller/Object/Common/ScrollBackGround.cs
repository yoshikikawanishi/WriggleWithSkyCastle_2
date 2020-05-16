using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour {

    [SerializeField] private Renderer left;
    [SerializeField] private Renderer center;    
    [SerializeField] private Renderer right;    

    [SerializeField][Range(0.1f, 1)] private float speed_Rate;
    [SerializeField] private float back_Ground_Width;

    private Renderer[] back_Grounds = new Renderer[3];
    private GameObject main_Camera;
    private float camera_Position;
    private Renderer center_Back_Ground;
    private DEQueue<Renderer> dequeue = new DEQueue<Renderer>();

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

        center_Back_Ground = center;
        for(int i = 0; i < 3; i++) {
            dequeue.Add_Last(back_Grounds[i]);
        }        
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
        if (center.isVisible) {
            return;
        }
        //中心背景が見えなくなったとき
        //左に流れた時 : 中心を右のに変更、左端のを中心の右側に配置、順番の並び替え
        if(center.transform.position.x < main_Camera.transform.position.x) {
            center = dequeue.Get_Last();
            dequeue.Get_First().transform.position = center.transform.position
                                                   + new Vector3(back_Ground_Width / 2, 0);
            dequeue.Add_Last(dequeue.Remove_First());
        }
        //右に流れた時 : 中心を左のに変更、右端のを中心の左側に配置、順番の並び替え
        else {
            center = dequeue.Get_First();
            dequeue.Get_Last().transform.position = center.transform.position
                                                  + new Vector3(-back_Ground_Width / 2, 0);
            dequeue.Add_First(dequeue.Remove_Last());
        }
    }
}
