using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScore : MonoBehaviour
{
    public static UIScore instance { get; private set; }

    public int scoreAmount = 0;
    public int totalRobots = 6;
    public TextMeshProUGUI scoreText;
    public GameObject winText;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText.text = "Fixed Robots: " + scoreAmount + "/" + totalRobots;
        winText.SetActive(false);
    }

    public void SetValue(bool isFixed)
    {
        if (isFixed == true)
        {
            scoreAmount++;
            scoreText.text = "Fixed Robots: " + scoreAmount + "/" + totalRobots;
            if (scoreAmount == 6)
            {
                winText.SetActive(true);
            }
        }
    }
}
