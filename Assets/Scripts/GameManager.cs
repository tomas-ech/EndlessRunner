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

    [Header("Score Info")]
    public int coins;
    public float distance;
    

    private void Awake()
    {
        instance = this;
        LoadColor();
    }

    public void SaveColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);
    }

    private void LoadColor()
    {
        SpriteRenderer sr = playerScript.GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                    PlayerPrefs.GetFloat("ColorG"),
                                    PlayerPrefs.GetFloat("ColorB"),
                                    PlayerPrefs.GetFloat("ColorA", 1));

        sr.color = newColor;
    }

    private void Update()
    {
        if (playerScript.transform.position.x > distance)
        {
            distance = playerScript.transform.position.x;
        }
    }

    public void UnlockPlayer() => playerScript.playerUnlocked = true;
    public void RestartLevel()
    {
        Save();
        SceneManager.LoadScene(0);
    }

    public void Save()
    {
        //En una variable se cargan los coins que ya se habian guardado por el playerpref
        int savedCoins = PlayerPrefs.GetInt("Coins");

        //Aqui guardamos los coins cargados mas los nuevos obtenidos
        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        float score = (distance * coins)/100;

        PlayerPrefs.SetFloat("LastScore", score);

        float lastScore = PlayerPrefs.GetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("HighScore") < score)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        PlayerPrefs.SetFloat("ColorR", 1);
        PlayerPrefs.SetFloat("ColorG", 1);
        PlayerPrefs.SetFloat("ColorB", 1);
    }


}
