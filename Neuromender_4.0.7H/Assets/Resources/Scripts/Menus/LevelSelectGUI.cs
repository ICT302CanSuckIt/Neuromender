using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectGUI : MonoBehaviour
{
	private float btnWide = 0.3f;
	private float btnHigh = 0.08f;
	
	private float left;
	private float top;
	private float width;
	private float height;
	private float spacer;

	private string menu = "levelselect";
	
	private LoginControl userConfig;
	private GameObject bg1;
	private GameObject bg2;
	private GameObject bg3;
    private GameObject titlePanel;

    private Text TitleText;
	public GUISkin mainMenuSkin;

    public Transform CameraLocationBeach;
    public Transform CameraLocationForest;
    public Transform CameraLocationTemple;
    public Transform CameraLocationStart;

    private bool hoverBeach = false;
    private bool hoverForest = false;
    private bool hoverTemple = false;
    private bool gotoStart = false;
    private bool hoverHowTo = false;
    private bool hoverBack = false;
    private bool hoverElbowRaise = false;
    private bool hoverBackMain = false;

    private bool mouseOverSlow = false;
    private bool mouseOverMedium = false;
    private bool mouseOverFast = false;

    private GameObject levelSelectPanel;

    void Start()
    {
        if (GameObject.Find("DatabaseController"))
        {
            userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        }

        TitleText = GameObject.Find("Title").GetComponent<Text>();
        levelSelectPanel = GameObject.Find("LevelSelectPanel");
        titlePanel = GameObject.Find("TitlePanel");
        levelSelectPanel.SetActive(false);
        bg1 = GameObject.Find("HowToVideo");
        bg1.SetActive(false);
		//bg2 = GameObject.Find ("BackGround2");
		//bg3 = GameObject.Find ("BackGround3");
		//SetBG (1);
    }
	void Update()
	{
		left = Screen.width * (1 - btnWide) * 0.5f;
		top = Screen.height * (1 - btnHigh) * 0.5f;
		
		width = Screen.width * btnWide;
		height = Screen.height * btnHigh;
		
		spacer = 0.5f * height;

        if (bg1.GetComponent<PlayVideo>().videoPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bg1.GetComponent<PlayVideo>().StopHowToVideo();
                titlePanel.SetActive(true);
                ToggleBG(1);
                
            }
        }

        handleCameraLocation();
	}


    public void handleCameraLocation()
    {
        GameObject main = GameObject.Find("Camera");

        if(hoverElbowRaise)
        {
            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Elbow Raise. \nThis task has the player jumping from the side of a mountain and wingsuiting through a set of rings. \nThe aim is for accuracy and control in the motion of raising the elbow away from the body.";
                
        }
        else if(hoverBackMain)
        {
            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Back to Main Menu.";
                
        }
        else if (hoverBeach)
        {
           // print("Beach (Low Detail)");
            if (CameraLocationBeach)
            {
                if (main.transform.position != CameraLocationBeach.position)
                {
                    main.transform.position = CameraLocationBeach.position;
                    main.transform.rotation = CameraLocationBeach.rotation;
                    levelSelectPanel.SetActive(true);
                    GameObject.Find("LevelSelection").GetComponent<Text>().text = "Beach Level. \nThis is the straightest path with the lowest level of detail. \nIt is meant to be used as an introductory level for new users who might not be able to take as much detail overload.";
                }
            }
        }
        else if (hoverForest)
        {
            //print("Forest (Medium Detail)");

            if (CameraLocationForest)
            {
                Vector3 pos = CameraLocationForest.position;
                Vector3 camPos = main.transform.position;
                if (camPos != pos)
                {
                    main.transform.position = pos;
                    main.transform.rotation = CameraLocationForest.rotation;
                    levelSelectPanel.SetActive(true);
                    GameObject.Find("LevelSelection").GetComponent<Text>().text = "Forest Level. \nThis level has a medium level of detail and is less straight that the beach. It is aimed at someone who has more intermediate knowledge of the system and can handle more in scene.";
                }
            }
        }
        else if (hoverTemple)
        {
           // print("Temple (High Detail)");
            if (CameraLocationTemple)
            {
                if (main.transform.position != CameraLocationTemple.position)
                {
                    main.transform.position = CameraLocationTemple.position;
                    main.transform.rotation = CameraLocationTemple.rotation;
                    levelSelectPanel.SetActive(true);
                    GameObject.Find("LevelSelection").GetComponent<Text>().text = "Temple Level. \nThis level is the highest level of detail and with the longest path. It has a high visual overload and would require someone who is used to the feel of the game.";
                }
            }
        }
        else if (hoverHowTo)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "View How To Play. \nBrief tutorial on how to interact with this Neuromend task.";

        }
        else if (hoverBack)
        {
            // print("Temple (High Detail)");

            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Go back to Task Select menu.";

        }
        else if(gotoStart)
        {
             if (main.transform.position != CameraLocationStart.position)
            {
                main.transform.position = CameraLocationStart.position;
                main.transform.rotation = CameraLocationStart.rotation;
                levelSelectPanel.SetActive(false);
            }
        }

        if(mouseOverSlow)
        {
            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Slowest speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackSlow + " seconds.";
                
        }
        else if(mouseOverMedium)
        {
            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Medium speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackMedium + " seconds.";
                
        }
        else if (mouseOverFast)
        {
            levelSelectPanel.SetActive(true);
            GameObject.Find("LevelSelection").GetComponent<Text>().text = "Fastest speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackFast + " seconds.";
        }
    }

	public void ToggleBG(int bg)
	{
		if (bg == 1) {
			bg1.SetActive (!bg1.activeSelf);
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

		if (menu == "levelselect") {
			//SetBG(1);
			if (GUI.Button (new Rect (left, -2 * (spacer + height) + top, width, height), new GUIContent("Elbow Raise", "Elbow"))) {
				Debug.Log ("Level One selected");
				menu = "level1_tute";
                TitleText.text = "View Tutorial";
			}
			
			if (GUI.Button (new Rect (left, 2 * (spacer + height) + top, width, height), new GUIContent("Back", "Back"))) {	
				//Debug.Log("Exit button clicked");
				Debug.Log ("Back button clicked");

				SceneManager.LoadScene("MainMenu");
			}

            var hover = GUI.tooltip;

            if (hover == "Elbow")
            {
                //print("Forest (Medium Detail)");
                hoverElbowRaise = true;
                hoverBackMain = false;
            }
            else if (hover == "Back")
            {
                //print("Forest (Medium Detail)");
                hoverElbowRaise = false;
                hoverBackMain = true;
            }
		}

        else if (menu == "level1_tute")
        {
            if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Yes", "Yes")))
            {
                Debug.Log("Yes View tutorial selected");
                menu = "level1";
                levelSelectPanel.SetActive(false);
                ToggleBG(1);
                bg1.GetComponent<PlayVideo>().PlayHowToVideo();
                titlePanel.SetActive(false);
                hoverElbowRaise = false;
                TitleText.text = "Select Track";
            }
            if (GUI.Button(new Rect(left, -1 * (spacer + height) + top, width, height), new GUIContent("No", "No")))
            {
                Debug.Log("No Dont View Tutorial selected");
                menu = "level1";
                hoverElbowRaise = false;
                TitleText.text = "Select Track";
            }
            if (GUI.Button(new Rect(left, 2 * (spacer + height) + top, width, height), "Back"))
            {
                //Debug.Log("Exit button clicked");
                Debug.Log("Back button clicked");
                menu = "levelselect";
                TitleText.text = "Select Task";
            }
        }

        else if (menu == "level1" && !bg1.GetComponent<PlayVideo>().videoPlaying)
        {
            bg1.SetActive(false);
           // GameObject.Find("TitlePanel").SetActive(true);
            if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Beach (Low Detail)", "Beach")))
            {
                Debug.Log("Level One A selected");
                menu = "speedselect";
                TitleText.text = "Select Speed";
                userConfig.selectedTrack = TrackName.beach;
            }
            if (GUI.Button(new Rect(left, -1 * (spacer + height) + top, width, height), new GUIContent("Forest (Medium Detail)", "Forest")))
            {
				Debug.Log ("Level One B selected");
				menu = "speedselect";
                TitleText.text = "Select Speed";
				userConfig.selectedTrack = TrackName.forest;
				//SpeedSelect();
			}
            if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), new GUIContent("Temple (High Detail)", "Temple")))
            {
				Debug.Log ("Level One C selected");
                menu = "speedselect";
                TitleText.text = "Select Speed";
				userConfig.selectedTrack = TrackName.temple;
            }
            if (GUI.Button(new Rect(left, 1 * (spacer + height) + top, width, height), new GUIContent("How To Play", "HowTo")))
            {
                Debug.Log("How To Selected");
                levelSelectPanel.SetActive(false);
                ToggleBG(1);
                bg1.GetComponent<PlayVideo>().PlayHowToVideo();
                titlePanel.SetActive(false);
                hoverHowTo = false;

                //menu = "speedselect";
                //userConfig.selectedTrack = TrackName.track3;
            }	
			if (GUI.Button (new Rect (left, 2 * (spacer + height) + top, width, height), new GUIContent("Back", "Back"))) {
				Debug.Log ("Back button clicked");
                menu = "levelselect";
                TitleText.text = "Select Task";
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = true;
                hoverBack = false;
				//LevelSelect ();
			}


            var hover = GUI.tooltip;

            if (hover == "Forest")
            {
                //print("Forest (Medium Detail)");
                hoverBeach = false;
                hoverForest = true;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }
            else if (hover == "Beach")
            {
                //print("Beach (Low Detail)");
                hoverBeach = true;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }
            else if (hover == "Temple")
            {
                //print("Temple (High Detail)");
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = true;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }
            else if (hover == "HowTo")
            {
                //print("Temple (High Detail)");
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = true;
                hoverBack = false;
            }
            else if (hover == "Back")
            {
                //print("Temple (High Detail)");
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = true;
            }
		}


        else if (menu == "speedselect")
        {
			//SetBG(3);
            if (GUI.Button(new Rect(left, -2 * (spacer + height) + top, width, height), new GUIContent("Slow", "Slow")))
            {
				Debug.Log ("Slow selected");
				userConfig.selectedSpeed = SpeedLevel.slow;
                GoToLevel("TestNeuromend3_CMaster");
			}

            if (GUI.Button(new Rect(left, -1 * (spacer + height) + top, width, height), new GUIContent("Medium", "Medium")))
            {
				Debug.Log ("Medium selected");
				userConfig.selectedSpeed = SpeedLevel.medium;
                GoToLevel("TestNeuromend3_CMaster");
			}

            if (GUI.Button(new Rect(left, 0 * (spacer + height) + top, width, height), new GUIContent("Fast", "Fast")))
            {
				Debug.Log ("Fast selected");
				userConfig.selectedSpeed = SpeedLevel.fast;
                GoToLevel("TestNeuromend3_CMaster");
			}

            if (GUI.Button(new Rect(left, 2 * (spacer + height) + top, width, height), new GUIContent("Back", "Back")))
            {
				Debug.Log ("Back button clicked");
				menu = "level1";
                TitleText.text = "Select Track";
                mouseOverSlow = false;
                mouseOverMedium = false;
                mouseOverFast = false;
			}

            var hover = GUI.tooltip;

            if (hover == "Slow")
            {
                mouseOverSlow = true;
                mouseOverMedium = false;
                mouseOverFast = false;
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }
            else if (hover == "Medium")
            {
                mouseOverSlow = false;
                mouseOverMedium = true;
                mouseOverFast = false;
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }
            else if (hover == "Fast")
            {
                mouseOverSlow = false;
                mouseOverMedium = false;
                mouseOverFast = true;
                hoverBeach = false;
                hoverForest = false;
                hoverTemple = false;
                gotoStart = false;
                hoverHowTo = false;
                hoverBack = false;
            }

		}
	}
	
	private void GoToLevel(string levelName)
	{
		Debug.Log ("GoToLevel");

        // KARL, not opening this whole method
        SceneManager.LoadScene(levelName);
	}
}
