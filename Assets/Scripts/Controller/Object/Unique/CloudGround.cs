using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(TilemapCollider2D))]
public class CloudGround : MonoBehaviour {

    private const string PLAYER_BODY_TAG = "PlayerTag";
    private const int CELL_SIZE = 32;
    private const float delete_Detect_Range = 10f;

    //コンポーネント
    private PlayerController player_Controller;
    private Tilemap _tilemap;

    //消滅時のエフェクト
    [SerializeField] private GameObject delete_Effect;

    //表面のタイル
    [SerializeField] private TileBase left_Top_Tile;
    [SerializeField] private TileBase center_Top_Tile;
    [SerializeField] private TileBase right_Top_Tile;
    [Space]
    [SerializeField] private TileBase left_Tile;
    [SerializeField] private TileBase right_Tile;
    [Space]
    [SerializeField] private TileBase left_Bottom_Tile;
    [SerializeField] private TileBase center_Bottom_Tile;
    [SerializeField] private TileBase right_Bottom_Tile;
    [Space]
    [SerializeField] private TileBase isolated_Tile;
    [Space]
    [SerializeField] private TileBase vertical_Straight_Top_Tile;
    [SerializeField] private TileBase vertical_Straight_Center_Tile;
    [SerializeField] private TileBase vertical_Straight_Bottom_Tile;

    //表面のタイルがどの方向への表面化を設定する用
    private class SurfaceTile {
        public TileBase tile;
        public bool[] is_Exist = new bool[4];   //上下左右にタイルが存在するかどうか ( 0:上 / 1:右 / 2:下 / 3:左 )      
        public SurfaceTile(TileBase tile, bool up, bool right, bool down, bool left) {
            this.tile = tile;
            is_Exist[0] = up;
            is_Exist[1] = right;
            is_Exist[2] = down;
            is_Exist[3] = left;
        }
    }
    private List<SurfaceTile> surface_Tiles;

    


    void Start () {
        //コンポーネント取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            Destroy(this);
        player_Controller = player.GetComponent<PlayerController>();
        _tilemap = GetComponent<Tilemap>();

        //エフェクトのオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(delete_Effect, 2);

        //表面タイルの初期設定
        Setting_Surface_Tile();
	}


    void OnCollisionEnter2D(Collision2D collision) {
        if (!player_Controller.Get_Is_Ride_Beetle()) {
            return;
        }

        if(collision.gameObject.tag == PLAYER_BODY_TAG) {            
            foreach(ContactPoint2D contact in collision.contacts) {                
                Delete_Tile(contact.point);
            }
        }    
    }


    //引数座標周辺のタイルを消す
    private void Delete_Tile(Vector2 pos) {
        //周辺座標を羅列
        Vector3Int[] points = new Vector3Int[5];
        points[0] = Get_Tilemap_Index(pos);
        points[1] = Get_Tilemap_Index(pos + new Vector2(1f, 1f) * delete_Detect_Range);
        points[2] = Get_Tilemap_Index(pos + new Vector2(1f, -1f) * delete_Detect_Range);
        points[3] = Get_Tilemap_Index(pos + new Vector2(-1f, 1f) * delete_Detect_Range);
        points[4] = Get_Tilemap_Index(pos + new Vector2(-1f, -1f) * delete_Detect_Range);

        List<Vector3Int> deleted_Point_List = new List<Vector3Int>();

        //消す
        for (int i = 0; i < 5; i++) {        
            if (_tilemap.GetTile(points[i]) == null)
                continue;
            _tilemap.SetTile(points[i], null);
            deleted_Point_List.Add(points[i]);
            Play_Delete_Effect(points[i]);
        }

        //表面タイルの入れ替え
        foreach(Vector3Int p in deleted_Point_List) {
            Swap_Surface_Tile_Around_Deleted_Tile(p);
        }        
    }


