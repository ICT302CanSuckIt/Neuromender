using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiamondScore : MonoBehaviour
{
    public Text scoreText;
    public Text FinalscoreText;

    public int CurrentScore = 0;
    public int DiamondPoints = 0;

    // Use this for initialization
    void Start()
    {
       // scoreText = GameObject.FindGameObjectWithTag("CurrentScore").GetComponent<Text>();
        UpdateScore(DiamondPoints);
    }

    public void UpdateScore( int addedValue)
    {
        CurrentScore += addedValue;
        scoreText.text = CurrentScore.ToString();
        FinalscoreText.text = CurrentScore.ToString();
    }
}