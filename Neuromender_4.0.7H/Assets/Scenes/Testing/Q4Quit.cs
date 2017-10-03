using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Script for changing the scene. This script will automatically send data (if there is any) if there is a data logger in the current scene.
/// </summary>
public class Q4Quit : MonoBehaviour
{
    [Tooltip("The key that must be pressed to change the scene.")]
    public KeyCode initiationKey = KeyCode.Escape;

    [Tooltip("The key that must be pressed to confirm the exit.")]
    public KeyCode confirmationKey = KeyCode.Space;

    [Tooltip("The key that must be pressed to undo the exit.")]
    public KeyCode denialKey = KeyCode.Return;

    [Tooltip("The scene that the application will open after the initiation key is pressed.")]
    public string destinationScene = "";

    [Tooltip("The text that shold always be onscreen to show that this option is available.")]
    public Text promptText = null;

    [Tooltip("TRUE: Exiting with this key requires a second confirmation key press.\nFALSE: No confirmation is needed. Confirmation key press check is circumvented.")]
    public bool needsConfirmation = false;

    [Tooltip("The notification displayed when the key is pressed (if one is required).")]
    public GameObject confirmNotificationObject = null;

    [Tooltip("Text associated with the displayed notification.")]
    public Text notificationText = null;

    private DataLogger_Base logger = null;  // Local handle for the data logger that might be in this scene (if this script is exiting from a game scene).
    private bool confirmFlag = false;       // TRUE: Scene change requested. Awaiting confirmation (if required), else change the scene immediately.
    public bool logData = false;           // TRUE: The current game must have its data recorded. FALSE: The current game doe not need its data recorded.

    // Use this for initialization
    void Start()
    {
        if(GameObject.Find("DataLogger") != null)
            logger = GameObject.Find("DataLogger").GetComponent<DataLogger_Base>();

        if(promptText == null)
        {
            Debug.Log("There is no prompt text associated with this component. RECTIFY THIS!!");
            enabled = false;
        }
        else
        {
            promptText.text = "Press " + initiationKey.ToString() + " to exit.";
        }

        // Initially, this notification should be hidden. Ensure this is the case.
        if(confirmNotificationObject != null)
            confirmNotificationObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If confirmation is required and the key is pressed, or if NO confirmation is required, serialise the data and change scene.
        if (confirmFlag)
        {
            if (Input.GetKeyDown(confirmationKey) || !needsConfirmation)
            {
                if(logData)
                {
                    // Serialise the game data BEFORE changing scene.
                    if (logger != null)
                        logger.SerialiseData();
                }

                // Load the next scene.
                SceneManager.LoadScene(destinationScene);
            }
            else
            if (Input.GetKeyDown(denialKey))
            {
                UndoExitRequest();
            }
        }
            

        // When the key is pressed, change the scene. This should be AFTER the confirm check because GetKeyDown seems to fire both times when in the opposite order.
        if (Input.GetKeyDown(initiationKey))
        {
            if (confirmNotificationObject != null && notificationText != null)
            {
                confirmNotificationObject.SetActive(true);
                notificationText.text = "Are you sure? Press " + confirmationKey.ToString() + " to exit. Press " + denialKey.ToString() + " to undo.";

                if(logData)
                    notificationText.text += " This will be recorded.";
            }

            // Raise the confirm request flag, to activate the logic for 
            confirmFlag = true;
        }
    }

    /// <summary>
    /// Specify that the data recorded during this session is to be recorded, even though it has been exited prematurely.
    /// </summary>
    public void SetSessionAsRecorded()
    {
        // Add an entry in the data logger to say that the current session has been only partially completed.
        logger.DATA.Add("PartialDataRecorded", true);

        // Raise the log data flag.
        logData = true;
    }

    /// <summary>
    /// In case an exit was not wanted and the denial key was pressed, undo everything so that the initial behaviour happens next time.
    /// </summary>
    void UndoExitRequest()
    {
        confirmFlag = false;

        confirmNotificationObject.SetActive(false);
    }
}
