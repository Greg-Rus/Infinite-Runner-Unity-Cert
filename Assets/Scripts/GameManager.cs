using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _lives = 5;
    public float LethalPitDepth = -4f;
    public float RespawnHeight = 4f;
    public PlayerController Player;
    [SerializeField] private int _score;
    [SerializeField] private int _enemies;
    public HudController Hud;
    public int EnemyKillMultiplyer = 10;
    [SerializeField] private int _distanceTraveled = 0;


    public PlatfromBuilder PlatformBuilder;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            Hud.Score = _score;
        }
    }

    public int Enemies
    {
        get { return _enemies; }
        set
        {
            _enemies = value;
            Hud.Enemies = _enemies;
        }
    }

    public int Lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            Hud.Lives = _lives;
        }
    }

    public int DistanceTraveled
    {
        get { return _distanceTraveled; }
        set
        {
            _distanceTraveled = value;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        Hud.Score = _distanceTraveled + _enemies;
    }

    // Use this for initialization
	void Start ()
	{
	    Hud.Lives = Lives;
	    Hud.Enemies = 0;
	    Hud.Score = 0;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    CheckIfPlayerDied();
	    CheckPlayerProgress();
	}

    private void CheckPlayerProgress()
    {
        DistanceTraveled = Mathf.Max(DistanceTraveled, (int) Player.transform.position.x);
    }

    private void CheckIfPlayerDied()
    {
        if (Player.transform.position.y <= PlatformBuilder.LowestPlatform + LethalPitDepth)
        {
            if (Lives >= 0)
            {
                Player.TeleportPlayer(PlatformBuilder.MiddleOfOldestPlatform);
                Lives--;
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void OnEnemyDeath(int HP)
    {
        Enemies = Enemies + HP * EnemyKillMultiplyer;
        UpdateScore();
    }

    
}
