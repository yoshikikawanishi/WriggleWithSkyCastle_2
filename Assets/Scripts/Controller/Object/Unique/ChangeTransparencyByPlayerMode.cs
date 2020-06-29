using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 自機が通常時または飛行時に半透明になり当たり判定を消す(Tilemapオブジェクトにもアタッチできる)
/// </summary>
public class ChangeTransparencyByPlayerMode : MonoBehaviour {

    private enum PlayerState {
        normal,
        fly,
    }
    [SerializeField] PlayerState appear_State;

    //コンポーネント
    private PlayerController player_Controller;
    private SpriteRenderer _sprite;
    private Tilemap _tilemap;

    private LayerMask default_Layer;

	
    // Use this for initialization
	void Start () {
        //取得
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        if ((_sprite = GetComponent<SpriteRenderer>()) == null) {
            _tilemap = GetComponent<Tilemap>();            
        }
        default_Layer = gameObject.layer;        
	}


    private void Update() {
        if(player_Controller == null) 
            return;
        
        //飛行時
        if(player_Controller.Get_Is_Ride_Beetle()) {           
            //具現化
            if (appear_State == PlayerState.fly) {
                Become_Appearance();
            }
            //透明化
            else {
                Become_Transparent();
            }
        }
        //地上時
        else {            
            //具現化
            if (appear_State == PlayerState.normal) {
                Become_Appearance();
            }
            //透明化
            else {
                Become_Transparent();
            }
        }
    }


    //透明になる
    private void Become_Transparent() {
        if (gameObject.layer == default_Layer) {
            if (_sprite == null) {
                _tilemap.color = new Color(1, 1, 1, 0.4f);                
            }
            else {
                _sprite.color = new Color(1, 1, 1, 0.4f);
            }
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
    }

    //具現化する
    private void Become_Appearance() {
        if (gameObject.layer == LayerMask.NameToLayer("InvincibleLayer")) {
            if (_sprite == null) {
                _tilemap.color = new Color(1, 1, 1, 1f);
            }
            else {
                _sprite.color = new Color(1, 1, 1, 1f);
            }
            gameObject.layer = default_Layer;
        }
    }

}
