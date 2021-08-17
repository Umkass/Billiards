using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]public int currentNumberOfBalls;
    public GameObject mainBallPrefab;
    public TextMeshProUGUI message;
    void Start()
    {
        currentNumberOfBalls = 15;
        message.text = "The ball hasn't stopped yet";
        message.gameObject.SetActive(false);
    }
   public void SpawnMainBall()
    {
        Instantiate(mainBallPrefab, mainBallPrefab.transform.position, Quaternion.identity);
    }
    public void ShowMessage(bool show)
    {
        message.gameObject.SetActive(show);
    }
    public void CheckBalls()
    {
        currentNumberOfBalls--;
        if(currentNumberOfBalls <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
