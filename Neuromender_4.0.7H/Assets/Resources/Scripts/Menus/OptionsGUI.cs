using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OptionsGUI : MonoBehaviour
{
	private float btnWide = 0.3f;
	private float btnHigh = 0.08f;
	
	private float left;
	private float top;
	private float width;
	private float height;
	private float spacer;

	private string menu = "options";
	
	private LoginControl userConfig;
	private GameObject bg1;
    private GameObject bg2;
	public bool debugData;
	public GUISkin mainMenuSkin;

    private bool hoverDebugOn = false;
    private bool hoverDebugOff = false;
    private bool hoverHowTo = false;
    private bool hoverCredits = false;
    private bool hoverBack = false;

    private GameObject levelSelectPanel;

    void Start()
    {
        if (GameObject.Find("DatabaseController"))
        {
            userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        }
		bg1 = GameObject.Find ("BackGround1");
        bg1.SetActive(false);
        bg2 = GameObject.Find("HowToVideo");
        bg2.SetActive(false);
		//bg3 = GameObject.Find ("BackGround3");
		//SetBG (1);
		debugData = false;
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

        if (bg2.GetComponent<PlayVideo>().videoPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bg2.GetComponent<PlayVideo>().StopHowToVideo();
                SetBG(2);
            }
        }

        hoverTextPopulator();
	}

	public void SetBG(int bg)
	{
		if (bg == 1) {
            bg1.SetActive(!bg1.activeSelf);
		}
        else if (bg == 2)
        {
            bg2.SetActive(!bg2.activeSelf);
        }
        else {
			print("Error BG");
		}
	}
	
	void OnGUI()
	{
		GUI.skin = mainMenuSkin;
		mainMenuSkin.button.fontSize = ( int )(height * 0.75f);
		mainMenuSkin.box.fontSize = ( int )(height * 0.75f);

		if (menu == "options") {
			//SetBG (1);
            if (!bg1.activeSelf && !bg2.GetComponent<PlayVideo>().videoPlaying)
            {
                bg2.SetActive(false);
                if (GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.showDebug == false)
                {
                    if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Turn On Debug Data", "DebugOn")))
                    {
                        debugData = true;
                        GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.showDebug = true;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Turn Off Debug Data", "DebugOff")))
                    {
                        debugData = false;
                        GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.showDebug = false;
                    }
                }

                if (GUI.Button(new Rect(left, -1 * (spacer + height) + top, width, height), new GUIContent("How To Play", "HowTo")))
                {
                    //menu = "howtoplay";
                    SetBG(2);
                    hoverDebugOn = false;
                    hoverDebugOff = false;
                    hoverHowTo = false;
                    hoverCredits = false;
                    hoverBack = false;
                    levelSelectPanel.SetActive(false);
                    bg2.GetComponent<PlayVideo>().PlayHowToVideo();
                }

                if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), new GUIContent("Show Credits", "Credits")))
                {
                    SetBG(1);
                    hoverDebugOn = false;
                    hoverDebugOff = false;
                    hoverHowTo = false;
                    hoverCredits = false;
                    hoverBack = false;
                    levelSelectPanel.SetActive(false);
                    //menu = "credits";
                }

                if (GUI.Button(new Rect(left, 2 * (spacer + height) + top, width, height), new GUIContent("Back", "Back")))
                {
                    //Debug.Log("Exit button clicked");
                    Debug.Log("Back button clicked");

                    SceneManager.LoadScene("MainMenu");
                }
            }
            else if (bg1.activeSelf)
            {

                if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), "Hide Credits"))
                {
                    SetBG(1);
                    //menu = "credits";
                }
            }
            var hover = GUI.tooltip;

            if (hover == "DebugOn")
            {
                //print("Forest (Medium Detail)");
                hoverDebugOn = true;
                hoverDebugOff = false;
                hoverHowTo = false;
                hoverCredits = false;
                hoverBack = false;
            }
            else if (hover == "DebugOff")
            {
                //print("Beach (Low Detail)");
                hoverDebugOn = false;
                hoverDebugOff = true;
                hoverHowTo = false;
                hoverCredits = false;
                hoverBack = false;
            }
            else if (hover == "HowTo")
            {
                //print("Beach (Low Detail)");
                hoverDebugOn = false;
                hoverDebugOff = false;
                hoverHowTo = true;
                hoverCredits = false;
                hoverBack = false;
            }
            else if (hover == "Credits")
            {
                //print("Beach (Low Detail)");
                hoverDebugOn = false;
                hoverDebugOff = false;
                hoverHowTo = false;
                hoverCredits = true;
                hoverBack = false;
            }
            else if (hover == "Back")
            {
                //print("Beach (Low Detail)");
                hoverDebugOn = false;
                hoverDebugOff = false;
                hoverHowTo = false;
                hoverCredits = false;
                hoverBack = true;
            }
		} 
	}

    public void hoverTextPopulator()
    {
        if (hoverDebugOn)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Debug On. \nThis will show the debug panel in the level.";

        }
        else if (hoverDebugOff)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Debug Off. \nThis will hide the debug panel in the level.";

        }
        else if (hoverHowTo)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "How To Video. \nClick on this to view the Tutorial Video.";

        }
        else if (hoverCredits)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Credits. \nClick on this to view the credits.";

        }
        else if (hoverBack)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Back. \nClick on this to go back to the main menu.";

        }
    }
}
