using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Module for adding a tooltip display for an object in-game.
/// 
/// <para>For more information, see the script.</para>
/// </summary>
/// 
/// <remarks>
/// 
/// Currently the HoverText component is hard-coded to use the main menu's instance to get a gameobject for displaying the description.
/// I would change it, but as I have said in other scripts, I have too many things to get done is a short period of time. Be aware of it
/// in any case.
/// 
/// Adding a tooltip to an object is relatively straight forward. I am assuming this is only for a new menu button in the MainMenu scene.
/// 
/// 1)  Add an EventTrigger and HoverText component to a new button object.
/// 
/// 2)  Write the text you wish to be displayed in the HoverText entry.
/// 
/// 3)  If there is an extra param you wish to display, then add a new enumeration entry in EXTRA_HOVER_DAT. In the Refactortext()
///     function add a new case which replaces the key '###' with the value that you wish to add. See other cases for examples.
///     
/// 4)  If this text needs to update constantly, check the constantTextUpdate box in the UnityInspector, or via another script.
///     See the case for DEBUG_MODE in Refactor code for the extra necessary information that might be required.
/// 
/// 5)  Add two event triggers to the EventTrigger component: PointerEnter and PointerExit. These will show / hide the description text.
/// 
/// 6)  For setting the function calls see other buttons as examples. Note that the object param is the object that this HoverText component
///     is attached to.
/// 
/// That should be it :D
/// 
/// </remarks>
public class HoverText : MonoBehaviour {

    /// <summary>The text that will be displayed on-screen when the associated GameObject is hovered over.</summary>
    [Tooltip("The text that will be displayed on-screen when the associated GameObject is hovered over.")]
    [TextArea(2, 10)]
    public string hoverText = "";

    /// <summary>Enumeration values should represent a specific piece of extra information from another script.</summary>
    public enum EXTRA_HOVER_DAT { NONE, FAST_WINGMAN_SPD, MED_WINGMAN_SPD, SLOW_WINGMAN_SPD, SHORT_CYCLING_DIS, MED_CYCLING_DIS, LONG_CYCLING_DIS, DEBUG_MODE }

    /// <summary>The specific value that should be inserted into the Hover Text.
    /// <para>Write '###' to include a valid variable specified by this param in the hover text.</para></summary>
    [Tooltip("The specific value that should be inserted into the Hover Text.\nWrite '###' to include a valid variable specified by this param in the hover text.")]
    public EXTRA_HOVER_DAT extraData = EXTRA_HOVER_DAT.NONE;

    /// <summary>TRUE: Constantly update the description object that is displaying this hover text description.</summary>
    [Tooltip("TRUE: Constantly update the description object that is displaying this hover text description.")]
    public bool constantTextUpdate = false;

    private bool hoveredOver = false;   // TRUE: This object is currently being hovered over.

    private LoginControl login = null;  // Local handle for accessing external game data.

    // Use this for initialization
    void Start () {

        if (GameObject.Find("DatabaseController") != null)
        {
            login = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
            RefactorText();
        }
        else
            enabled = false;
	}

    void Update()
    {
        // If this hover text needs a constant update, do so (but only when it is being hovered over...)
        if (constantTextUpdate && hoveredOver)
        {
            RefactorText();
            MainMenuController.Instance.HoverDescription.GetComponentInChildren<Text>().text = hoverText;
        }
    }

    // If there is extra data, insert it anywhere there is the key '###'.
    private void RefactorText()
    {
        switch (extraData)
        {
            case EXTRA_HOVER_DAT.FAST_WINGMAN_SPD:
                hoverText = hoverText.Replace("###", login.config.trackFast.ToString());
                break;
            case EXTRA_HOVER_DAT.MED_WINGMAN_SPD:
                hoverText = hoverText.Replace("###", login.config.trackMedium.ToString());
                break;
            case EXTRA_HOVER_DAT.SLOW_WINGMAN_SPD:
                hoverText = hoverText.Replace("###", login.config.trackSlow.ToString());
                break;
            case EXTRA_HOVER_DAT.LONG_CYCLING_DIS:
                hoverText = hoverText.Replace("###", login.config.DistanceLong.ToString());
                break;
            case EXTRA_HOVER_DAT.MED_CYCLING_DIS:
                hoverText = hoverText.Replace("###", login.config.DistanceMedium.ToString());
                break;
            case EXTRA_HOVER_DAT.SHORT_CYCLING_DIS:
                hoverText = hoverText.Replace("###", login.config.DistanceShort.ToString());
                break;
            case EXTRA_HOVER_DAT.DEBUG_MODE:
                hoverText = hoverText.Replace("###", login.config.showDebug.ToString());

                // Because the button for setting the debug mode is a toggle, it needs to have extra code in case it is toggled multiple times.
                if (login.config.showDebug)
                    hoverText = hoverText.Replace("False", login.config.showDebug.ToString());
                else
                    hoverText = hoverText.Replace("True", login.config.showDebug.ToString());
                break;
            default:
                // Do nothing...
                break;
        }
    }

    /// <summary>
    /// Display this hover text on the menu system's description object. (Also raise the hover flag).
    /// </summary>
    public void ShowHoverText()
    {
        hoveredOver = true;

        // This check is necessary in case the hover description is destroyed before this code is called.
        if (MainMenuController.Instance.HoverDescription != null)
        {
            MainMenuController.Instance.HoverDescription.SetActive(true);
            MainMenuController.Instance.HoverDescription.GetComponentInChildren<Text>().text = hoverText;
        }
    }

    /// <summary>
    /// Hide the menu system's description object and make its text null. (Also lower the hover flag).
    /// </summary>
    public void HideHoverText()
    {
        hoveredOver = false;

        // This check is necessary in case the hover description is destroyed before this code is called.
        if(MainMenuController.Instance.HoverDescription != null)
        {
            MainMenuController.Instance.HoverDescription.GetComponentInChildren<Text>().text = "";
            MainMenuController.Instance.HoverDescription.SetActive(false);
        }
    }
}
