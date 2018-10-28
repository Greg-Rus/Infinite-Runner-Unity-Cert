using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformController : MonoBehaviour
{

    public GameObject Tile;
    public Sprite TopTileSprite;
    public Sprite MidTileSprite;

    public int Width;
    public int Height;

    public int CliffWidth = 0;
    public int CliffHeight = 0;
    public int CliffOffset = 0;

    private List<GameObject> _tiles;

    private bool BecameVisible = false;
	// Use this for initialization
	void Start ()
	{
	    BuildPlatform();
    }

    public void SetupUpPlatform(int width, int height)
    {
        _tiles = new List<GameObject>();
        Width = width;
        Height = height;
    }

    private void BuildPlatform()
    {
        AddCollider(Width, Height);
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

        //foreach (var go in _tiles)
        //{
        //    go.GetComponent<SpriteRenderer>().sprite = MidTileSprite;
        //}
        AssignTileSprites();
    }

    private void AddCollider(int width, int height, int offset = 0)
    {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(width, height, 0);
        collider.offset = new Vector3(width * 0.5f - 0.5f + offset, height * -0.5f + 0.5f, 0f);
    }

    public void AddCliff(int width, int height, int offset)
    {
        CliffWidth = width;
        CliffHeight = height;
        CliffOffset = offset;
        for (int x = offset; x < offset + width; x++)
        {
            for (int y = 1; y <= height; y++)
            {
                var newTile = Instantiate(Tile, new Vector3((float)x, (float)y, 0f), Quaternion.identity);
                _tiles.Add(newTile);
                newTile.transform.SetParent(transform, false);
            }
        }

        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(width, height, 0);
        collider.offset = new Vector3(width * 0.5f - 0.5f + offset, height * 0.5f + 0.5f, 0f);

        AssignTileSprites();
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

    private void AssignTileSprites()
    {
        var columns = _tiles.GroupBy(o => o.transform.position.x);
        foreach (var gameObjects in columns)
        {
            var sortedTiles = gameObjects.OrderByDescending(go => go.transform.position.y);
            sortedTiles.First().GetComponent<SpriteRenderer>().sprite = TopTileSprite;
            foreach (var go in sortedTiles.Skip(1))
            {
                go.GetComponent<SpriteRenderer>().sprite = MidTileSprite;
            }
        }
    }
}
