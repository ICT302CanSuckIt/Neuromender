using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scrip for controlling and updating the debug parameter display for the 'Targets' game.
/// </summary>
public class Targets_debug : MonoBehaviour {

    private Grid grid = null;           // Local handle for the grid that is being referenced by this script.
    private GloveRelative glove = null; // Local handle for the glove that is being tracked by this script.

    [Tooltip("Display for the timer that is counting down before moving the grid closer.")]
    public Text waitTimerTxt = null;

    [Tooltip("Display for the current depth of the grid.")]
    public Text gridCurrentDepthTxt = null;

    [Tooltip("Display for the average depth of the grid over the entire game.")]
    public Text gridAvgDeptTxt = null;

    [Tooltip("Display for the cell index of the next target that will be spawnied by the grid.")]
    public Text nextCellSpawnTxt = null;

    [Tooltip("Display for the co-ordinates of the glove.")]
    public Text glovePosTxt = null;

    [Tooltip("Dispaly for the co-ordinates of the currently spawned target.")]
    public Text targetPosTxt = null;

	// Use this for initialization
	void Start () {

        // Check that there is a grid in the scene.
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        // Check that there is a glove in the scene.
        glove = GameObject.Find("GloveRelative").GetComponent<GloveRelative>();

        // If there is no grid or glove in the scene, disable this script immediately.
        if (grid == null || glove == null)
            enabled = false;
        else // If there is a text object that has not been set, disable this script also.
            if (waitTimerTxt == null || gridCurrentDepthTxt == null || gridAvgDeptTxt == null || nextCellSpawnTxt == null || glovePosTxt == null || targetPosTxt == null)
                enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

        waitTimerTxt.text = "Wait Time: " + grid.gridMoveWaitTimerCurrent.ToString("0.00");

        gridCurrentDepthTxt.text = "Current Depth: " + grid.Depth.ToString("0.000");

        gridAvgDeptTxt.text = "Average Depth: " + grid.avgReach.ToString("0.000");

        if (grid.spawnOrderIndex + 1 < grid.spawnOrder.Count)
            nextCellSpawnTxt.text = "Next Spawn: " + (grid.spawnOrder[grid.spawnOrderIndex] + 1); // Value must be offset by 1 because the grid cells begin from index 1.
        else
            nextCellSpawnTxt.text = "Spawns Done!";

        glovePosTxt.text = "Glove Pos: " + glove.gameObject.transform.position.ToString("0.000");

        targetPosTxt.text = "Target Pos: " + grid.GetActiveNode().transform.position.ToString("0.000");
	}
}
