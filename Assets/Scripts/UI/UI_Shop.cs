using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct ColorToSell
{
    public Color color;
    public int price;
}

public enum ColorType
{
    playerColor,
    platformColor
}

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notifyText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [Space]

    [Header ("Platform Colors")]
    [SerializeField] Transform platformColorParent;
    [SerializeField] private GameObject platformColorButton;
    [SerializeField] private Image platformColorPreview;
    [SerializeField] private ColorToSell[] platformColors;

    [Header("Player Colors")]
    [SerializeField] Transform playerColorParent;
    [SerializeField] private GameObject playerColorButton;
    [SerializeField] private Image playerColorPreview;
    [SerializeField] private ColorToSell[] playerColors;


    void Start()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("F0");


        for (int i= 0; i < platformColors.Length; i++)
        {
            Color color = platformColors[i].color;
            int price = platformColors[i].price;

            GameObject newButton = Instantiate(platformColorButton, platformColorParent);

            newButton.transform.GetChild(0).GetComponent<Image>().color = platformColors[i].color;

            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = platformColors[i].price.ToString("F0");

            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.platformColor));
        }

        for (int i= 0; i < playerColors.Length; i++)
        {
            Color color = playerColors[i].color;
            int price = playerColors[i].price;

            GameObject newButton = Instantiate(playerColorButton, playerColorParent);

            newButton.transform.GetChild(0).GetComponent<Image>().color = playerColors[i].color;

            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerColors[i].price.ToString("F0");

            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.playerColor));
        }
    }

    public void PurchaseColor(Color color, int price, ColorType colorType)
    {
        AudioManager.instance.PlaySFX(4);
        
        if (EnoughMoney(price))
        {
            if (colorType == ColorType.platformColor)
            {
                GameManager.instance.platformColor = color;
                platformColorPreview.color = color;
            }
            else if (colorType == ColorType.playerColor)
            {
                GameManager.instance.playerScript.GetComponent<SpriteRenderer>().color = color;
                GameManager.instance.SaveColor(color.r, color.g, color.b);
                playerColorPreview.color = color;

            }
            
            StartCoroutine(Notify("Purchase successful", 1f));
        }

        else
        {
            StartCoroutine(Notify("Not enough money!", 1f));
        }
    }

    private bool EnoughMoney(int price)
    {
        int myCoins = PlayerPrefs.GetInt("Coins");

        if (myCoins > price)
        {
            int newAmountofCoins = myCoins - price;
            PlayerPrefs.SetInt("Coins", newAmountofCoins);
            coinsText.text = PlayerPrefs.GetInt("Coins").ToString("F0");

            Debug.Log("Purchase Successful");
            return true;
        }
        else {return false;}
    }

    IEnumerator Notify(string text, float seconds)
    {
        notifyText.text = text;

        yield return new WaitForSeconds(seconds);

        notifyText.text = "Tap to buy";
    }

}
