using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    public int currenthealth;
    public int numOfHearts;
    public float colorA;
    public GameObject skullVfx;
    public GameObject[] skullPos;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Start()
    {
        currenthealth = numOfHearts;
    }
   
    void Update()
    {
        if(currenthealth <= 0)
        {
            Time.timeScale = 0f;
        }

        if (currenthealth > numOfHearts)
        {
            currenthealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currenthealth)
            {
                hearts[i].sprite = fullHeart;
                Color color = Color.white;
                color.a = 255;
                hearts[i].color = color;
            }
            else
            {
                hearts[i].sprite = fullHeart;
                Color color = Color.red;
                color.a = colorA;
                hearts[i].color = color;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            currenthealth -= 1;
            int index = Random.Range(0, skullPos.Length);
            GameObject skullSpawnPoint = skullPos[index];
            GameObject vfxSpawned = Instantiate(skullVfx, skullSpawnPoint.transform.position, skullVfx.transform.rotation);
            Destroy(vfxSpawned, 3);
            Destroy(collision.gameObject);
        }
    }
}
