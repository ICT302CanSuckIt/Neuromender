using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central controller for all menu-related functionality.
/// 
/// <para>For more information, see the script.</para>
/// </summary>
/// 
/// <remarks>
/// 
/// This script is in the 'MainMenu' GameObject (under the 'Menu' GameObject) in the MainMenu untiy scene.
/// 
/// The menu system has been overhauled from the previous version of Neuromender (2.6.2).
/// It has been changed to work a bit like a poowerpoint presentation, with each menu being a 'slide' in the system.
/// 
/// The menu uses the new SceneManager class (i.e SCeneManager.LoadScene() instead of Application.LoadLevel(), so those warnings bout using 
/// Application.LoadLevel() will not appear in the Unity Console during testing IF these functions are used.
/// 
/// The old code that uses Application.LoadLevel() is still in the project because there is a chance that during development of the new menu,
/// some of the old functionality was overlooked (and is thus not present in the new menu system). If you think this might be the case, then 
/// please refer to ButtonManager.cs, OptionsBtnManager.cs, and TaskSelectionBtnManager.cs for all previously used script functions. Also see
/// the objects contained under the  'OldMenuStuff' GameObject in the MainMenu unity scene.
/// 
/// The menu does not use booleans to track what is being hovered over. For that functionality, see the HoverText script.
/// The HoverText script also contains instructions for how to hook everything up so that the description appears / disappears properly.
/// 
/// The menu also does not use an Update() loop. Everything is driven by the unity event system.
/// There is also similar functionality for HoverText (see script for more info).
/// 
/// As a side note, when you are building your new menu slide you might want to disable the other menu sildes in the scene to reduce clutter.
/// If you do this please remember to re-enable them before you try testing as there will be issues if some objects are not active.
/// Could probably fix this with code but I have 101 things to do and not enough time D:
/// 
/// To add a new menu to the system:
/// 
/// 1)  Assuming that a param does not already exist, add it in. Please also include a summary XML, and a tooltip for the Unity Inspector.
/// 
/// 2)  Add a new enumeration entry in the 'MENU" enum definition for your new menu.
/// 
/// 3)  Add an entry to the new menu in the 'SETUP' stage of the Start function (or add it manually in the Unity Inspector).
/// 
/// 4)  Add references to your new menu in the ActivateMenu() and DeactivateMenu() functions.
///     If you don't do this then the main menu will appear when you try to change menus.
///     
/// 5)  If the new menu has button options for games (such as like ArmExtensionMenu), then add a new entry in the FormatMenu() function.
///     See the function for how other menus do their formatting.
///     
/// 6)  Add a function call to DeactivateMenu() in the 'DEACTIVATE' stage of the start function,
///     so that the new menu doesn't appear onscreen when the scene starts.
/// 
/// 7)  Add a void function that can be called by a button event. This function should call the ActivateMenu() function with your slide enum
///     as its input param. This will be how you navigate to your menu from within the system.
/// 
/// 8)  Lastly, there is some things you will need to change manually on the actual canvas object. See existing menus for what to change,
///     (They all have the same settings).
/// 
/// At this point everything should be ready.
/// 
/// Just hook up the void button function to the correspoonding button in the scene  (via the Unity Inspector) and try it out.
/// I'll apologise ahead of time if I've missed something out but I've had a look through everything and it seems to all be there.
/// 
/// When in doubt, check how the pre-existing menus have been setup (maybe you'll find a step I've overlooked by accident :P ).
/// 
/// </remarks>
public class MainMenuController : MonoBehaviour
{

    /// <summary>Each enum value should represent a single slide in the menu system.</summary>
    public enum MENU { MAIN, TASK, OPTIONS, ARM_RAISE_1, ARM_RAISE_2, ARM_RAISE_TUTE, ARM_EXTEND, ARM_EXTEND_TARGTUTE, ARM_EXTEND_ROWTUTE, ARM_EXTEND_CYCLE1, ARM_EXTEND_CYCLE2, ARM_EXTEND_CYCLETUTE, PEDAL }

    /// <summary>Each enum value represents a different tutorial video available in the application.</summary>
    public enum GAME_TUTORIAL { WINGMAN, TARGETS, ROWING, ARM_PEDAL, LEG_PEDAL, CYCLING }

