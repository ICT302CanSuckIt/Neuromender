using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameState : MonoBehaviour
{
	private static GameState instance;

	//public float timer;
	public int rings;
	public float leftThresh;
	public float rightThresh;
	public float threshold;
	public float score;
	
	public Vector3 leftShoulder;
	public Vector3 leftElbow;
	public Vector3 leftHand;
	public Vector3 rightShoulder;
	public Vector3 rightElbow;
	public Vector3 rightHand;
	public Vector3 bodyCenter;
	
	public float rightArmAngle;
	public float leftArmAngle;
	public float leftArmExtend;
	public float rightArmExtend;


//	public int score01;
//	public int score02;
//	public int score03;
	public string activeLevel;
	
	// recent level
	public bool lastLevel = false;
	public float lastScore;
	public int lastRings;
	public float lastThresh;
	//public int lastTime;
//	public int levelNum;
	
	
	//  Creates an instance of GameState if one does not exist
	public static GameState Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("GameState").AddComponent<GameState>();
			}
			
			return instance;
		}
	}
	
	//  Sets the instance to NULL on quit
	public void OnApplicationQuit()
	{
		instance = null;
	}
	
	//  Creates a new GameState
	public void StartState()
	{
		Debug.Log("Creating a new game state");
		
		rings = PlayerPrefs.GetInt("Rings");
		score = PlayerPrefs.GetInt("Score");
		
		// Load Map
		Debug.Log("Moving to Map Menu");
		GameState.Instance.SetLevel("LevelSelect");
        SceneManager.LoadScene("LevelSelect");
	}
	
	public string GetLevel()
	{
		return activeLevel;
	}
	
	public void SetLevel(string newLevel)
	{
		activeLevel = newLevel;
	}
	
	public void ResetScores()
	{
//		score01 = 0;
//		PlayerPrefs.SetInt("Score01", 0);
//		score02 = 0;
//		PlayerPrefs.SetInt("Score02", 0);
//		score03 = 0;
//		PlayerPrefs.SetInt("Score03", 0);
	}
}
