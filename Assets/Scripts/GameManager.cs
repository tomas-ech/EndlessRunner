using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player playerScript;
    public UI_Main ui;

    [Header("Color Info")]
    public Color platformColor;

    [Header("Score Info")]
    public int coins;
    public float distance;
    public float score;
    

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        LoadColor();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;

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
        SceneManager.LoadScene(0);
    }

    public void Save()
    {
        //En una variable se cargan los coins que ya se habian guardado por el playerpref
        int savedCoins = PlayerPrefs.GetInt("Coins");

        //Aqui guardamos los coins cargados mas los nuevos obtenidos
        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        score = (distance * coins)/100;

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

    public void GameEnded()
    {
        Save();
        ui.OpenEndGameUI();
    }

}
