using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlatfromBuilder : MonoBehaviour
{
    public Camera Camera;
    public PlatformController PlatformPrefab;
    public LayerMask GroundLayer;
    public int PlatformCount = 6;
    public float MaxJumpRange = 4f;
    public float MinJumpRange = 2f;
    public float MaxJumpHeight = 3f;
    public float MinJumpHeight = 2f;
    public int MaxPlatformWidth = 6;
    public int MaxPlatformHeight = 4;
    public float MaxPlatformPosition = 3f;
    public float MinPlatformPosition = -2f;
    public float LowestPlatform { get; private set; }

    [Header("Debug")]
    public float PlatformRightEdge;
    public float ScreenLeftEdge;

    private Queue<PlatformController> _platformQueue;

    // Use this for initialization
	void Start () {
	    _platformQueue = new Queue<PlatformController>(PlatformCount);
	    if (Camera == null)
	    {
	        Camera = Camera.main;
	    }
	    LowestPlatform = 0f;
        SpawnInitialPlatforms();
	}

    void Update()
    {
        if (CheckIfOldestPlatformOutOfView())
        {
            DestroyOldestPlatform();
            GetNewRandomPlatform(GetLastPlatform);
        }
    }

    private void SpawnInitialPlatforms()
    {
        SpawnPlatform(Vector2.zero, 5, 2);
        for (int i = 1; i < PlatformCount; i++)
        {
            GetNewRandomPlatform(GetLastPlatform);
        }
    }

    private PlatformController GetNewRandomPlatform(PlatformController lastPlatform)
    {
        var newPlatformWidth = Random.Range(1, MaxPlatformWidth + 1);
        var newPlatformHeight = Random.Range(1, MaxPlatformHeight + 1);

        var oldPlatformEdge = lastPlatform.GetPlatformRightEdge;
        var offsetPercentage = Random.value;
        var xOffset = Mathf.Lerp(MinJumpRange, MaxJumpRange, offsetPercentage);
        var yOffset = Mathf.Lerp(MaxJumpHeight, MinJumpHeight, offsetPercentage);
        if ((int) (Random.value * 100) % 2 == 0) yOffset *= -1f;
        
        var newPlatformPosition = new Vector2(oldPlatformEdge, lastPlatform.GetPlatformHeight)
                                  + Vector2.right * 0.5f
                                  + new Vector2(xOffset, yOffset);
        return SpawnPlatform(newPlatformPosition, newPlatformWidth, newPlatformHeight);
    }

    public PlatformController SpawnPlatform(Vector2 position, int width, int height)
    {
        var platform = Instantiate(PlatformPrefab, position, Quaternion.identity);
        platform.SetupUpPlatform(width,height);
        _platformQueue.Enqueue(platform);
        LowestPlatform = _platformQueue.Min(p => p.GetPlatformHeight);
        return platform;
    }

    public void DestroyOldestPlatform()
    {
        var oldPlatform = _platformQueue.Dequeue();
        Destroy(oldPlatform.gameObject);
    }

    private PlatformController GetLastPlatform
    {
        get { return _platformQueue.Last(); }
    }

    private bool CheckIfOldestPlatformOutOfView()
    {
        var platform = _platformQueue.Peek();
        PlatformRightEdge = platform.GetPlatformRightEdge + 1;
        ScreenLeftEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        return PlatformRightEdge < ScreenLeftEdge;
    }

    public Vector2 MiddleOfOldestPlatform
    {
        get
        {
            var oldestPlatform = _platformQueue.Peek();
            return oldestPlatform.GetMiddlePlatformPosition;
        }
    }
}
