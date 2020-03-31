using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaStockZeroMovie : MonoBehaviour {

	//残機０になって復活した際に始めるムービー
    public void Start_Movie() {
        StartCoroutine("Movie_Cor");
    }

    private IEnumerator Movie_Cor() {
        //取得
        MessageDisplay _message = GetComponent<MessageDisplay>();
        if(_message == null)
            _message = gameObject.AddComponent<MessageDisplay>();
        GameObject player = GameObject.FindWithTag("PlayerTag");        
        
        //自機の操作不可
        if(player != null) {
            player.GetComponent<PlayerController>().Set_Is_Playable(false);
        }
        //ポーズ不可
        PauseManager.Instance.Set_Is_Pausable(false);

        //セリフ開始
        _message.Start_Display("LarvaStockZeroText", 1, 2);
        yield return new WaitUntil(_message.End_Message);

        //自機とポーズの解除
        if(player != null) {
            player.GetComponent<PlayerController>().Set_Is_Playable(true);
        }
        PauseManager.Instance.Set_Is_Pausable(true);
    }
}