    //座標からそこのタイルマップ上のインデックスを取得
    private Vector3Int Get_Tilemap_Index(Vector2 pos) {        
        Vector3Int point = new Vector3Int((int)(pos.x / CELL_SIZE), (int)(pos.y / CELL_SIZE), 0);
        if(pos.x < 0) {
            point += new Vector3Int(-1, 0, 0);
        }
        if(pos.y < 0) {
            point += new Vector3Int(0, -1, 0);
        }        
        return point;
    }


    //タイルマップのインデックスから座標を取得
    private Vector2 Get_Position(Vector3Int point) {
        Vector2 position = new Vector2((point.x + 0.5f) * CELL_SIZE, (point.y + 0.5f) * CELL_SIZE);
        return position;
    }


    //消滅時のエフェクト
    private void Play_Delete_Effect(Vector3Int point) {
        var effect = ObjectPoolManager.Instance.Get_Pool(delete_Effect).GetObject();
        effect.transform.position = Get_Position(point);
    }


    // ========================== 表面入れ替え関係 ====================================

    //表面タイルの初期設定, Startで呼ぶこと
    //それぞれのタイルがどの方向への表面かをセットする
    private void Setting_Surface_Tile() {
        surface_Tiles = new List<SurfaceTile>{
            new SurfaceTile(left_Top_Tile,      false, true, true, false),
            new SurfaceTile(center_Top_Tile,    false, true, true, true),
            new SurfaceTile(right_Top_Tile,     false, false, true, true),

            new SurfaceTile(right_Tile,     true, false, true, true),
            new SurfaceTile(left_Tile,      true, true, true, false),

            new SurfaceTile(left_Bottom_Tile,   true, true, false, false),
            new SurfaceTile(center_Bottom_Tile, true, true, false, true),
            new SurfaceTile(right_Bottom_Tile,  true, false, false, true),

            new SurfaceTile(isolated_Tile,  false, false, false, false),

            new SurfaceTile(vertical_Straight_Top_Tile,     false, false, true, false),
            new SurfaceTile(vertical_Straight_Center_Tile,  true, false, true, false),
            new SurfaceTile(vertical_Straight_Bottom_Tile,  true, false, false, false),
        };
    }


    //タイルの状況に合わせて表面のタイル変更
    private void Swap_Surface_Tile_Around_Deleted_Tile(Vector3Int deleted_Point) {
        if (_tilemap.GetTile(deleted_Point) != null)
            return;
        Vector3Int[] surface_Points = new Vector3Int[4];
        surface_Points[0] = deleted_Point + new Vector3Int(0, 1, 0);   //上部
        surface_Points[1] = deleted_Point + new Vector3Int(1, 0, 0);   //右部
        surface_Points[2] = deleted_Point + new Vector3Int(0, -1, 0);   //下部
        surface_Points[3] = deleted_Point + new Vector3Int(-1, 0, 0);   //左部

        for(int i = 0; i < 4; i++) {
            if (_tilemap.GetTile(surface_Points[i]) == null)
                continue;
            Swap_Surface_Tile(surface_Points[i]);
        }
    }


    //表面タイルを実際に変更する関数
    private void Swap_Surface_Tile(Vector3Int point) {
        //上下左右にタイルが存在するかどうか ( 0:上 / 1:右 / 2:下 / 3:左 ) 
        bool[] is_Exist = { true, true, true, true };
        if (_tilemap.GetTile(point + new Vector3Int(0, 1, 0)) == null)
            is_Exist[0] = false;
        if (_tilemap.GetTile(point + new Vector3Int(1, 0, 0)) == null)
            is_Exist[1] = false;
        if (_tilemap.GetTile(point + new Vector3Int(0, -1, 0)) == null)
            is_Exist[2] = false;
        if (_tilemap.GetTile(point + new Vector3Int(-1, 0, 0)) == null)
            is_Exist[3] = false;
        
        foreach(SurfaceTile sTile in surface_Tiles) {
            if(ArraysComparator.Is_Equals(is_Exist, sTile.is_Exist)) {
                _tilemap.SetTile(point, sTile.tile);
                break;
            }
        }
    }


   
}
