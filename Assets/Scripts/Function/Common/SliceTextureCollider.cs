using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceTextureCollider : MonoBehaviour {

    protected enum Kind {
        vertical,   //縦に切る
        //horizon,    //横に切る
    }

    [SerializeField] protected Texture2D texture;
    [SerializeField] protected Color color;
    [SerializeField] protected int sorting_Order;
    [SerializeField] protected bool is_Trigger;
    [SerializeField] protected Vector2Int center;
    [SerializeField] protected Kind kind;           

    public class Tile {
        private SliceTextureCollider parent;
        private Sprite sprite;
        private Vector2 offset;
        public Vector2Int size;        
        private int id;
        public GameObject obj;        

        public Tile(SliceTextureCollider parent, Sprite sprite,  Vector2 offset, Vector2Int size, int id) {
            this.parent = parent;
            this.sprite = sprite;
            this.offset = offset;
            this.size = size;            
            this.id = id;    
        }

        public void Create() {
            obj = new GameObject("Tile" + id);
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = (Vector2)offset;                     
            SpriteRenderer _sprite = obj.AddComponent<SpriteRenderer>();
            _sprite.sprite = sprite;
            _sprite.color = parent.color;
            _sprite.sortingOrder = parent.sorting_Order;            
        }
    }

    [HideInInspector] public Tile[] tiles;    


    public void Create() {
        //テクスチャのスライス
        TextureSlicer slice = new TextureSlicer();
        texture = slice.createReadableTexture2D(texture);
        Sprite[] grid = slice.Slice_Sprite(texture, new Vector2Int(1, 1));        

        Vector2Int range;   //テクスチャの１マスグリッドのうち連続で透明ではないところの範囲(x, 始点 / y, 終点)
        Sprite sprite;      //タイル一枚のスプライト
        Vector2Int size;    //rangeのサイズ
        Vector2 offset;     //rangeの位置

        //縦に切るとき
        tiles = new Tile[texture.width];
        //縦一列のうち、透明になっていない範囲を取得        
        for(int x = 0; x < texture.width; x++) {
            range = new Vector2Int(-1, -1);
            for(int y = 0; y < texture.height; y++) {
                if (slice.Is_Tranceparent(texture, new Vector2Int(x, y), 0.01f)) {
                    if (range.y == -1 && range.x != -1)
                        range.y = y;
                }
                else if(range.x == -1) {
                    range.x = y;
                }
            }
            //すべて透明の時
            if (range.x == -1)
                range = new Vector2Int(0, 0);
            //すべて透明じゃないとき
            if (range.y == -1)
                range = new Vector2Int(0, texture.height);
            
            sprite = slice.Slice_Sprite(texture, new Vector2Int(1, texture.height))[x];
            size = new Vector2Int(1, range.y - range.x);
            offset = new Vector2(x, range.x + size.y / 2.0f) - center;
            tiles[x] = new Tile(this, sprite, offset, size, x);
            tiles[x].Create();
            Attach_Collision(tiles[x]);
        }
    }


    public virtual void Attach_Collision(Tile tile) {
        BoxCollider2D _collider = tile.obj.AddComponent<BoxCollider2D>();
        _collider.size = tile.size;
        _collider.isTrigger = is_Trigger;        
    }
}
