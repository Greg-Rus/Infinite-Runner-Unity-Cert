using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Text _lives;
    [SerializeField] private Text _score;
    [SerializeField] private Text _enemies;

    public int Lives
    {
        set { _lives.text = value.ToString(); }
    }

    public int Score
    {
        set { _score.text = value.ToString(); }
    }

    public int Enemies
    {
        set { _enemies.text = value.ToString(); }
    }
}
