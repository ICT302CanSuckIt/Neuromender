using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuGUI : MonoBehaviour
{
	private float btnWide = 0.3f;
	private float btnHigh = 0.08f;
	
	private float left;
	private float top;
	private float width;
	private float height;
	private float spacer;

    private bool hoverLvlSelect = false;
    private bool hoverOptions = false;
    private bool hoverWebsite = false;
    private bool hoverLogout = false;
	public GUISkin mainMenuSkin; //ransakSkin;
    private GameObject levelSelectPanel;

    void Start()
    {

        levelSelectPanel = GameObject.Find("LevelSelectPanel");
        levelSelectPanel.SetActive(false);
    }
	
	void Update()
	{
		left = Screen.width * (1 - btnWide) * 0.5f;
		top = Screen.height * (1 - btnHigh) * 0.5f;
		
		width = Screen.width * btnWide;
		height = Screen.height * btnHigh;
		
		spacer = 0.5f * height;
        hoverTextPopulator();
	}
	
	void OnGUI()
	{
		GUI.skin = mainMenuSkin;
		mainMenuSkin.button.fontSize = ( int )(height * 0.75f);
		mainMenuSkin.box.fontSize = ( int )(height * 0.75f);

        if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Task Select", "LevelSelect")))
		{
			Debug.Log("Start button clicked");
            SceneManager.LoadScene("LevelSelect");
			//LevelSelect();
			//StartGame();
		}
		
//		if (GUI.Button(new Rect(left, -1 * (spacer+height) + top, width, height), "How To Play"))
//		{
//			Debug.Log("How To Play button clicked");
//			//Application.LoadLevel("HowToPlay");
//		}

        if (GUI.Button(new Rect(left, -1 * (spacer + height) + top, width, height), new GUIContent("Options", "Options")))
		{
			Debug.Log("Options button clicked");
            SceneManager.LoadScene("Options");
		}

        if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), new GUIContent("Website", "Website")))
		{
			Debug.Log("Website button clicked");

            // handled by button manager now
            //Application.OpenURL("http://vegas.murdoch.edu.au/neuromend/Main/index.php");
		}

        if (GUI.Button(new Rect(left, 2 * (spacer + height) + top, width, height), new GUIContent("Log Out", "Logout")))
		{	
			//Debug.Log("Exit button clicked");
			Debug.Log ("Log Out button clicked");
            if (GameObject.Find("DatabaseController"))
            {
                GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();
                GameObject.Destroy(GameObject.Find("DatabaseController"));
            }
            SceneManager.LoadScene("Login");
			//Application.Quit();
		}

        var hover = GUI.tooltip;

        if (hover == "LevelSelect")
        {
            //print("Forest (Medium Detail)");
            hoverLvlSelect = true;
            hoverOptions = false;
            hoverWebsite = false;
            hoverLogout = false;
        }
        else if (hover == "Options")
        {
            //print("Beach (Low Detail)");
            hoverLvlSelect = false;
            hoverOptions = true;
            hoverWebsite = false;
            hoverLogout = false;
        }
        else if (hover == "Website")
        {
            //print("Beach (Low Detail)");
            hoverLvlSelect = false;
            hoverOptions = false;
            hoverWebsite = true;
            hoverLogout = false;
        }
        else if (hover == "Logout")
        {
            //print("Beach (Low Detail)");
            hoverLvlSelect = false;
            hoverOptions = false;
            hoverWebsite = false;
            hoverLogout = true;
        }
	}

    public void hoverTextPopulator()
    {
        if (hoverLvlSelect)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Task Selection. \nClick on this to go to the task selection.";

        }
        else if (hoverOptions)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Options Menu. \nClick on this to see the configurable options, view the tutorial or credits.";

        }
        else if (hoverWebsite)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Website. \nClick on this to go to the Neuromend Web Portal.";

        }
        else if (hoverLogout)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Logout. \nClick on this to log out and end this Session.";

        }
    }
	
	
	private void LevelSelect() //StartGame()
	{
		Debug.Log("Level Select Screen");
		//Debug.Log("Starting Game");
		//DontDestroyOnLoad(GameState.Instance);
		//GameState.Instance.StartState();
	}
}
