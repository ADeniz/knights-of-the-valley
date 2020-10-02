using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject[] chars = null;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = (GameObject)Instantiate(chars[0]);
        player.name = "Player";
        GameObject enemy = (GameObject)Instantiate(chars[1]);
        enemy.name = "Enemy";
        Char enemyOfChar = enemy.GetComponent<Char>();
        enemyOfChar.currentPlayer = false;
        Vector3 position = enemy.transform.position;
        position.y += 50;
        //enemy.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
