using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {


    private new void Awake() {
        base.Awake();
        //フレームレートの固定化
        Application.targetFrameRate = 60;
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer) {
            Screen.SetResolution(960, 540, false);
        }
    }

    //ミス時の処理
    public void Miss() {
        //TODO:エフェクト
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player.SetActive(false);
        //ステータスの調整
        BeetlePowerManager.Instance.Set_Beetle_Power(0);
        //復活
        PlayerManager.Instance.Reduce_Stock();
        if (PlayerManager.Instance.Get_Stock() == 0 && PlayerPrefs.GetInt("GameOver") == 0) {            
            Game_Over();
        }
        else {
            StartCoroutine("Revive");
        }
    }


    //復活の処理
    public IEnumerator Revive() {
        //取得
        PlayerManager player_Manager = PlayerManager.Instance;

        yield return new WaitForSeconds(1.0f);

        if (!PlayerPrefs.HasKey("SCENE")) 
            DataManager.Instance.Initialize_Player_Data();
        
        //セーブデータの取得
        string scene = PlayerPrefs.GetString("SCENE");
        float pos_X = PlayerPrefs.GetFloat("POS_X");
        float pos_Y = PlayerPrefs.GetFloat("POS_Y");        

        SceneManager.LoadScene(scene);
        yield return null;    

        //自機の調整
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) { Debug.Log("Can't Find Player"); yield break; }        
        player.transform.position = new Vector3(pos_X, pos_Y);

        //ステータスの調整
        if (CollectionManager.Instance.Is_Collected("BigFrog"))
            player_Manager.Set_Life(4);
        else
           player_Manager.Set_Life(3);
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 50);

        //エフェクト
        Play_Revive_Effect(player);
                
    }   


    //ゲームオーバー時の処理
    public void Game_Over() {
        if (SceneManagement.Instance.Has_Visited("Stage2_1Scene")) {
            gameObject.AddComponent<GameOverMovie>().Start_Movie(true);
            PlayerPrefs.SetInt("GameOver", 1);
        }
        else {
            gameObject.AddComponent<GameOverMovie>().Start_Movie(false);
        }
    }  


    //復活時のエフェクト
    private void Play_Revive_Effect(GameObject player) {
        //点滅
        player.GetComponent<PlayerController>().StartCoroutine("Blink", 3.0f);
        //無敵化
        player.GetComponentInChildren<PlayerBodyCollision>().StartCoroutine("Become_Invincible_Cor", 3.1f);
    }

}
