using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaMovie : SingletonMonoBehaviour<YukaMovie> {

    [SerializeField] private GameObject boss_Battle_Canvas;
    //コンポーネント
    private MessageDisplayCustom _message;

    private GameObject yuka;


	// Use this for initialization
	void Start () {
        //取得
        _message = GetComponent<MessageDisplayCustom>();
        yuka = GameObject.Find("Yuka");       
    }
	

    public void Start_Movie() {
        //クリア済み
        if (PlayerPrefs.GetInt("YukaTutorial") == 2) {
            return;
        }
        StartCoroutine("Movie_Cor");
    }

    private IEnumerator Movie_Cor() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        
        //自機の固定
        player_Controller.Set_Is_Playable(false);
        player_Controller.To_Disable_Ride_Beetle();
        player_Controller.Change_Animation("IdleBool");
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //カメラの固定
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.AddComponent<MoveTwoPoints>().Start_Move(new Vector3(2062f, 0, -10f));
        camera.GetComponent<CameraController>().enabled = false;
        yield return new WaitForSeconds(0.8f);

        //初回のみ会話
        if (!PlayerPrefs.HasKey("YukaTutorial")) {
            _message.Start_Display("YukaText", 1, 10);
            yield return new WaitUntil(_message.End_Message);
        }
        PlayerPrefs.SetInt("YukaTutorial", 1);

        //戦闘開始
        player_Controller.Set_Is_Playable(true);
        player_Controller.To_Enable_Ride_Beetle();
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.4f, 0.4f, 0.4f), 0.1f);
        yuka.GetComponent<Yuka>().Start_Battle();
        if (boss_Battle_Canvas != null)
            boss_Battle_Canvas.SetActive(true);
    }


    //Yukaで呼ばれる
    //クリアムービー
    public void Start_Clear_Movie() {
        //クリアの保存
        PlayerPrefs.SetInt("YukaTutorial", 2);

        StartCoroutine("Clear_Movie_Cor");
    }

    private IEnumerator Clear_Movie_Cor() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        //自機の固定
        player_Controller.Set_Is_Playable(false);
        player_Controller.To_Disable_Ride_Beetle();
        player_Controller.Change_Animation("IdleBool");
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        _message.Start_Display("YukaText", 20, 21);
        yield return new WaitUntil(_message.End_Message);

        //自機の固定外す
        player_Controller.Set_Is_Playable(true);
        player_Controller.To_Enable_Ride_Beetle();
        //カメラの固定を外す
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.GetComponent<CameraController>().enabled = true;
    }
}
