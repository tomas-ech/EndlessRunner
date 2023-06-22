using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //esta codicion es igual que poner other.tag == "Player" solo que en vez de revisar un tag, revisa si el objeto que toca tiene el componente Player, esto evita poner mal el nombre del tag
        if (other.GetComponent<Player>() != null)
        {
            GameManager.instance.coins++;
            Destroy(gameObject);
        }
    }
}
