using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives = 5;
    public float LethalPitDepth = -4f;
    public float RespawnHeight = 4f;
    public PlayerController Player;

    public PlatfromBuilder PlatformBuilder;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    CheckIfPlayerDied();
	}

    private void CheckIfPlayerDied()
    {
        if (Player.transform.position.y <= PlatformBuilder.LowestPlatform + LethalPitDepth)
        {
            if (lives >= 0)
            {
                Player.TeleportPlayer(PlatformBuilder.MiddleOfOldestPlatform);
                Debug.Log(PlatformBuilder.MiddleOfOldestPlatform);
                lives--;
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }
    }
}
