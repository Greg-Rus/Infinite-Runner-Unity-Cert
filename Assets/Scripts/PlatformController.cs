using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformController : MonoBehaviour
{

    public GameObject Tile;
    public Sprite TopTileSprite;
    public Sprite MidTileSprite;

    public int Width;
    public int Height;

    private BoxCollider2D _boxCollider2D;

    private List<GameObject> _tiles;

    private bool BecameVisible = false;
	// Use this for initialization
	void Start ()
	{
	    _boxCollider2D = GetComponent<BoxCollider2D>();
	    _tiles = new List<GameObject>();
	    BuildPlatform();
    }

    public void SetupUpPlatform(int width, int height)
    {
        Width = width;
        Height = height;
    }

    private void BuildPlatform()
    {
        _boxCollider2D.size = new Vector3(Width, Height, 0);
        _boxCollider2D.offset = new Vector3(Width * 0.5f - 0.5f, Height * -0.5f + 0.5f, 0f);
        for (int y = 0; y > -Height; y--)
        {
            for (int x = 0; x < Width; x++) 
            {
                var newTile = Instantiate(Tile, new Vector3((float) x, (float) y, 0f), Quaternion.identity);
                _tiles.Add(newTile);
                newTile.transform.SetParent(transform,false);

            }
        }

        //for (int i = 0; i < Width; i++)
        //{
        //    _tiles[i].GetComponent<SpriteRenderer>().sprite = TopTileSprite;
        //}

        foreach (var go in _tiles)
        {
            go.GetComponent<SpriteRenderer>().sprite = _tiles.IndexOf(go) < Width ? TopTileSprite : MidTileSprite;
        }
    }

    void OnBecameVisible()
    {
        BecameVisible = true;
    }

    void OnBecameInvisible()
    {
        if (BecameVisible)
        {
            Destroy(gameObject);
        }
    }

    public float GetPlatformRightEdge
    {
        get { return GetPlatformLeftEdge + Width + 0.5f; }
    }

    public float GetPlatformLeftEdge
    {
        get { return transform.position.x - 0.5f; }
    }

    public float GetPlatformHeight
    {
        get { return transform.position.y; }
    }

    public Vector2 GetMiddlePlatformPosition
    {
        get
        {
            return (Vector2)transform.position + new Vector2((GetPlatformRightEdge - GetPlatformLeftEdge) * 0.5f, 0.5f);
        }
    }
}
