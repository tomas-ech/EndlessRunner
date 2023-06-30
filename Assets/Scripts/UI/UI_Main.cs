using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;
    [Space]

    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    [Header("Volume Info")]
    [SerializeField] private UI_Volume[] slider;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image inGameMuteIcon;


    private void Start()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].SetupSlider();
        }


        SwitchMenu(mainMenu);
        
        lastScoreText.text = "Last Score: " + PlayerPrefs.GetFloat("LastScore").ToString("F0");
        highScoreText.text = "Best Score: " + PlayerPrefs.GetFloat("HighScore").ToString("F0");
    }

    public void SwitchMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);

        AudioManager.instance.PlaySFX(4);

        totalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString("F0");
    }

    public void SwitchSkybox(int index)
    {
        AudioManager.instance.PlaySFX(4);
        GameManager.instance.SetupSkybox(index);
    }

    public void MuteButton()
    {
        gameMuted = !gameMuted;

        if (gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, 0.5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }

    }

    public void StartGame()
    { 
        muteIcon = inGameMuteIcon;
        GameManager.instance.UnlockPlayer();

        if (gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void PauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame() => GameManager.instance.RestartLevel();

    public void OpenEndGameUI()
    {
        SwitchMenu(endGame);
    }

}
