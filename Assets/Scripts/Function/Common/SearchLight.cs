using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁を貫通しない光を作る 
/// 
/// このオブジェクトか親にRigidbody2Dをつけること
/// </summary>
public class SearchLight : SliceTextureCollider {

    [HideInInspector] public List<string> wall_Tags = new List<string> { "GroundTag" };
    [HideInInspector] public string detect_Tag = "PlayerBodyTag";

    private int tile_Num;    
    private SearchLightTile[] _tile_Collisions;


    void Awake() {
        Create();
        tile_Num = tiles.Length;
        _tile_Collisions = new SearchLightTile[tile_Num];
        for(int i = 0; i < tile_Num; i++) {
            _tile_Collisions[i] = tiles[i].obj.AddComponent<SearchLightTile>();
            _tile_Collisions[i].Setting(this);
        }        
    }


    void Update() {
        //centerから左右に見ていく,途中で壁があればそれ以降は非表示
        bool[] on_Wall = { false, false };
        
        for(int i = center.x; i >= 0; i--) {
            if (on_Wall[0]) {
                _tile_Collisions[i].To_Disable();
            }
            else if (_tile_Collisions[i].on_Wall) {
                _tile_Collisions[i].To_Disable();
                on_Wall[0] = true;
            }
            else {
                _tile_Collisions[i].To_Enable();
            }
        }
        for (int i = center.x; i < tile_Num; i++) {
            if (on_Wall[1]) {
                _tile_Collisions[i].To_Disable();
            }
            else if (_tile_Collisions[i].on_Wall) {
                _tile_Collisions[i].To_Disable();
                on_Wall[1] = true;
            }
            else {
                _tile_Collisions[i].To_Enable();
            }
        }

    }


    public bool Is_Detect() {
        for(int i = 0; i < tile_Num; i++) {
            if (_tile_Collisions[i].is_Detect)
                return true;
        }
        return false;
    }


    //当たり判定のサイズを変える
    public override void Attach_Collision(Tile tile) {
        BoxCollider2D _collider = tile.obj.AddComponent<BoxCollider2D>();
        _collider.size = new Vector2(tile.size.x, tile.size.y / 2);
        _collider.isTrigger = is_Trigger;
    }
}
