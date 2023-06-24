using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player playerScript;
    public static GameManager instance;

    [Header("Color Info")]
    public Color platformColor;
    public Color playerColor = Color.white;


    [Header("Score Info")]
    public int coins;
    public float distance;
    

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (playerScript.transform.position.x > distance)
        {
            distance = playerScript.transform.position.x;
        }
    }

    public void UnlockPlayer() => playerScript.playerUnlocked = true;
    public void RestartLevel() => SceneManager.LoadScene(0);


}
