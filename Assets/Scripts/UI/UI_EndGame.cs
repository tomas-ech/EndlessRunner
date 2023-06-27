using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        GameManager manager = GameManager.instance;

        Time.timeScale = 0;

        distanceText.text = "Distance: " + manager.distance.ToString("F0") + " m";
        coinsText.text = manager.coins.ToString("F0") + " coins";
        scoreText.text = "Score: " + manager.score.ToString("F0");
    }

   
}
