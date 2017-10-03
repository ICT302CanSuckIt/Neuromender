using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The play tutorial script is accessed by the MainMenuController.cs script when a tutorial needs to be displayed.
/// 
/// <para>For more information, see the script.</para>
/// </summary>
/// 
/// <remarks>
/// 
/// This script has the functionality of playing the video with OR without audio.
/// 
/// It also has the ability to pause / play the video at any time with the SPACE key.
/// Subsequent key presses will toggle between pause and play statuses.
/// 
/// The tutorial video can also be exited prematurely by pressing the ESCAPE key.
/// 
/// Note:   Future developers sould not need to add this script anywhere in the unity scenes, unless more than one player is needed.
///         This is becuse has been designed to be exclusively used by the main menu, which assumes there is only one in the scene 
///         for all videos.
/// 
/// Note:   Adding this script to a GameObject will also add the required RawImage and AudioSource component too.
///         Isn't the RequireComponent field awesome :D
///         
/// </remarks>
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(AudioSource))]
public class PlayTutorial : MonoBehaviour {

    /// <summary>The video that will be played.</summary>
    [Tooltip("The video that will be played.")]
    public MovieTexture video = null;

    /// <summary>TRUE: There will be NO sound while the video is played.</summary>
    [Tooltip("TRUE: There will be NO sound while the video is played.")]
    public bool mute = false;

    // Local handle for the 'screen' component that the video will be played on.
    private RawImage screen = null;

    // Local handle for the audio player for the video.
    private AudioSource sound = null;

    /// <summary>The number of seconds the tutorial screen will wait for before it closes.</summary>
    [Tooltip("The number of seconds the tutorial screen will wait for before it closes.")]
    public float countdown = 3.0f;

    // Private counter that does the actual counting.
    private float countdownDt = 0.0f;

    /// <summary>The textual prompt that displays the current behaviour of the tutorial screen.</summary>
    [Tooltip("The textual prompt that displays the current behaviour of the tutorial screen.")]
    public GameObject promptText = null;

    /// <summary>The textual prompt that displays to the player instructions of press space tp pause or unpause play of the tutorial screen.</summary>
    [Tooltip("The textual prompt that displays to the player instructions of press space to pause or play of the tutorial screen.")]
    public GameObject pausePrompt = null;

    // Private flag for showing the current status of the tutorial video.
    private bool isPaused = false;    // TRUE: The tutorial video has been paused.
    private bool done = false;        // TRUE: The tutorial video has finished playing.
    private bool pauseInst = false;   // TRUE: The tutorial video is playing

    // Use this for initialization
    void Start () {

        // Initialise the local handles.
        screen = GetComponent<RawImage>();
        sound = GetComponent<AudioSource>();

        // Prime the counter for when the video is over.
        countdownDt = countdown;

        if (video == null)
            enabled = false;
        else
        {
            screen.texture = video;
            sound.clip = video.audioClip;
        }

        // Hide the visible parts of the tutorial screen so that they do not affect the menu during normal operation.
        screen.color = Color.clear;
        promptText.SetActive(false);
        pausePrompt.SetActive(false);
	}

    // Check the keyboard input and execute the appropriate behaviour if a key is pressed. Also countdown once the video is over.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Stop();

        if (Input.GetKeyDown(KeyCode.Space) && video.isPlaying)
            Pause();
        else
            if (Input.GetKeyDown(KeyCode.Space) && (!video.isPlaying || isPaused))
                Play();

        // Stop the video once it has finished playing.
        if (!video.isPlaying && !isPaused && !done)
        {
            countdownDt -= Time.deltaTime;

            if (promptText)
                promptText.GetComponent<Text>().text = "Exiting in " + countdownDt.ToString("0.0");

            if(countdownDt <= 0)
            {
                done = true;
                countdownDt = countdown;
                Stop();
            }
        }
    }
	
    /// <summary>
    /// Specify what video this tutorial player is going to play next.
    /// </summary>
    /// <param name="vid">The video that will be played.</param>
    public void SetVideo(MovieTexture vid)
    {
        video = vid;
        screen.texture = video;
    }

    /// <summary>
    /// Play the current tutorial video.
    /// </summary>
	public void Play()
    {
        Debug.Log("Playing tutorial");

        if (pausePrompt)
            pausePrompt.GetComponent<Text>().text = "Press Space To\n Pause Video";
        pauseInst = true;
        pausePrompt.SetActive(true);

        if (promptText)
            promptText.GetComponent<Text>().text = "";

        done = false;
        isPaused = false;

        screen.color = Color.white;
        promptText.SetActive(true);

        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        enabled = true;

        video.Play();

        if(!mute)
            sound.Play();
    }

    // Pause the video.
    void Pause()
    {
        isPaused = true;

        if (promptText)
            promptText.GetComponent<Text>().text = "Paused";

        video.Pause();

        if(!mute)
            sound.Pause();

        pauseInst = true;
        if (pausePrompt)
            pausePrompt.GetComponent<Text>().text = "Press Space To\n Play Video";
    }

    // Stop the video. This also re-activates the last menu that was displayed.
    void Stop()
    {
        video.Stop();
        gameObject.SetActive(false);
        MainMenuController.Instance.ActivateMenu(MainMenuController.Instance.newActiveMenu);
    }
}
