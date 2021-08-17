using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.CheckBalls();
        }
        if (other.CompareTag("MainBall"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.SpawnMainBall();
        }
    }
}
