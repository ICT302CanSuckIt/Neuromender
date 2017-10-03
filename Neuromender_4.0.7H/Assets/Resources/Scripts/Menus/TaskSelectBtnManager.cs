using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class TaskSelectBtnManager : MonoBehaviour {

	public string menu = "";
	private string SelectedTask;
	
	private LoginControl userConfig;
	public GameObject bg1;
    public GameObject armExtVideo;
    public GameObject header;
	public GameObject levelSelectPanel;
	public GameObject menuControl;

	public GameObject canvas;

	private bool hoverBeach = false;
	private bool hoverForest = false;
	private bool hoverTemple = false;
	private bool hoverHowTo = false;
	private bool hoverBack = false;
	private bool hoverElbowRaise = false;
	private bool hoverBackMain = false;
	private bool HoverArmExtension = false;
    private bool HoverArmExtButton = false;
    private bool HoverRow = false;
    private bool HoverPractice = false;

    private bool mouseOverSlow = false;
	private bool mouseOverMedium = false;
	private bool mouseOverFast = false;
	


	void Start()
	{
		if (GameObject.Find("DatabaseController"))
		{
			userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
		}

		header.SetActive(false);
		menuControl = GameObject.Find("TaskSelectCanvas");
        bg1.SetActive(false);
        armExtVideo.SetActive(false);
	    levelSelectPanel.SetActive(false);
	}

	void Update()
	{
       // Debug.Log(levelSelectPanel.activeSelf);
		if (bg1.GetComponent<PlayVideo> ().videoPlaying) 
		{
			if (Input.GetKeyDown (KeyCode.Escape))
            {
                bg1.GetComponent<PlayVideo>().StopHowToVideo();
                ToggleBG(1);
                canvas.SetActive(true);
			}
			canvas.SetActive (false);
			levelSelectPanel.SetActive (false);
		} 
		else 
		{
			canvas.SetActive(true);
		}

        if (armExtVideo.GetComponent<PlayVideo>().videoPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                armExtVideo.GetComponent<PlayVideo>().StopHowToVideo();
                ToggleBG(3);
                canvas.SetActive(true);

            }
            canvas.SetActive(false);
            levelSelectPanel.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
        }


        HoverTextPopulator();

		menu = GetComponent<OptionsBtnManager> ().menu;

		if (bg1.GetComponent<PlayVideo> ().stopped == true && menu == "ArmRaiseTut") //checks if video has stopped. If video has stopped then proceed to next screen.
		{
			this.GetComponent<OptionsBtnManager> ().menu = "LevelSelect";
			header.SetActive(true);
		}

        if (armExtVideo.GetComponent<PlayVideo>().stopped == true && menu == "ArmExtTut") //checks if video has stopped. If video has stopped then proceed to next screen.
        {
            this.GetComponent<OptionsBtnManager>().menu = "ArmExtension";
            header.SetActive(true);
        }

        if (menu == "Main") 
		{
			header.SetActive(false);

		}

		if (menu == "TaskSelect") 
		{
			header.SetActive(true);
			header.GetComponentInChildren<Text>().text = "Choose task:";
			foreach (Transform child in menuControl.transform) 
			{
				if(child.tag == "TaskSelect")
				{
					child.gameObject.SetActive (true);
				}
			}
		} 
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if(child.tag == "TaskSelect")
				{
					child.gameObject.SetActive (false);
				}
			}
		}

		if (menu == "ViewArmRaiseTut") 
		{
			header.GetComponentInChildren<Text>().text = "View Tutorial?";
			foreach (Transform child in menuControl.transform) 
			{
				if(child.tag != "ArmRaiseTut")
				{
					child.gameObject.SetActive (false);
				}
				else
				{
					child.gameObject.SetActive (true);
				}

			}
		}
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if (child.tag == "ArmRaiseTut") 
				{
					child.gameObject.SetActive (false);
				} 
			}
		}

        if (menu == "ViewArmExtTut")
        {
            header.GetComponentInChildren<Text>().text = "View Tutorial?";
            foreach (Transform child in menuControl.transform)
            {
                if (child.tag != "ArmExtTut")
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }

            }
        }
        else
        {
            foreach (Transform child in menuControl.transform)
            {
                if (child.tag == "ArmExtTut")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }


        if (menu == "LevelSelect") 
		{
			header.GetComponentInChildren<Text>().text = "Select Level:";
			foreach (Transform child in menuControl.transform) 
			{
				if (child.tag != "LevelSelect") 
				{
					child.gameObject.SetActive (false);
				} 
				else 
				{
					child.gameObject.SetActive (true);
				}
			}
		} 
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if (child.tag == "LevelSelect") 
				{
					child.gameObject.SetActive (false);
				} 
			}
		}

		if (menu == "SpeedSelect") 
		{
			header.GetComponentInChildren<Text>().text = "Select Speed:";
			foreach (Transform child in menuControl.transform) 
			{
				if(child.tag != "SpeedSelect")
				{
					child.gameObject.SetActive (false);
				}
				else
				{
					child.gameObject.SetActive (true);
				}
			}
		}
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if (child.tag == "SpeedSelect") 
				{
					child.gameObject.SetActive (false);
				} 
			}
		}

		if (menu == "ArmExtension") 
		{
			header.GetComponentInChildren<Text>().text = "Select version: ";
			foreach (Transform child in menuControl.transform) 
			{
				if(child.tag != "ArmExtension")
				{
					child.gameObject.SetActive (false);
				}
				else
				{
					child.gameObject.SetActive (true);
				}
			}
		}
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if (child.tag == "ArmExtension") 
				{
					child.gameObject.SetActive (false);
				} 
			}
		}
	}
	
	public void ToggleBG(int bg)
	{
		if (bg == 1) {
			bg1.SetActive (!bg1.activeSelf);
		} 
		else
            if(bg == 3)
            {
                armExtVideo.SetActive(!armExtVideo.activeSelf);
		    }
        else
        {
            print("Error BG");
        }
	}

	public void ElbowRaise()
	{
		SelectedTask = "TestNeuromend3_CMaster";
		this.GetComponent<OptionsBtnManager> ().menu = "ViewArmRaiseTut";
	}

    public void ArmExtension()
	{
		SelectedTask = "ArmExtension";
        this.GetComponent<OptionsBtnManager>().menu = "ViewArmExtTut";
    }

	public void ArmExtensionReturn()
	{
		this.GetComponent<OptionsBtnManager> ().menu = "TaskSelect";
	}

	public void Practice()
	{
        SceneManager.LoadScene("ArmExtension");
	}

	public void Targets ()
	{
        SceneManager.LoadScene("Targets");
	}

	public void Rowing()
	{
        SceneManager.LoadScene("Rowing");
	}


	public void ReturnToMenu()
	{
		this.GetComponent<OptionsBtnManager> ().menu = "Main";
		
	}

	public void ReturnToSelection()
	{
		this.GetComponent<OptionsBtnManager> ().menu = "TaskSelect";
        bg1.GetComponent<PlayVideo>().stopped = false;
        bg1.GetComponent<PlayVideo>().videoPlaying = false;
        armExtVideo.GetComponent<PlayVideo>().stopped = false;
        armExtVideo.GetComponent<PlayVideo>().videoPlaying = false;
    }

	public void ReturnToLevel()
	{
		this.GetComponent<OptionsBtnManager> ().menu = "LevelSelect";
	}

	public void Beach() 
	{
		Debug.Log("Level One A selected");
		this.GetComponent<OptionsBtnManager> ().menu = "SpeedSelect";
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.beach;
	}

	public void Forest() 
	{
		Debug.Log ("Level One B selected");
		this.GetComponent<OptionsBtnManager> ().menu = "SpeedSelect";
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.forest;
	}

	public void Temple() 
	{
		Debug.Log ("Level One C selected");
		this.GetComponent<OptionsBtnManager> ().menu = "SpeedSelect";
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.temple;
	}

	public void TutYes()
	{
		Debug.Log("Yes view tutorial");
		
		this.GetComponent<OptionsBtnManager> ().menu = "ArmRaiseTut";
		header.SetActive (false);
		ToggleBG(1);
		bg1.GetComponent<PlayVideo>().PlayHowToVideo();
		hoverHowTo = false;
		HoverArmExtension = false;
        levelSelectPanel.SetActive(false);
    }

    public void ArmExtTutYes()
    {
        Debug.Log("Yes view tutorial");
        levelSelectPanel.SetActive(false);
        this.GetComponent<OptionsBtnManager>().menu = "ArmExtTut";
        header.SetActive(false);
        ToggleBG(3);
        armExtVideo.GetComponent<PlayVideo>().PlayHowToVideo();
        hoverHowTo = false;
        HoverArmExtension = false;
    }

    public void TutNo()
	{
        this.GetComponent<OptionsBtnManager>().menu = "LevelSelect";
        Debug.Log("No Dont View Tutorial selected");
        hoverElbowRaise = false;
        HoverArmExtension = false;
    }

    public void ArmExtTutNo()
    {
        this.GetComponent<OptionsBtnManager>().menu = "ArmExtension";

        Debug.Log("No Dont View Tutorial selected");
        hoverElbowRaise = false;
        HoverArmExtension = false;
    }

    public void HoverBeach()
	{
		hoverBeach = true;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverForest()
	{
		hoverBeach = false;
		hoverForest = true;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}
	public void HoverTemple()
	{
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = true;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverHowTo()
	{
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = true;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverBack()
	{
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = true;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverSlow()
	{
		mouseOverSlow = true;
		mouseOverMedium = false;
		mouseOverFast = false;
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverMedium()
	{
		mouseOverSlow = false;
		mouseOverMedium = true;
		mouseOverFast = false;
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverFast()
	{
		mouseOverSlow = false;
		mouseOverMedium = false;
		mouseOverFast = true;
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverArmExt()
	{
		mouseOverSlow = false;
		mouseOverMedium = false;
		mouseOverFast = false;
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = true;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
	}

	public void HoverElbRaise()
	{
		mouseOverSlow = false;
		mouseOverMedium = false;
		mouseOverFast = false;
		hoverBeach = false;
		hoverForest = false;
		hoverTemple = false;
		hoverHowTo = false;
		hoverBack = false;
		HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = true;
	}

    public void HoverArmExtButtonFunction()
    {
        mouseOverSlow = false;
        mouseOverMedium = false;
        mouseOverFast = false;
        hoverBeach = false;
        hoverForest = false;
        hoverTemple = false;
        hoverHowTo = false;
        hoverBack = false;
        HoverArmExtension = false;
        HoverArmExtButton = true;
        HoverRow = false;
        HoverPractice = false;
        hoverElbowRaise = false;
    }

    public void HoverRowFunction()
    {
        mouseOverSlow = false;
        mouseOverMedium = false;
        mouseOverFast = false;
        hoverBeach = false;
        hoverForest = false;
        hoverTemple = false;
        hoverHowTo = false;
        hoverBack = false;
        HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = true;
        HoverPractice = false;
        hoverElbowRaise = false;
    }

    public void HoverPracticeFunction()
    {
        mouseOverSlow = false;
        mouseOverMedium = false;
        mouseOverFast = false;
        hoverBeach = false;
        hoverForest = false;
        hoverTemple = false;
        hoverHowTo = false;
        hoverBack = false;
        HoverArmExtension = false;
        HoverArmExtButton = false;
        HoverRow = false;
        HoverPractice = true;
        hoverElbowRaise = false;
    }


    public void Slow() 
	{
		Debug.Log ("Slow selected");
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedSpeed = SpeedLevel.slow;
        SceneManager.LoadScene("TestNeuromend3_CMaster");
	}

	public void Medium() 
	{
		Debug.Log ("Medium selected");
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedSpeed = SpeedLevel.medium;
        SceneManager.LoadScene("TestNeuromend3_CMaster");
	}

	public void Fast() 
	{
		Debug.Log ("Fast selected");
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedSpeed = SpeedLevel.fast;
        SceneManager.LoadScene("TestNeuromend3_CMaster");
	}


    public void StopElbowRaiseHovering()
    {
        // A bug in the code keeps hoverElbowRaise set to true even when the button is set to inactive. I didn't have time to fix it properly (I.E without using this function as this works) before the due date so you'll have to do it, sorry. - Michael Vatskalis.

        hoverElbowRaise = false;

    }


	void HoverTextPopulator()
	{
		if (hoverElbowRaise) 
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Elbow Raise. \nThis task has the player jumping from the side of a mountain and wingsuiting through a set of rings. \nThe aim is for accuracy and control in the motion of raising the elbow away from the body.";
		}

		if (HoverArmExtension) 
		{
			levelSelectPanel.SetActive(true);
            levelSelectPanel.GetComponentInChildren<Text>().text = "Arm Extension. \nThis aims for accuracy and distance in the extension of the player's arm away from their body.";
        }

        if (HoverRow)
        {
            levelSelectPanel.SetActive(true);
            levelSelectPanel.GetComponentInChildren<Text>().text = "Rowing: \nThis task has the player control a rowboat using the forward and back motion of their arm.";
        }

        if (HoverArmExtButton)
        {
            levelSelectPanel.SetActive(true);
            levelSelectPanel.GetComponentInChildren<Text>().text = "Targets: \nThis task has the player sequentially reach out to touch a number of targets.";
        }

        if (HoverPractice)
        {
            levelSelectPanel.SetActive(true);
            levelSelectPanel.GetComponentInChildren<Text>().text = "Practice: \nNot for rehabilitation purposes - non-functional";
        }

        if (hoverBackMain) 
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Back to Main Menu.";
		}

		if (mouseOverSlow) 
		{
			levelSelectPanel.SetActive (true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Slowest speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackSlow + " seconds.";
		}

		if (mouseOverMedium)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Medium speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackMedium + " seconds.";
			
		}
		if (mouseOverFast)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Fastest speed as defined in User config in the Neuromend Web Portal.\nDuration: " + userConfig.config.trackFast + " seconds.";

		}
		if (hoverBeach)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Beach Level. \nThis is the straightest path with the lowest level of detail. \nIt is meant to be used as an introductory level for new users who might not be able to take as much detail overload.";
		}
		if (hoverForest)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Forest Level. \nThis level has a medium level of detail and is less straight that the beach. It is aimed at someone who has more intermediate knowledge of the system and can handle more in scene.";
		}
		if (hoverTemple)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Temple Level. \nThis level is the highest level of detail and with the longest path. It has a high visual overload and would require someone who is used to the feel of the game.";
		}
		if (hoverBack)
		{
			levelSelectPanel.SetActive(true);
			levelSelectPanel.GetComponentInChildren<Text>().text = "Go back to Task Select menu.";
		}


	}



}
