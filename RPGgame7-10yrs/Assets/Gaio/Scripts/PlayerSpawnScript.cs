using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
    private GameObject p1Prefab;
    private GameObject p2Prefab;

    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1Prefab = GameManagerScript.instance.playerPrefabs[GameManagerScript.instance.player1index];
        p1Prefab.GetComponent<PlayerInput>().PlayerNumber = 1;
        Instantiate(p1Prefab, p1SpawnPoint);
        if (GameManagerScript.instance.playerCount == 2)
        {
            p2Prefab = GameManagerScript.instance.playerPrefabs[GameManagerScript.instance.player2index];
            p2Prefab.GetComponent<PlayerInput>().PlayerNumber = 2;
            Instantiate(p2Prefab, p2SpawnPoint);
        }
        else
        {
            GameObject.Find("PlayerUI 2").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
