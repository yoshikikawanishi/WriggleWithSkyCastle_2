using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MBLDefine;

public class GuideWindowDisplayer : MonoBehaviour {


    private GameObject guide_Window;
	

    /// <summary>
    /// ガイドウィンドウを開く
    /// </summary>
    /// <param name="canvas_Path">Resourcesからガイドウィンドウ用キャンバスを生成するのでそのパス</param>
    public void Open_Window(string canvas_Path) {
        StartCoroutine(Open_Window_Cor(canvas_Path));
    }


    private IEnumerator Open_Window_Cor(string canvas_Path) {
        var prefab = Resources.Load(canvas_Path) as GameObject;
        if (prefab == null) {
            Debug.Log(canvas_Path + " is Not Correct Path");
            yield break;
        }

        //自機の操作無効化
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<PlayerController>().Set_Is_Playable(false);
        }
        //ポーズ無効
        PauseManager.Instance.Set_Is_Pausable(false);

        Time.timeScale = 0;

        //ウィンドウ表示
        guide_Window = Instantiate(prefab);
        guide_Window.SetActive(true);

        //ウィンドウを閉じる
        for (float t = 0; t < 1.5f; t += 0.016f) { yield return null; }
        yield return new WaitUntil(Wait_Input);
        Close_Window();
    }


    public bool Wait_Input() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            return true;
        }
        return false;
    }


    //ガイドウィンドウを閉じる
    private void Close_Window() {
        //取得
        PlayerController player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();        

        //操作有効化
        player_Controller.Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);
        Time.timeScale = 1;

        //ウィンドウを消す
        guide_Window.GetComponentInChildren<Animator>().SetTrigger("CloseTrigger");        
        Destroy(guide_Window, 0.7f);
    }
    
}
