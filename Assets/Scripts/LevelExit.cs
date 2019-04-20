using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private float LevelLoadDelay = 1f;

    public List<GameObject> gameobjectsToHide;
    public List<GameObject> gameobjectsToShow;

    public GameObject player;
    public TextMeshProUGUI ColletCoinWarning;
    [SerializeField] private int CoinsMustBeCollected = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.GetComponent<Player>().Coins >= CoinsMustBeCollected)
        {
            StartCoroutine(ShowGameResults());
        }
        else if (player.GetComponent<Player>().Coins < CoinsMustBeCollected)
        {
            //show warning;
            ColletCoinWarning.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ColletCoinWarning.gameObject.SetActive(false);
    }

    IEnumerator ShowGameResults()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);



        for (int i = 0; i < gameobjectsToHide.Count; i++)
        {
            gameobjectsToHide[i].SetActive(false);
        }
        for (int i = 0; i < gameobjectsToShow.Count; i++)
        {
            gameobjectsToShow[i].SetActive(true);
        }
        player.GetComponent<Player>().enabled = false;
    }


}