    // The current and previous menus.
    private MENU oldActiveMenu = MENU.MAIN;
    /// <summary>The current active menu in the menu system</summary>
    public MENU newActiveMenu = MENU.MAIN;

    // local handles for external information access.
    private LoginControl login = null;
    private database DB = null;
    private GameObject DBCons = null;

    // Below are all of the slides in this menu system. More will likely be needed...  D:
    /// <summary>Local handle for the main menu in this system.</summary>
    private GameObject MainMenu = null;

    /// <summary>The menu that contains the task selection options.</summary>
    [Tooltip("The menu that contains the task selection options.")]
    public GameObject TaskMenu = null;

    /// <summary>The menu that contins the extra options.</summary>
    [Tooltip("The menu that contins the extra options.")]
    public GameObject OptionsMenu = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Wingman' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Wingman' tutorial.")]
    public GameObject ArmRaiseTutorialCheck = null;

    /// <summary>The menu that checks what map the next 'Wingman' game will be played on.</summary>
    [Tooltip("The menu that checks what map the next 'Wingman' game will be played on.")]
    public GameObject ArmRaiseMenu1 = null;

    /// <summary>The menu that checks what speed the next 'Wingman' game will be played at.</summary>
    [Tooltip("The menu that checks what speed the next 'Wingman' game will be played at.")]
    public GameObject ArmRaiseMenu2 = null;

    /// <summary>The video that demonstates a tutorial for the 'Wingman' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the Wingman game.")]
    public MovieTexture WingmanTutorial = null;

    /// <summary>The menu that shows the available arm extension games.</summary>
    [Tooltip("The menu that shows the available arm extension games.")]
    public GameObject ArmExtensionMenu = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Targets' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Targets' tutorial.")]
    public GameObject TargetsTutorialCheck = null;

    /// <summary>The video that demonstates a tutorial for the 'Targets' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the 'Targets' game.")]
    public MovieTexture TargetsTutorial = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Rowing' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Rowing' tutorial.")]
    public GameObject RowingTutorialCheck = null;

    /// <summary>The video that demonstates a tutorial for the 'Rowing' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the 'Rowing' game.")]
    public MovieTexture RowingTutorial = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Wingman' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Cycling' tutorial.")]
    public GameObject CyclingTutorialCheck = null;

    /// <summary>The menu that checks what graphic the next 'Cycling' game will be played on.</summary>
    [Tooltip("The menu that checks what graphic the next 'Cycling' game will be played on.")]
    public GameObject CyclingMenu1 = null;

    /// <summary>The menu that checks what distance the next 'Cycling' game will be played at.</summary>
    [Tooltip("The menu that checks what distance the next 'Cycling' game will be played at.")]
    public GameObject CyclingMenu2 = null;

    /// <summary>The video that demonstates a tutorial for the 'Cycling' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the Cycling game.")]
    public MovieTexture CyclingTutorial = null;

    /// <summary>The menu that shows the available pedalling games.</summary>
    [Tooltip("The menu that shows the available pedalling games.")]
    public GameObject PedallingMenu = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Arm Pedalling' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Arm Pedalling' tutorial.")]
    private GameObject ArmPedalTutorialCheck = null;

    /// <summary>The video that demonstates a tutorial for the 'Arm Pedalling' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the 'Arm Pedalling' game.")]
    public MovieTexture ArmPedalTutorial = null;

    /// <summary>The menu that checks if the survivor wants to view the 'Leg Pedalling' tutorial.</summary>
    [Tooltip("The menu that checks if the survivor wants to view the 'Leg Pedalling' tutorial.")]
    private GameObject LegPedalTutorialCheck = null;

    /// <summary>The video that demonstates a tutorial for the 'Leg Pedalling' game.</summary>
    [Tooltip("The video that demonstates a tutorial for the 'Leg Pedalling' game.")]
    public MovieTexture LegPedalTutorial = null;

    /// <summary>The GameObject that is used as a screen to project the tutorial video onto.</summary>
    [Tooltip("The GameObject that is used as a screen to project the tutorial video onto.")]
    public GameObject tutorialScreen = null;

