using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class LevelScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    public Transform Spawnpoint;
    public Transform Checkpoint;

    private List<GameObject> players = new List<GameObject>();
    private bool _gameOver;
    
    IEnumerator Start()
    {
        yield return null; // wait 1 frame

        player1 = GameObject.Find("Player Spawnpoint").transform.GetChild(0).gameObject;
        players.Add(player1);
        if (GameManagerScript.instance.playerCount == 2)
        {
            player2 = GameObject.Find("Player2 Spawnpoint").transform.GetChild(0).gameObject;
            players.Add(player2);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManagerScript.instance.score1 = GameObject.Find("Player Spawnpoint").GetComponentInChildren<PlayerStatsScript>().score;

            if (GameManagerScript.instance.playerCount == 2)
            {
                GameManagerScript.instance.score2 = GameObject.Find("Player2 Spawnpoint").GetComponentInChildren<PlayerStatsScript>().score;
            }
            SceneManager.LoadScene(8);
        }

        CheckGameOver();

        if (_gameOver)
        {
            Respawn();
        }
    }

    public void CheckGameOver()
    {
        _gameOver = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponentInParent<PlayerStatsScript>().currentHP > 0)
            {
                _gameOver = false;
            }
        }
    }

    public void Respawn()
    {

    }
}
