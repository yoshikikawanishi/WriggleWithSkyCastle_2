using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ライトの画像を表示する、壁を貫通しない光を作る
/// このオブジェクトか親にRigidbody2Dをつけること
/// </summary>
public class LightTexture : MonoBehaviour {

    [SerializeField] private Texture2D texture;
    [SerializeField] private Vector2Int center;    
    [SerializeField] private Color color = new Color(1, 1, 1, 0.3f);
    [SerializeField] private int sorting_Order = 6;    

    public class Tile {
        public GameObject obj;        

        public Tile(GameObject obj) {
            this.obj = obj;            
        }
    }

    private Tile[] tiles;           //左下が(0, 0)
    private Vector2Int tile_Num;
    private int center_Index;

    private List<string> cut_Wall_Tags = new List<string> { "GroundTag" };


    void Awake() {
        //テクスチャのスライス
        TextureSlicer slice = new TextureSlicer();
        texture = slice.createReadableTexture2D(texture);
        int tile_Size = 1;
        Sprite[] sprites = slice.Slice_Sprite(texture, new Vector2Int(tile_Size, tile_Size));

        //変数の計算
        tile_Num = new Vector2Int(texture.width / tile_Size, texture.height / tile_Size);
        center_Index = center.y * tile_Num.x + center.x;

        //SpriteRendererオブジェクトの生成、tilesに代入                
        tiles = new Tile[sprites.Length];
        GameObject obj;
        SpriteRenderer obj_Sprite;
        Vector2Int position = new Vector2Int();            

        for (int i = 0; i < tile_Num.y; i++) {
            for (int j = 0; j < tile_Num.x; j++) {
                int index = i * tile_Num.x + j;
                position = new Vector2Int(tile_Size * j, tile_Size * i);                

                if (slice.Is_Tranceparent(texture, position, 0.02f)) {
                    obj = null;
                }
                else {
                    obj = new GameObject("Tile" + index);
                    obj.transform.SetParent(transform);
                    obj_Sprite = obj.AddComponent<SpriteRenderer>();
                    obj_Sprite.sprite = sprites[index];
                    obj_Sprite.sortingOrder = sorting_Order;
                    obj_Sprite.color = color;
                    obj.AddComponent<BoxCollider2D>().isTrigger = true;
                    obj.transform.localPosition = (Vector2)position - center;
                }                
                
                tiles[index] = new Tile(obj);                
            }
        }
                
        //Destroy(texture);
    }


    public void Destroy() {
        Destroy(texture);
        Destroy(gameObject);
    }


   
}
