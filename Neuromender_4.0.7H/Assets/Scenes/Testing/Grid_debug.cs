using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Controller for a debug overlay of a Targets grid.
/// 
/// This debug overlay provides information on:
/// 
///     * The possible spawn position for targets in the grid.
///     * The indexes that this target will appear at during the treatment plan that is specified for the grid.
///     * The Grid index number.
///     * The total number of times that the grid has been selected.
///     
/// Note: This script functions very similarly to the Grid class with regards to spawning, positioning, and scaling cells.
/// </summary>
public class Grid_debug : MonoBehaviour {

    [Tooltip("The grid that is being represented by this debug grid.")]
    public Grid grid = null;

    [Tooltip("The number of columns that this debug grid has.")]
    public int columns = 0;

    [Tooltip("The number of rows that this debug grid has.")]
    public int rows = 0;

    [Tooltip("The template for the object that will represent a grid cell.\nNOTE: Requires three child objects, each with a Text component.")]
    public GameObject cellTemplate = null;

    [Tooltip("A modifier for the scale that the cells will be spawned with.")]
    public float cellScaleMod = 1.0f;
    private Vector3 cellScaleVector;    // A vector used for scaling the cells that are spawned by this debug grid.

    [Tooltip("A container for all of the cells associated with this grid.")]
    public List<GameObject> cells = null;

    [Tooltip("The grid might want to be active AND hidden. So use this.\nTRUE: This debug grid is displaying its cells.\nFALSE: This grid's cells are hidden.")]
    public bool displayCells = false;

    private int cellCount = 1;          // Index of a particular grid cell. Used during spawning only.

	// Use this for initialization
	void Start () {

        if (grid == null && GameObject.Find("Grid") != null)
            grid = GameObject.Find("Grid").GetComponent<Grid>();

        if (grid == null || cellTemplate == null)
            enabled = false;

        if(enabled)
        {
            cells = new List<GameObject>();

            columns = grid.cols;
            rows = grid.rows;

            cellScaleMod = grid.targetScaleMod;    // Link this grid's scaling modifier to the modifier of the grid it is monitoring.

            Debug.Log("(BEFORE) Cell Scale: " + cellScaleMod);

            // If the scale mod if greater than 1, limit it.
            if (cellScaleMod > 1)
                cellScaleMod = 1;
            else
            {
                if (cellScaleMod * rows > grid.HEIGHT_LIMIT)
                    cellScaleMod = grid.HEIGHT_LIMIT / rows;

                if (cellScaleMod * columns > grid.WIDTH_LIMIT)
                    cellScaleMod = grid.WIDTH_LIMIT / columns;

                Debug.Log("(DURING) Cell Scale: " + cellScaleMod);

                cellScaleMod *= 5;
            }

            Debug.Log("(AFTER) Cell Scale: " + cellScaleMod);

            cellScaleVector = new Vector3(cellScaleMod, cellScaleMod, 1);

            SpawnGridCells();
        }

	}
	
	// Update is called once per frame
	void Update () {
	
        if(displayCells)
        {

        }
        else
        {

        }

    }

    /// <summary>
    /// Create and position the required number of cells based on the position and scale of the referenced grid.
    /// </summary>
    void SpawnGridCells()
    {
        for(int r = 0; r < rows; r++)
            for(int c = 0; c < columns; c++)
            {
                float adjX = (c - (columns - 1) * 0.5f) * grid.gridScale.x;
                float adjY = (r - (rows - 1) * 0.5f) * grid.gridScale.y;

                if(columns > 1)
                    adjX /= (columns - 1);

                if (rows > 1)
                    adjY /= (rows - 1);

                CreateCell(c, r, new Vector3((adjX + grid.gridOffset.x), (adjY + grid.gridOffset.y), 0), cellScaleVector);
            }
    }

    /// <summary>
    /// Create a new cell in the debug grid and size / position it properly.
    /// </summary>
    /// <param name="col">The collumn that this cell will be referenced as from.</param>
    /// <param name="row">The row that this cell will be referenced as from.</param>
    /// <param name="pos">The position that this cell will be placed in relative to the debug grid game object.</param>
    /// <param name="scale">The scaling that this cell will be relative to the debug grid game object.</param>
    void CreateCell(int col, int row, Vector3 pos, Vector3 scale)
    {
        GameObject newCell = Instantiate(cellTemplate);

        // Set the parent transform of this new cell before changing its position / scale.
        newCell.transform.SetParent(transform);

        newCell.name = "Cell_" + col + "_" + row;

        newCell.transform.localPosition = pos;
        newCell.transform.localScale = scale;

        // Add all of the indexes that this grid cell is selected in the treatment plan into a string, and find the number of selections.
        string occurenceTxt = "";
        int cellSelections = 0;

        for(int i = 0; i < grid.spawnOrder.Count; i++)
        {
            if (grid.spawnOrder[i] == cellCount - 1)
            {
                occurenceTxt += i + ",";
                cellSelections++;
            }
        }

        newCell.GetComponentsInChildren<Text>()[0].text = occurenceTxt.Trim(',');   // Get rid of the last ',' so that it is prettier.
        newCell.GetComponentsInChildren<Text>()[1].text = cellCount.ToString();     // Set the grid cell index (starts at one for more intuitive reading).
        newCell.GetComponentsInChildren<Text>()[2].text = cellSelections + "x";     // Add an x on the end to represent the 'times' this cell was selected.

        cells.Add(newCell);
        cellCount++;
    }

    /// <summary>
    /// Makes the specified cell more noticable to the viewer.
    /// </summary>
    /// <param name="col">The collumn that the cell is in.</param>
    /// <param name="row">The row that the cell is in.</param>
    void HighlightCell(int col, int row)
    {
        // Highlight the selected cell here.
        if (gameObject.transform.Find("Cell_" + col + "_" + row))
        {

        }
        else
            Debug.LogWarning("Grid in " + gameObject.name + "attempted to highlight a non-existant cell!");
    }
}
