using UnityEngine;

/// <summary>
/// Class that represents the functionality of a single waypoint in the 'Rowing' game.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class Buoy : MonoBehaviour {

    [Tooltip("The world position Y-value that all buoys will use (to set all buoys to the same height try editing the prefab).")]
    public float worldHeight = 7.5f;

    [Tooltip("Alternate place to set the size of the collider.")]
    public float colliderRadius = 1.0f;

    [Tooltip("The time taken for the buoy to disappear.")]
    public float fadeOutTime = 4.0f;
    private float fadeOutDt = 0.0f;

    [Tooltip("Sound played when the Buoy is hit by the boat.")]
    public AudioClip hitClip = null;

    public Color color = Color.yellow;

    // --- Local handles ---
    BoatTrack track = null;
    SphereCollider bounds = null;
    AudioSource sound = null;
    MeshRenderer render = null;

    bool wasHit = false;    // TRUE: The boat has entered within the specified bounds of this buoy.
    bool faded = false;     // TRUE: The buoy has finished fading out.

    // Use this for initialization
    void Start () {

        track = GetComponentInParent<BoatTrack>();

        if(track == null)
        {
            enabled = false;
        }
        else
        {
            if (transform.localPosition.y != worldHeight)
                transform.localPosition = new Vector3(transform.localPosition.x, worldHeight, transform.localPosition.z);

            // Set up the material for fade-out functionality.
            render = GetComponent<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Transparent/Diffuse"));    // <-- VERY important to use 'Transparent/Diffuse' instead os regular 'Diffuse'.
            newMaterial.color = color;
            render.material = newMaterial;

            fadeOutDt = fadeOutTime;

            // Set up the collider and make sure it is a trigger.
            bounds = GetComponent<SphereCollider>();
            bounds.radius = colliderRadius;
            bounds.isTrigger = true;

            // Set the audio and make sure it does NOT loop.
            sound = GetComponent<AudioSource>();
            sound.clip = hitClip;
            sound.loop = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
        // Play the audio clip and disappear when the boar hits this buoy.
        if(wasHit)
        {
            if(sound.clip != null)
                sound.Play();

            if(fadeOutDt > 0.0f)
            {
                fadeOutDt -= Time.deltaTime;

                if(fadeOutDt < 0.0f)
                {
                    fadeOutDt = 0.0f;
                    faded = true;
                }

                color = new Color(color.r, color.g, color.b, fadeOutDt / fadeOutTime);
                render.material.color = color;
            }

            // Make the buoy object disappear when the audio clip stops playing.
            if (!sound.isPlaying && faded)
                gameObject.SetActive(false);
        }

	}

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Hit");

        if(coll.gameObject.name == "Boat" && !wasHit)
        {
            wasHit = true;
            track.NextWaypoint();
        }
    }
}