    /// <summary>The GameObject that is used as a screen to describe a button when the button is hovered over.</summary>
    [Tooltip("The GameObject that is used as a screen to describe a button when the button is hovered over")]
    public GameObject HoverDescription = null;

    /// <summary>The slide that displays the credits for this project / application.</summary>
    [Tooltip("The slide that displays the credits for this project / application.")]
    public GameObject CreditsSlide = null;

    private bool init = false;

    /// <summary>Since there should only be one Main Menu in a scene, this param represents that one instance.</summary>
    public static MainMenuController Instance { get { return instance; } private set { instance = value; } }
    private static MainMenuController instance = null;

    // Use this for initialization
    void Start()
    {

        // --------------------------------------- INIT -------------------------------------------

        // There shouldn't be more than one of these in a secene at any time.
        if (Instance == null)
            Instance = this;
        else
            enabled = false;

        DBCons = GameObject.Find("DatabaseController");

        if (DBCons != null)
        {
            login = DBCons.GetComponent<LoginControl>();

            // Update what games are able to be shown on the menu.
            if (login != null)
                login.UpdateGameAvailability();

            DB = DBCons.GetComponent<database>();
        }
        else
        {
            //enabled = false;
            Debug.LogError("Could not find database controller.");
        }

        MainMenu = this.gameObject;

        // -----------------------------------------------------------------------------------------

        // --------------------------------------- SETUP -------------------------------------------

        // Make sure that all menu have a canvas group for easy activation / deactivation.
        // Add additional menus below here as required.
        if (MainMenu.GetComponent<CanvasGroup>() == null)
            MainMenu.AddComponent<CanvasGroup>();

        if (TaskMenu.GetComponent<CanvasGroup>() == null)
            TaskMenu.AddComponent<CanvasGroup>();

        if (OptionsMenu.GetComponent<CanvasGroup>() == null)
            OptionsMenu.AddComponent<CanvasGroup>();

        if (ArmRaiseMenu1.GetComponent<CanvasGroup>() == null)
            ArmRaiseMenu1.AddComponent<CanvasGroup>();

        if (ArmRaiseMenu2.GetComponent<CanvasGroup>() == null)
            ArmRaiseMenu2.AddComponent<CanvasGroup>();

        if (ArmExtensionMenu.GetComponent<CanvasGroup>() == null)
            ArmExtensionMenu.AddComponent<CanvasGroup>();

        if (CyclingMenu1.GetComponent<CanvasGroup>() == null)
            CyclingMenu1.AddComponent<CanvasGroup>();

        if (CyclingMenu2.GetComponent<CanvasGroup>() == null)
            CyclingMenu2.AddComponent<CanvasGroup>();

        if (PedallingMenu.GetComponent<CanvasGroup>() == null)
            PedallingMenu.AddComponent<CanvasGroup>();

        // -----------------------------------------------------------------------------------------

        // --------------------------------------- FORMATTING -------------------------------------------

        // Check which menu options are avaiable from the website login details.
        FormatMenu(MENU.TASK);
        FormatMenu(MENU.ARM_EXTEND);
        //FormatMenu(MENU.PEDAL);

        // -----------------------------------------------------------------------------------------

        // --------------------------------------- DEACTIVATION -------------------------------------------

        // Deactivate all of the sub menus. Add additional menu entries here as required.
        DeactivateMenu(MENU.TASK);
        DeactivateMenu(MENU.OPTIONS);
        DeactivateMenu(MENU.ARM_RAISE_1);
        DeactivateMenu(MENU.ARM_RAISE_2);
        DeactivateMenu(MENU.ARM_RAISE_TUTE);
        DeactivateMenu(MENU.ARM_EXTEND);
        DeactivateMenu(MENU.ARM_EXTEND_TARGTUTE);
        DeactivateMenu(MENU.ARM_EXTEND_ROWTUTE);
        DeactivateMenu(MENU.ARM_EXTEND_CYCLE1);
        DeactivateMenu(MENU.ARM_EXTEND_CYCLE2);
        DeactivateMenu(MENU.ARM_EXTEND_CYCLETUTE);
        DeactivateMenu(MENU.PEDAL);

        // -----------------------------------------------------------------------------------------

        // Make sure that the main menu is the first active menu.
        ActivateMenu();

        // --------------------------------------- EXTRA -------------------------------------------

        // Set the slide for the credits display.
        // Its a bit extra than just deactivating the options canvas because these buttons should be off initially even if the options canvas is activated.
        HideCreditSlide();
        OptionsMenu.transform.Find("ShowPrevTeams").gameObject.SetActive(false);
        OptionsMenu.transform.Find("RtnOptions").gameObject.SetActive(false);

        // -----------------------------------------------------------------------------------------
    }

