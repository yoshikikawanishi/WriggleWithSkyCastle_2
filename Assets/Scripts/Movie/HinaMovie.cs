using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinaMovie : MonoBehaviour {

    [SerializeField] private CollectionBox collection_Box;


	public void Start_Hina_Movie() {
        StartCoroutine("Hina_Movie_Cor");
    }

    private IEnumerator Hina_Movie_Cor() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        MoveTwoPoints _move = GetComponent<MoveTwoPoints>();
        MessageDisplay _message = GetComponent<MessageDisplay>();
        Vector3 default_Pos = transform.position;

        //操作無効化
        while (!player_Controller.Get_Is_Playable()) {            
            yield return null;
        }
        player_Controller.Set_Is_Playable(false);
        player_Controller.Change_Animation("IdleBool");
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player_Controller.To_Disable_Ride_Beetle();
        PauseManager.Instance.Set_Is_Pausable(false);

        //雛が上から降りてくる
        transform.position = new Vector3(player.transform.position.x + 64f, 250f);
        _move.Start_Move(new Vector3(transform.position.x, 80f));
        yield return new WaitUntil(_move.End_Move);

        //会話開始
        _message.Start_Display("HinaText", 4, 4);
        yield return new WaitUntil(_message.End_Message);

        //収集アイテムを落とす
        if (!CollectionManager.Instance.Is_Collected("Hina")) {
            collection_Box.gameObject.SetActive(true);
        }

        //元の位置に戻る
        _move.Start_Move(new Vector3(transform.position.x, 250f));
        yield return new WaitUntil(_move.End_Move);
        transform.position = default_Pos;

        //操作有効か
        player_Controller.Set_Is_Playable(true);
        player_Controller.To_Enable_Ride_Beetle();
        PauseManager.Instance.Set_Is_Pausable(true);        
    }
}
