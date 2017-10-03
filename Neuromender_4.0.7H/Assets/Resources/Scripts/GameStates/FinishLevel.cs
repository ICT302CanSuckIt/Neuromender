using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour
{
	public GameObject scoreObject;
    HeadsUpDisplay myHUD;
	
	// Use this for initialization
	void Start()
	{
        scoreObject = GameObject.Find("HUD");
        myHUD = GameObject.Find("HUD").GetComponent<HeadsUpDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collObj)
	{
		if (collObj.tag == "Player")
		{
            //HeadsUpDisplay myScore = (HeadsUpDisplay)scoreObject.GetComponent("HeadsUpDisplay");
            //Sync out tracking data!
            GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>().syncTracking();
			Debug.Log("LEVEL FINISHED");

            //so.FinalScore();

            // 261new
            GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();

            myHUD.EndLevel();
            //myScore.EndLevel(/*1*/);
            //Scoring(myScore.rings/*so.score*/);
            Scoring(myHUD.rings);
		}
	}
	
	private void Scoring(int rings/*int score*/)
	{
		string level = GameState.Instance.GetLevel();
		GameState.Instance.rings = rings;
		PlayerPrefs.SetInt("Rings", rings);
	}
}

