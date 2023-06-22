using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer platformSprite;
    [SerializeField] private SpriteRenderer headerSprite;

    void Start()
    {
        headerSprite.transform.parent = transform.parent;
        //spriterenderer.BOUNDS habla de los limites del sprote SIZE del tamaño en un eje, MAX toma el punto mayor del eje
        headerSprite.transform.localScale = new Vector2(platformSprite.bounds.size.x, 0.2f);
        headerSprite.transform.position = new Vector2(transform.position.x, platformSprite.bounds.max.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            headerSprite.color = GameManager.instance.platformColor;
        }
    }
}