    public void Update()
    {
        if (!init)
        {
            if (DBCons != null)
            {
                login = DBCons.GetComponent<LoginControl>();

                // Update what games are able to be shown on the menu.
                if (login != null)
                    login.UpdateGameAvailability();

                DB = DBCons.GetComponent<database>();

                init = true;
            }
            else
            {
                //enabled = false;
                Debug.LogError("Could not find database controller.");
            }
        }
    }

    /// <summary>
    /// Activate the specified menu. The default activation is the main menu.
    /// <para>(The current menu is also disabled automatically so as to not hinder the new one).</para>
    /// </summary>
    /// <param name="theMenu">The enumeration value that specifies which menu is to be activated.</param>
    public void ActivateMenu(MENU theMenu = MENU.MAIN)
    {
        CanvasGroup menu = null;

        // Specify the new active menu.
        oldActiveMenu = newActiveMenu;
        newActiveMenu = theMenu;

        // Choose the menu that corresponds to the enumeration value.
        switch (theMenu)
        {
            case MENU.TASK:
                menu = TaskMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.OPTIONS:
                menu = OptionsMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_1:
                menu = ArmRaiseMenu1.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_2:
                menu = ArmRaiseMenu2.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_TUTE:
                menu = ArmRaiseTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND:
                menu = ArmExtensionMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_TARGTUTE:
                menu = TargetsTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_ROWTUTE:
                menu = RowingTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLE1:
                menu = CyclingMenu1.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLE2:
                menu = CyclingMenu2.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLETUTE:
                menu = CyclingTutorialCheck.GetComponent<CanvasGroup>();
                break;

            case MENU.PEDAL:
                menu = PedallingMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.MAIN:
            default:
                menu = MainMenu.GetComponent<CanvasGroup>();
                break;
        }

        DeactivateMenu(oldActiveMenu);
        Debug.Log("Activating menu: " + menu.gameObject.name);

        // The below code makes sure the menu is visible, interactable, and blocks mouse clicks so that underlying buttons aren't pressed.
        menu.alpha = 1;
        menu.blocksRaycasts = true;
        menu.interactable = true;
    }

    /// <summary>
    /// Deactivate the current menu. It will still be active in the scene, but will be invisible AND non-interactable.
    /// </summary>
    /// <param name="theMenu">The enumeration value for the menu that will be deactivated.</param>
    public void DeactivateMenu(MENU theMenu)
    {
        CanvasGroup menu = null;    // Local handle for the canvas group of the menu to be deactivated.

        // Choose the menu that corresponds to the input enumeration param value.
        switch (theMenu)
        {
            case MENU.TASK:
                menu = TaskMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.OPTIONS:
                menu = OptionsMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_1:
                menu = ArmRaiseMenu1.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_2:
                menu = ArmRaiseMenu2.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_RAISE_TUTE:
                menu = ArmRaiseTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND:
                menu = ArmExtensionMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_TARGTUTE:
                menu = TargetsTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_ROWTUTE:
                menu = RowingTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLE1:
                menu = CyclingMenu1.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLE2:
                menu = CyclingMenu2.GetComponent<CanvasGroup>();
                break;
            case MENU.ARM_EXTEND_CYCLETUTE:
                menu = CyclingTutorialCheck.GetComponent<CanvasGroup>();
                break;
            case MENU.PEDAL:
                menu = PedallingMenu.GetComponent<CanvasGroup>();
                break;
            case MENU.MAIN:
            default:
                menu = MainMenu.GetComponent<CanvasGroup>();
                break;
        }

        Debug.Log("Deactivating menu: " + menu.gameObject.name);

        // The below code makes sure the menu is NOT visible or interactable, and allows mouse clicks to press buttons 'underneath' this canvas.
        menu.alpha = 0;
        menu.blocksRaycasts = false;
        menu.interactable = false;
    }

