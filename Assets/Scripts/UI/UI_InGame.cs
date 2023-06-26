using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;

    void Start()
    {
        playerScript = GameManager.instance.playerScript;
        InvokeRepeating("UpdateInfo", 0f, 0.15f);
    }

    private void UpdateInfo()
    {
        distanceText.text = GameManager.instance.distance.ToString("F0") + " m";
        coinsText.text = GameManager.instance.coins.ToString();
        
        heartEmpty.enabled = !playerScript.extraLife;
        heartFull.enabled = playerScript.extraLife;
    }
}
