using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreData : MonoBehaviour {

	//public float timer;
	public int rings;
	//public float leftThresh;
	//public float rightThresh;
	public float threshold;
	public int score;

	//public int ringValue = 10;
	//public int threshValue = 25;
	
	//public int ringBonus = 25;
	//public int threshBonus = 25;
	
	//private float btnWide = 0.2f;
	//private float btnHigh = 0.08f;
	
	//private float left;
	//private float right;
	//private float top;
	//private float width;
	//private float height;
	//private float spacer;
	
	//public GUISkin levelSplashSkin;
	//public GUISkin levelSplashSkin1;	
	
//	public AudioClip gotCoinSound;
//	public AudioClip got10CoinSound;
//	public AudioSource audioSource;

	//bool levelOver;
    //bool paused;

    //public Texture2D finish;

    //public GameObject splash;
    //public GameObject camObj;
	
	//private float ratio;
	
	
	
	// Use this for initialization
	void Start()
	{
		//ratio = Screen.height / Screen.width;
		
		//Time.timeScale = 1.0f;
		//splash.guiTexture.enabled = false;
		//levelOver = false;		
		//paused = false;		
		//camObj.camera.enabled = true;
		
		
		score = 0;
		rings = 0;
		threshold = 0;
	}

	public int GetScore() {
		return score;
	}

	public int GetRings() {
		return rings;
	}


	void Update()
	{
		//ratio = Screen.height / Screen.width;
		//width = Screen.width * btnWide * ratio;
		//height = Screen.height * btnHigh;

		//left = Screen.width * 0.005f;
		//right = (float)(((Screen.width) - (btnWide / 2)) * 0.9);// * 0.750f;
		//right = (float)(Screen.width * 0.79);// * 0.750f;
		//print ("" + right);
		//print ("" + btnWide);
		//top = Screen.height * 0.005f;
		
		//spacer = 0.3f * height;
	}
	
	private void OnGUI()
	{
		//ratio = Screen.width / (Screen.height * 1.6f);
		//width = Screen.width * btnWide / ratio;
		//height = Screen.height * btnHigh;
		//GUI.skin = levelSplashSkin;
		//levelSplashSkin.button.fontSize = ( int )(height * 0.75f);
		//levelSplashSkin.box.fontSize = ( int )(height * 0.75f);
		//levelSplashSkin1.button.fontSize = ( int )(height * 0.75f);
		//levelSplashSkin1.box.fontSize = ( int )(height * 0.75f);
		
		/*if (levelOver) {
			GUI.skin = levelSplashSkin1;
			
			if (GUI.Button (new Rect (0.5f * Screen.width - 1.5f * width - 2 * spacer, Screen.height * 0.75f, width, height), "Restart")) {
				Debug.Log ("Restarting Level");
				Application.LoadLevel (Application.loadedLevelName);
				Debug.Log (Application.loadedLevelName);
			}
			
			if (GUI.Button (new Rect (0.5f * Screen.width - 0.5f * width, Screen.height * 0.75f, width, height), "Level Select")) {
				GameState.Instance.SetLevel ("LevelSelect");
				Application.LoadLevel ("LevelSelect");
			}
			
			if (GUI.Button (new Rect (0.5f * Screen.width + 0.5f * width + 2 * spacer, Screen.height * 0.75f, width, height), "Quit")) { 
				GameState.Instance.SetLevel ("LevelSelect");
				Application.LoadLevel ("MainMenu");
			}

			GUI.Box (new Rect (0.5f * Screen.width - 1.5f * width - 2 * spacer, Screen.height * 0.65f, width, height), "Rings: ");
			GUI.Box (new Rect (0.5f * Screen.width - 1.5f * width - 2 * spacer, Screen.height * 0.65f, width, height), "" + rings);
			GUI.Box (new Rect (0.5f * Screen.width - 0.5f * width, Screen.height * 0.65f, width, height), "Threshold: " + threshold);
			GUI.Box (new Rect (0.5f * Screen.width - 0.5f * width, Screen.height * 0.65f, width, height), "Threshold: " + threshold);
			GUI.Box (new Rect (0.5f * Screen.width - 0.5f * width, Screen.height * 0.65f, width, height), "Threshold: " + threshold);
			//GUI.Box(new Rect(0.5f * Screen.width + 0.5f * width + 2 * spacer, Screen.height * 0.65f, width, height), "Time: " + ( int )timer);

			width *= 3;
			height *= 2;			
			levelSplashSkin1.box.fontSize = (int)(height * 0.75f);
			//GUI.Box(new Rect(0.5f * Screen.width - 0.5f * width, Screen.height * 0.48f, width, height), "Score: " + score);
			GUI.Box (new Rect (0.5f * Screen.width - 0.5f * width, Screen.height * 0.1f, width, height), "Score: " + score);
		}


        else if (paused) {

            if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), "Restart"))
            {
                Debug.Log("Restarting Level");
                Application.LoadLevel(Application.loadedLevelName);
                Debug.Log(Application.loadedLevelName);
            }
			
            if (GUI.Button(new Rect(left, 1 * (spacer + height) + top, width, height), "Quit"))
            {
                Debug.Log("Moving to Map Menu");
                GameState.Instance.SetLevel("LevelSelect");
                Application.LoadLevel("LevelSelect");
            }
			
            if (GUI.Button(new Rect(left, 2 * (spacer + height) + top, width, height), "Unpause"))
            {
                paused = false;
            }
        }

	
        else
        {
            /*GUI.Box(new Rect(right, 2 * (spacer + height) + top, width, height), "" + rings + " To Go");
            GUI.Box(new Rect(right, 1 * (spacer + height) + top, width, height), "Rings: " + rings);
            GUI.Box(new Rect(right, 0 * (spacer + height) + top, width, height), "Score: " + score);
        } */
	}
	
	public void FinalScore()
	{
		GameState.Instance.lastRings = rings;
		GameState.Instance.lastThresh = threshold;
		
		GameState.Instance.lastScore = score;
		
		GameState.Instance.lastLevel = true;
	}
	
	public void EndLevel(/*int type*/)
	{
		FinalScore();
		//splash.guiTexture.texture = finish;
		
		//splash.guiTexture.enabled = true;
		//levelOver = true;	
		//camObj.camera.enabled = false;
		//Time.timeScale = 0.0f;
	}
}