    /// <summary>
    /// Format the options in the selected menu according to what is marked as available in the login controller.
    /// </summary>
    /// <param name="theMenu">The enumeration value for the menu that will be formatted.</param>
    void FormatMenu(MENU theMenu)
    {
        switch (theMenu)
        {
            case MENU.TASK:

                // Check if the Wingman functionality has been enabled.
                TaskMenu.transform.Find("ElbowRaiseButton").gameObject.SetActive(login.config.allowWingman);

                break;
            case MENU.ARM_EXTEND:

                // Check if the Targets functionality has been enabled.
                ArmExtensionMenu.transform.Find("TargetsButton").gameObject.SetActive(login.config.allowTargets);

                // Check if the Rowing functionality has been enabled.
                ArmExtensionMenu.transform.Find("RowingButton").gameObject.SetActive(login.config.allowRowing);

                // Check if the Cycling functionality has been enabled.
                ArmExtensionMenu.transform.Find("CyclingButton").gameObject.SetActive(login.config.allowCycling);

                // Disable access to the whole arm extension menu if neither 'Targets' OR 'Rowing' games are available to the survivor.
                TaskMenu.transform.Find("ArmExtensionButton").gameObject.SetActive(!(!login.config.allowTargets && !login.config.allowRowing && !login.config.allowCycling));

                break;
            case MENU.PEDAL:
                // Code not implemented here yet...
                break;
            case MENU.MAIN:
            default:
                // Do nothing...
                break;
        }
    }

    /// <summary>
    /// Show the tutorial video for the specified game.
    /// <para>Note: The video is not automatically played in this function, but it is ready to be.</para>
    /// </summary>
    /// <param name="tutorial"></param>
    void ShowTutorial(GAME_TUTORIAL tutorial)
    {
        switch (tutorial)
        {
            case GAME_TUTORIAL.WINGMAN:
                DeactivateMenu(MENU.ARM_RAISE_TUTE);
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(WingmanTutorial);
                break;
            case GAME_TUTORIAL.TARGETS:
                DeactivateMenu(MENU.ARM_EXTEND_TARGTUTE);
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(TargetsTutorial);
                break;
            case GAME_TUTORIAL.ROWING:
                DeactivateMenu(MENU.ARM_EXTEND_ROWTUTE);
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(RowingTutorial);
                break;
            case GAME_TUTORIAL.CYCLING:
                DeactivateMenu(MENU.ARM_EXTEND_CYCLETUTE);
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(CyclingTutorial);
                break;
            case GAME_TUTORIAL.ARM_PEDAL:
                DeactivateMenu(MENU.PEDAL); // <-- This will be whatever you call the tutorial check slide for the arm pedalling menu.
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(ArmPedalTutorial);
                break;
            case GAME_TUTORIAL.LEG_PEDAL:
                DeactivateMenu(MENU.PEDAL); // <-- This will be whatever you call the tutorial check slide for the leg pedalling menu.
                tutorialScreen.GetComponent<PlayTutorial>().SetVideo(LegPedalTutorial);
                break;
            default:
                // Do nothing...
                break;
        }

        // Turn the tutorial screen on.
        tutorialScreen.SetActive(true);
    }

    /// <summary>
    /// Show the first menu of the Arm Raise (Wingman) game. This is the 'Map Select' menu.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowArmRaiseMenu1()
    {
        ActivateMenu(MENU.ARM_RAISE_1);
    }

    /// <summary>
    /// Choose the 'Beach' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseBeachMap()
    {
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.beach;
    }

    /// <summary>
    /// Choose the 'Forest' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseForestMap()
    {
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.forest;
    }

    /// <summary>
    /// Choose the 'Temple' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseTempleMap()
    {
        GameObject.Find("DatabaseController").GetComponent<LoginControl>().selectedTrack = TrackName.temple;
    }

