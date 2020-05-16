using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassGround : MonoBehaviour {

    private List<string> player_Attack_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
    };
    private List<string> player_Bullet_Tags = new List<string> {
        "PlayerBulletTag",
    };

    private Tilemap _tilemap;

    private PlayerAttackCollision player_Attack_Collision;
    private PlayerKickCollision player_Kick_Collision;

    private GameObject leaf_Effect_Prefab;  //消滅時のエフェクト
    private TileBase grass_Tile_Top;        //最上部のタイル
    private TileBase grass_Tile_Bottom;     //最下部のタイル

    [SerializeField] private Vector2Int CELL_SIZE = new Vector2Int(32, 32);


	// Use this for initialization
	void Start () {
        //取得
        _tilemap = GetComponent<Tilemap>();
        player_Attack_Collision = GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerAttackCollision>();
        player_Kick_Collision = GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerKickCollision>();
        grass_Tile_Top =    Resources.Load("GrassGroundTop") as TileBase;
        grass_Tile_Bottom = Resources.Load("GrassGroundBottom") as TileBase;
        //エフェクトのオブジェクトプール
        leaf_Effect_Prefab = Resources.Load("Effect/LeafEffect") as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(leaf_Effect_Prefab, 4);
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (player_Attack_Collision == null)
            return;

        //近接攻撃を受けた時
        foreach(string tag in player_Attack_Tags) {
            if(collision.tag == tag) {
                //攻撃の範囲を取得
                Vector2 left_Bottom = player_Attack_Collision.Get_Collision_Range()[0];
                Vector2 right_Top = player_Attack_Collision.Get_Collision_Range()[1];
                //範囲内のタイルを消す
                Search_Tile_And_Delete(left_Bottom, right_Top);
                return;
            }
        }  
        //ショットが当たったとき
        foreach(string tag in player_Bullet_Tags) {
            if(collision.tag == tag) {
                //攻撃の範囲を取得
                Vector2 left_Bottom = collision.transform.position + new Vector3(-6f, -16f);
                Vector2 right_Top = collision.transform.position + new Vector3(16f, 16f);
                //範囲内のタイルを消す
                Search_Tile_And_Delete(left_Bottom, right_Top);
                return;
            }
        }
        //キックが当たったとき
        if(collision.tag == "PlayerKickTag") {
            //攻撃の範囲を取得
            Vector2 left_Bottom = player_Kick_Collision.Get_Collision_Range()[0];
            Vector2 right_Top = player_Kick_Collision.Get_Collision_Range()[1];
            //範囲内のタイルを消す
            Search_Tile_And_Delete(left_Bottom, right_Top);
            return;
        }
        
    }


    //引数範囲内のタイルを消す
    private void Search_Tile_And_Delete(Vector2 left_Bottom, Vector2 right_Top) {        

        //範囲内に含まれる一番左下のセルの番号        
        /*
                ...｜   ｜   ｜   ｜.....
           Pos    -64  -32    0   32
           Index     -2   -1    0    1
         */
        Vector2Int left_Bottom_Cell = new Vector2Int(
            (int)(left_Bottom.x / CELL_SIZE.x),
            (int)(left_Bottom.y / CELL_SIZE.y)
            );
        if (left_Bottom.x < 0)
            left_Bottom_Cell += new Vector2Int(-1, 0);
        if (left_Bottom.y < 0)
            left_Bottom_Cell += new Vector2Int(0, -1);
        //範囲内に含まれる一番右上のセルの番号
        Vector2Int right_Top_Cell = new Vector2Int(
            (int)(right_Top.x / CELL_SIZE.x),
            (int)(right_Top.y / CELL_SIZE.y)
            );
        if (right_Top.x < 0)
            right_Top_Cell += new Vector2Int(-1, 0);
        if (right_Top.y < 0)
            right_Top_Cell += new Vector2Int(0, -1);        

        //範囲内のタイルを消す
        TileBase tile_tmp;
        for(int x = left_Bottom_Cell.x; x <= right_Top_Cell.x; x++) {
            for (int y = left_Bottom_Cell.y; y <= right_Top_Cell.y; y++) {

                tile_tmp = _tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile_tmp == null)
                    continue;

                _tilemap.SetTile(new Vector3Int(x, y, 0), null);                            //消す
                Play_Delete_Effect(_tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)));   //エフェクト出す
            }
        }

        //最上部と最下部の入れ替え
        Swap_Surface_Tile(left_Bottom_Cell, right_Top_Cell);
    }    


    //消滅エフェクト
    private void Play_Delete_Effect(Vector3 pos) {
        var effect = ObjectPoolManager.Instance.Get_Pool(leaf_Effect_Prefab).GetObject();
        effect.transform.position = pos;
        ObjectPoolManager.Instance.Set_Inactive(effect, 2.5f);
    }


    //消した後表面になるタイルを入れ替える
    //引数は消すタイルの範囲
    private void Swap_Surface_Tile(Vector2Int left_Bottom_Cell, Vector2Int right_Top_Cell) {
        //最下部の入れ替え、消すタイルの一個上にタイルがあれば入れ替え
        int bottom_Height = right_Top_Cell.y + 1;
        for(int i = left_Bottom_Cell.x; i <= right_Top_Cell.x; i++) {
            if(_tilemap.HasTile(new Vector3Int(i, bottom_Height, 0))) {                     //タイルがあれば
                if (!_tilemap.HasTile(new Vector3Int(i, bottom_Height + 1, 0))) /*上のタイルが最上部のタイルの場合入れ替えない*/
                    continue;
               _tilemap.SetTile(new Vector3Int(i, bottom_Height, 0), grass_Tile_Bottom);    //入れ替え
            }
        }

        //最上部の入れ替え、消すタイルの一個下にタイルがあれば入れ替え
        int top_Height = left_Bottom_Cell.y - 1;
        for(int i = left_Bottom_Cell.x; i <= right_Top_Cell.x; i++) {
            if (_tilemap.HasTile(new Vector3Int(i, top_Height, 0))) {
                _tilemap.SetTile(new Vector3Int(i, top_Height, 0), grass_Tile_Top);
            }
        }
    }
}
