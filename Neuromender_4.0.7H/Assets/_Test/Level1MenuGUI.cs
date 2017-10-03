using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level1MenuGUI : MonoBehaviour
{
	private float btnWide = 0.2f;
	private float btnHigh = 0.08f;
	
	private float left;
	private float top;
	private float width;
	private float height;
	private float spacer;
	
	public GUISkin mainMenuSkin; //ransakSkin;
	
	void Update()
	{
		left = Screen.width * (1 - btnWide) * 0.5f;
		top = Screen.height * (1 - btnHigh) * 0.5f;
		
		width = Screen.width * btnWide;
		height = Screen.height * btnHigh;
		
		spacer = 0.5f * height;
	}
	
	void OnGUI()
	{
		GUI.skin = mainMenuSkin;
		mainMenuSkin.button.fontSize = ( int )(height * 0.75f);
		mainMenuSkin.box.fontSize = ( int )(height * 0.75f);
		
		if (GUI.Button(new Rect(left, -2 * (spacer+height) + top, width, height), "Slow Track"))
		{
			Debug.Log("Course One selected");
            //LevelSelect();
            //StartGame();
            SceneManager.LoadScene("TestNeuromend3_C1");
		}
		
		if (GUI.Button(new Rect(left, -1 * (spacer+height) + top, width, height), "Mid Track"))
		{
			Debug.Log("Course Two selected");
            //Application.LoadLevel("HowToPlay");
            SceneManager.LoadScene("TestNeuromend3_C2");
		}
		
		if (GUI.Button(new Rect(left, 0 * (spacer+height) + top, width, height), "Fast Track"))
		{
			Debug.Log("Course Three selected");
            SceneManager.LoadScene("TestNeuromend3_C3");
		}	
		
		if (GUI.Button(new Rect(left, 2 * (spacer+height) + top, width, height), "Level Select"))
		{	
			//Debug.Log("Exit button clicked");
			Debug.Log ("Back button clicked");
            SceneManager.LoadScene("LevelSelect");
		}
	}
	
	
//	private void LevelSelect() //StartGame()
//	{
//		Debug.Log("Level Select Screen");
//		//Debug.Log("Starting Game");
//		//DontDestroyOnLoad(GameState.Instance);
//		//GameState.Instance.StartState();
//	}
}