    /// <summary>
    /// Show the second menu of the Arm Raise (Wingman) game. This is the 'Speed Select' menu.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowArmRaiseMenu2()
    {
        ActivateMenu(MENU.ARM_RAISE_2);
    }

    /// <summary>
    /// Choose the slowest speed from the available options for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseSlowSpeed()
    {
        login.selectedSpeed = SpeedLevel.slow;
    }

    /// <summary>
    /// Choose the medium speed from the available options for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseMediumSpeed()
    {
        login.selectedSpeed = SpeedLevel.medium;
    }

    /// <summary>
    /// Choose the fastest speed from the available options for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseFastSpeed()
    {
        login.selectedSpeed = SpeedLevel.fast;
    }

    /// <summary>
    /// Show the menu that checks if the survivor want to view the 'Wingman' tutorial or not.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowWingmanTuteCheck()
    {
        ActivateMenu(MENU.ARM_RAISE_TUTE);
    }

    /// <summary>
    /// Display (and play) the video tutorial of the 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowWingmanTutorial()
    {
        ShowTutorial(GAME_TUTORIAL.WINGMAN);
        if (tutorialScreen.GetComponent<PlayTutorial>().video != null)
            tutorialScreen.GetComponent<PlayTutorial>().Play();
    }

    /// <summary>
    /// Show the menu for selecting available arm extension games.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowArmExtensionMenu()
    {
        ActivateMenu(MENU.ARM_EXTEND);
    }

    /// <summary>
    /// Show the menu that checks if the survivor want to view the 'Targets' tutorial or not.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowTargetsTuteCheck()
    {
        ActivateMenu(MENU.ARM_EXTEND_TARGTUTE);
    }

    /// <summary>
    /// Display (and play) the video tutorial of the 'Targets' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowTargetsTutorial()
    {
        ShowTutorial(GAME_TUTORIAL.TARGETS);
        if (tutorialScreen.GetComponent<PlayTutorial>().video != null)
            tutorialScreen.GetComponent<PlayTutorial>().Play();
    }

    /// <summary>
    /// Show the menu that checks if the survivor want to view the 'Rowing' tutorial or not.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowRowingTuteCheck()
    {
        ActivateMenu(MENU.ARM_EXTEND_ROWTUTE);
    }

    /// <summary>
    /// Display (and play) the video tutorial of the 'Rowing' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowRowingTutorial()
    {
        // Code is not implemented yet...
    }

    ///////MICH
    /// <summary>
    /// Show the first menu of the Arm Raise (Wingman) game. This is the 'Map Select' menu.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowCyclingMenu1()
    {
        ActivateMenu(MENU.ARM_EXTEND_CYCLE1);
    }

    /// <summary>
    /// Choose the 'Beach' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseLowGraphic()
    {
        login.selectedGraphic = GraphicName.Low;
    }

    /// <summary>
    /// Choose the 'Forest' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChoosemediumGraphic()
    {
        login.selectedGraphic = GraphicName.Medium;
    }

    /// <summary>
    /// Choose the 'Temple' map for the next 'Wingman' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseHighGraphic()
    {
        login.selectedGraphic = GraphicName.High;
    }

    /// <summary>
    /// Show the second menu of the Arm Raise (Wingman) game. This is the 'Speed Select' menu.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowCyclingMenu2()
    {
        ActivateMenu(MENU.ARM_EXTEND_CYCLE2);
    }

    /// <summary>
    /// Choose the Short distance from the available options for the next 'Cycling' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseShortDistance()
    {
        login.selectedDistance = DistanceLevel.Short;
    }

    /// <summary>
    /// Choose the medium distance from the available options for the next 'Cycling' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseMediumDistance()
    {
        login.selectedDistance = DistanceLevel.Medium;
    }

    /// <summary>
    /// Choose the Long Distance from the available options for the next 'Cycling' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ChooseLongDistance()
    {
        login.selectedDistance = DistanceLevel.Long;
    }

    /// <summary>
    /// Show the menu that checks if the survivor want to view the 'Wingman' tutorial or not.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowCyclingTuteCheck()
    {
        ActivateMenu(MENU.ARM_EXTEND_CYCLETUTE);
    }

