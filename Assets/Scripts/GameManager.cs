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

    public void Save()
    {
        //En una variable se cargan los coins que ya se habian guardado por el playerpref
        int savedCoins = PlayerPrefs.GetInt("Coins");

        //Aqui guardamos los coins cargados mas los nuevos obtenidos
        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        float score = distance * coins;

        float lastScore = PlayerPrefs.GetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("HighScore") < score)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }


}
