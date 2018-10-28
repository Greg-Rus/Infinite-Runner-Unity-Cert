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
    public int MinPlatformWidthForCliff = 4;
    public int MaxCliffHeight = 2;
    public float MaxPlatformPosition = 3f;
    public float MinPlatformPosition = -2f;
    public float LowestPlatform { get; private set; }
    public Transform DespawnZone;

    [Header("EnemyPlacement")]
    public float FlyingEnemyChance;

    public float MaxPlatformGap;
    public float GroundEnemyChance;

    [Header("EnemyPrefabs")]
    public Enemy Bobber;
    public Enemy Walker;

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
	    UpdateDespawnZone();
	}

    private void UpdateDespawnZone()
    {
        var newDespawnZonePosition = Camera.transform.position;
        newDespawnZonePosition.y = LowestPlatform - 4f;
        DespawnZone.position = newDespawnZonePosition;
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

        bool shouldHaveFlyingEnemy = Random.value <= FlyingEnemyChance;
        bool shouldHaveGroundEnemy = Random.value <= GroundEnemyChance;
        

        var flayerGap = shouldHaveFlyingEnemy ? Vector2.right * MaxPlatformGap * Random.value : Vector2.zero;

        var newPlatformPosition = new Vector2(oldPlatformEdge, lastPlatform.GetPlatformHeight)
                                  + Vector2.right * 0.5f + flayerGap
                                  + new Vector2(xOffset, yOffset);
        var platform = SpawnPlatform(newPlatformPosition, newPlatformWidth, newPlatformHeight);

        //if (newPlatformWidth >= MinPlatformWidthForCliff)
        //{
        //    var cliffWidth = Random.Range(1, newPlatformWidth - MinPlatformWidthForCliff + 1);
        //    var maxCliffOffset = newPlatformWidth - cliffWidth;
        //    //platform.AddCliff(cliffWidth, MaxPlatformHeight, Random.Range(1, maxCliffOffset));
        //}

        if (shouldHaveFlyingEnemy)
            SpawnFlyer(new Vector2(lastPlatform.GetPlatformRightEdge, lastPlatform.GetPlatformHeight),
                newPlatformPosition);
        if (shouldHaveGroundEnemy)
            SpawnGroundEnemy(newPlatformPosition, new Vector2(platform.GetPlatformRightEdge, platform.GetPlatformHeight));

        return platform;
    }

    private void SpawnFlyer(Vector2 gapStart, Vector2 gapEnd)
    {
        var enemyPosition = (gapEnd - gapStart) * 0.5f + gapStart;
        Instantiate(Bobber, enemyPosition, Quaternion.identity);
    }

    private void SpawnGroundEnemy(Vector2 platformStart, Vector2 platformEnd)
    {
        var enemyPosition = (platformEnd - platformStart) * 0.5f + platformStart;
        Instantiate(Walker, enemyPosition, Quaternion.identity);
    }

    private Vector2 FlyingEnemyGap()
    {
        if (Random.value >= FlyingEnemyChance)
        {
            return Vector2.right * MaxPlatformGap * Random.value;
        }
        else
        {
            return Vector2.zero;
        }
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