    /// <summary>
    /// Display (and play) the video tutorial of the 'Cycling' game.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowCyclingTutorial()
    {
        ShowTutorial(GAME_TUTORIAL.CYCLING);
        if (tutorialScreen.GetComponent<PlayTutorial>().video != null)
            tutorialScreen.GetComponent<PlayTutorial>().Play();
    }

    /// <summary>
    /// Show the menu for selecting available pedalling games.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowPedalingMenu()
    {
        ActivateMenu(MENU.PEDAL);
    }

    /// <summary>
    /// Show the menu for selecting from all broad categories of games.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowTaskMenu()
    {
        ActivateMenu(MENU.TASK);
    }

    /// <summary>
    /// Show the menu for selecting / altering additional game options, and seeing the credits.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowOptionsMenu()
    {
        ActivateMenu(MENU.OPTIONS);
    }

    /// <summary>
    /// Show the Main Menu. Surprise! :D
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowMainMenu()
    {
        ActivateMenu();
    }

    /// <summary>
    /// Change the options screen to hide unnecessary buttons and show only the relevant buttons and the credits slide.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ShowCreditSlide()
    {
        CreditsSlide.SetActive(true);
        OptionsMenu.transform.Find("ShowPrevTeams").gameObject.SetActive(true);
        OptionsMenu.transform.Find("RtnOptions").gameObject.SetActive(true);
        OptionsMenu.transform.Find("Show Credits").gameObject.SetActive(false);
        OptionsMenu.transform.Find("ToggleDebug").gameObject.SetActive(false);
        OptionsMenu.transform.Find("Return").gameObject.SetActive(false);
        HoverDescription.SetActive(false);
    }

    /// <summary>
    /// Change the options screen to hide the credits slide and related buttons, and show the default menu buttons.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void HideCreditSlide()
    {
        CreditsSlide.SetActive(false);
        OptionsMenu.transform.Find("ShowPrevTeams").gameObject.SetActive(false);
        OptionsMenu.transform.Find("RtnOptions").gameObject.SetActive(false);
        OptionsMenu.transform.Find("Show Credits").gameObject.SetActive(true);
        OptionsMenu.transform.Find("ToggleDebug").gameObject.SetActive(true);
        OptionsMenu.transform.Find("Return").gameObject.SetActive(true);
    }

    /// <summary>
    /// Toggle the current debug mode for the game. Each call of this function will flip the setting.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void ToggleDebugMode()
    {
        login.config.showDebug = !login.config.showDebug;
    }

    /// <summary>
    /// Open a web link to the About page of the Neuromender site (currently v3).
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// <para>Note: At the moment, this function defaults to the login screen unless you are currently logged in.</para>
    /// </summary>
    public void ViewPastTeams()
    {
        Application.OpenURL("http://vegas.murdoch.edu.au/neuromender3/Main/About.php");
    }

    /// <summary>
    /// Open a web link to the Login page of the Neuromender site (currently v3).
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void WebsiteLogin()
    {
        Application.OpenURL("http://vegas.murdoch.edu.au/neuromender3/Main/Login.php");
    }

    /// <summary>
    /// Logout of the application. This empties the database controller and returns to the login screen.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void Logout()
    {
        if (GameObject.Find("DatabaseController"))
        {
            GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();
            Destroy(GameObject.Find("DatabaseController"));
        }

        SceneManager.LoadScene("Login");
    }

    /// <summary>
    /// Load the 'Wingman' game scene.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void LoadWingmanScene()
    {
        SceneManager.LoadScene("TestNeuromend3_CMaster");
    }

    /// <summary>
    /// Load the 'Targets' game scene.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void LoadTargetsScene()
    {
        SceneManager.LoadScene("Targets");
    }

    /// <summary>
    /// Load the 'Rowing' game scene.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void LoadRowingScene()
    {
        SceneManager.LoadScene("Rowing");
    }

    /// <summary>
    /// Load the 'Rowing' game scene.
    /// <para>Note: This function is intended to be called in a button click event.</para>
    /// </summary>
    public void LoadCyclingScene()
    {
        SceneManager.LoadScene("Cycling Test3");
    }

}