using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Spawner for a grid of targets in a game world. Used in the 'Targets' treatment game.
/// </summary>
public class Grid : MonoBehaviour
{
    [Header("Basic Params")]
    [Tooltip("The prefab target object, used as a template for spawning the grid of targets in the game world.")]
    public GameObject node;

    [Tooltip("List of all targets the belong to this grid.")]
    public List<GameObject> NodeList = new List<GameObject>();

    public float Depth = 0.5f;
    public float ReloadDist = 0.10f;

    public int ActiveNode = 0;      // Index specifying which target in the grid is active.
    public bool canSpawn = false;   // TURE: The grid is able to spawn the next target.

    [Tooltip("Local handle to the DatabaseController game object.")]
    public GameObject DBcons;
    [Tooltip("Local handle to the score board game object.")]
    public GameObject Score;

    private GameObject ActiveNodeObject = null;

    private List<float> reachDistances = new List<float>();
    public float avgReach = 0.0f;

    //------------------------------------------------------------------------------------------
    [Header("Grid Dimensions")]
    //------------------------------------------------------------------------------------------
    
    [Tooltip("The number of rows in the grid.")]
    public int rows = 2;

    [Tooltip("The number of columns in the grid.")]
    public int cols = 2;

    [Tooltip("Scale of the distance between each target.")]
    public Vector2 gridScale;

    [Tooltip("Offset to move the grid in the event that it is not centred properly.")]
    public Vector2 gridOffset;

    [Tooltip("Scale of individual targets.")]
    [Range(0.5f, 1.5f)]
    public float targetScaleMod = 1.0f;
    private Vector3 targetScaleVector;

    [Tooltip("TRUE: Allow targets to overlap bounds.\nFALSE: Automatically alter grid scale to remove overlapping target bounds.")]
    public bool allowTargetOverlapping = false;

    //------------------------------------------------------------------------------------------
    [Header("Treatment Plan")]
    //------------------------------------------------------------------------------------------

    [Tooltip("The current index from the spawn order.")]
    public int spawnOrderIndex = 0;

    [Tooltip("TRUE: Repeat the spawnOrder list a certain number of times.")]
    public bool repeatSpawnOrder = false;

    [Tooltip("The number of times that the spawnOrder will repeat (if repetitions are set to TRUE).")]
    public ushort numRepetitions = 0;
    private ushort repIndex = 0;

    [Tooltip("The delay (in seconds) before the grid begins to move closer.")]
    public float gridMoveWaitTime = 5.0f;
    public float gridMoveWaitTimerCurrent { get { return gridMoveWaitTimerDt; } }
    private float gridMoveWaitTimerDt = 0.0f;
    private bool gridMove = false;
    public bool minDistanceMet = false;
    public bool gridReload = false;

    private float gridWidthLimit = 0.8f;
    public float WIDTH_LIMIT { get { return gridWidthLimit; } }

    private float gridHeightLimit = 0.6f;
    public float HEIGHT_LIMIT { get { return gridHeightLimit; } }

    [Tooltip("The order that the targets will spawn in.")]
    public List<ushort> spawnOrder = new List<ushort>();

    void OnLevelWasLoaded()
    {
        if (!Score)
        {
            Score = GameObject.Find("ScoreBoard");
        }

        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            

            Depth = DBcons.GetComponent<LoginControl>().config.extensionThreshold * 0.001f;

            //if (Depth < 0.25f) Depth = 0.25f;

            //Testing IncreaseDepth and DecreaseDepth functions
            //IncreaseDepth();
            //DecreaseDepth();

            //    ReloadDist = DBcons.GetComponent<LoginControl>().config.minExtensionThreshold * 0.01f;
            //     if (ReloadDist <= 15.0f) ReloadDist = 0.15f;
        }
    }

    // Use this for initialization
    void Awake()
    {
        DBcons = GameObject.Find("DatabaseController");

        // Get relevant data from the database, if it exists.
        if (DBcons != null)
        {
            LoginControl login = DBcons.GetComponent<LoginControl>();
            string[] data;

            // Get and set the grid dimensions from the database.
            data = login.config.GridSize.Split(',');
            rows = System.Convert.ToInt32(data[0]);
            cols = System.Convert.ToInt32(data[1]);

            // Get and set the target spawn order from the database.
            char[] delim = { ',' };
            data = login.config.GridOrder.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);

            spawnOrder.Clear();
            for (int i = 0; i < data.Length; i++)
                spawnOrder.Add((ushort)(System.Convert.ToUInt16(data[i]) - 1));

            // Specify the number of shots in the treatment plan.
            Score.GetComponent<ScoreCarnival>().ShotsTotal = spawnOrder.Count;
        }
        else
            enabled = false;

        float targetScale = targetScaleMod * 0.2f;    // Hardcoded value to keep to world co-ordinates. Default target size is 20 cm.

        float targetScaleMaxX, targetScaleMaxY;
        
        // Find the max scale targets can be along each axis to reach the grid dimension limits.
        targetScaleMaxY = gridHeightLimit / rows;
        targetScaleMaxX = gridWidthLimit / cols;

        // Choose the ratio that has BOTH axis within the dimension limits
        if (targetScaleMaxY * cols > gridWidthLimit)
            targetScale = targetScaleMaxX;
        else
            targetScale = targetScaleMaxY;

        if (gridScale == Vector2.zero)
            gridScale = new Vector2((cols - 1) * targetScale, (rows - 1) * targetScale);

        // Apply target overlap removal grid rescaling only when it is NOT allowed...
        if (!allowTargetOverlapping)
        {
            if ((cols - 1) * targetScale > gridScale.x)
                gridScale = new Vector2((cols - 1) * targetScale, gridScale.y);

            if ((rows - 1) * targetScale > gridScale.y)
                gridScale = new Vector2(gridScale.x, (rows - 1) * targetScale);
        }

        targetScaleVector = new Vector3(targetScale, targetScale, 1);

        gridMoveWaitTime = DBcons.GetComponent<LoginControl>().config.gridMoveCountdownTime;

        // Create the grid of targets.
        CreateTargets();

        GetActiveNode();

        // Allow first active target 'spawn'.
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("l")) AlterDB();

        //check to see if GameObject = null
        if (GameObject.Find("GloveRelative") != null)
        {
            // Let the grid start counting down before moving once the survivor has reached out a certain distance.
            if (GameObject.Find("GloveRelative").transform.position.z >= Depth - (DBcons.GetComponent<LoginControl>().config.gridMoveCountdownDistance / 1000.0f))
                minDistanceMet = true;
            else
                minDistanceMet = false;
        }
        // If the grid timer hasn't finished, keep waiting, else begin to move the grid closer.
        if(gridMoveWaitTimerDt > 0 && minDistanceMet && !gridReload)
        {
            gridMoveWaitTimerDt -= Time.deltaTime;

            if (gridMoveWaitTimerDt < 0)
            {
                gridMove = true;
                gridMoveWaitTimerDt = 0;
            }
        }
        
        if(gridMove && !gridReload && Depth > DBcons.GetComponent<LoginControl>().config.minExtensionThreshold / 1000.0f)
        {
            float movement = 0.01f * Time.deltaTime;
            Depth -= movement;

            // Make sure that the grid is exactly at the min extension threshold. Not closer by a small amount after the last increment.
            if (Depth < DBcons.GetComponent<LoginControl>().config.minExtensionThreshold / 1000.0f)
                Depth = DBcons.GetComponent<LoginControl>().config.minExtensionThreshold / 1000.0f;
        }
    }

    private void AlterDB()
    {
        if (DBcons)
        {
            string table = "Affliction";
            string fields = "Repetitions = " + 10;
            string where = "UserID =" + DBcons.GetComponent<LoginControl>().config.UserId;
            DBcons.GetComponent<database>().UpdateData(table, fields, where);
        }
    }

    /// <summary>
    /// Spawn (i.e make active) the currently active target (if it isn't already).
    /// 
    /// NOTE: The target will only be spawned if the 'canSpawn' flag is NOT true.
    /// </summary>
    public void SpawnTarget()
    {
        if(canSpawn)
            if (Score.GetComponent<ScoreCarnival>().Finished == false)
            {

                if (NodeList[ActiveNode].GetComponent<Node>().SpawnTarget())
                {
                    //Debug.Log("Spawned at " + NodeList[WhichNode].name);
                    Score.GetComponent<ScoreCarnival>().tPosX = NodeList[ActiveNode].transform.localPosition.x;
                    Score.GetComponent<ScoreCarnival>().tPosY = NodeList[ActiveNode].transform.localPosition.y;
                    ActiveNodeObject = NodeList[ActiveNode];
                    canSpawn = false;
                }
                else
                {
                    SpawnTarget();
                }
            }
	}

    /// <summary>
    /// Increase Depth.
    /// Sets the target backward if survivor is do well with final average of reaching for the targets.
    /// </summary>
    public void IncreaseDepth()
    {
        if (DBcons)
        {
            Debug.Log("INCREASING");
            float DepthIncrease = DBcons.GetComponent<LoginControl>().config.ExtensionThresholdIncrease * 0.001f;

            Debug.Log("DepthIncrease: " + DepthIncrease);
            Debug.Log("Depth: " + Depth);

            Depth += DepthIncrease;
            Debug.Log("New Depth: " + Depth);

            float DBdepth = Depth * 1000;
            Debug.Log("DBdepth: " + DBdepth);

            if (DBdepth <= DBcons.GetComponent<LoginControl>().config.armLength)
            {
                DBcons.GetComponent<LoginControl>().config.extensionThreshold = (int)DBdepth;

            }
            else if (DBdepth > DBcons.GetComponent<LoginControl>().config.armLength)
            {
                DBdepth = (int)DBcons.GetComponent<LoginControl>().config.armLength;
                Debug.Log("DBdepth altered to arm length: " + DBdepth);
            }



            string table = "TargetRestrictions";
            string fields = "ExtensionThreshold = " + DBdepth;
            string where = "UserID =" + DBcons.GetComponent<LoginControl>().config.UserId;
            DBcons.GetComponent<database>().UpdateData(table, fields, where);
        }
    }

    /// <summary>
    /// Decrease Depth.
    /// Sets the target forward if survivor is not do so well with final average of reaching for the targets.
    /// </summary>
    public void DecreaseDepth()
	{
		if (DBcons)
		{
			Debug.Log("DECREASING");
			float DepthIncrease = DBcons.GetComponent<LoginControl>().config.ExtensionThresholdIncrease * 0.001f;
			
            Debug.Log("DepthIncrease: " + DepthIncrease);
            Debug.Log("Depth: " + Depth);

            Depth -= DepthIncrease;
            Debug.Log("New Depth: " + Depth);

            float DBdepth = Depth * 1000;
            Debug.Log("DBdepth: " + DBdepth);

            if (DBdepth >= DBcons.GetComponent<LoginControl>().config.minExtensionThreshold)
            {
                DBcons.GetComponent<LoginControl>().config.extensionThreshold = (int)DBdepth;

            }
            else if (DBdepth < DBcons.GetComponent<LoginControl>().config.minExtensionThreshold)
            {
                DBdepth = (int)DBcons.GetComponent<LoginControl>().config.minExtensionThreshold;
                Debug.Log("DBdepth altered to arm length: " + DBdepth);
                
            }

            string table = "TargetRestrictions";
			string fields = "extensionThreshold = " + DBdepth;
			string where = "UserID =" + DBcons.GetComponent<LoginControl>().config.UserId;
			DBcons.GetComponent<database>().UpdateData(table, fields, where);
		}
	}

    /// <summary>
    /// Allow the next target to be spawned. (Seperate from load function in case the load function is looped).
    /// </summary>
    public void AllowNextTargetSpawn()
    {
        canSpawn = true;
        gridReload = false;
    }

    /// <summary>
    /// Get the currently active target / node.
    /// </summary>
    /// <returns>A reference to the currently active target / node.</returns>
    public GameObject GetActiveNode()
    {
        if (ActiveNodeObject == null)
        {
            try
            {
                ActiveNodeObject = NodeList[spawnOrder[0]];
            }
            catch
            {
                ActiveNodeObject = null;
            }
        }
        return ActiveNodeObject;
    }

    /// <summary>
    /// Create a new target node.
    /// </summary>
    /// <param name="col">The collumn of the grid that this node is in.</param>
    /// <param name="row">The row of the grid that this node is in.</param>
    /// <param name="pos">The position of the node relative to the grid object.</param>
    /// <param name="scale">The scale of the node relative to the grid object.</param>
    public void CreateNode(int col, int row, Vector3 pos, Vector3 scale)
    {
        //GameObject newNode = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject newNode = Instantiate(node, Vector3.zero, Quaternion.identity) as GameObject;
        //make childe of grid
        newNode.transform.parent = transform;
        //rename as location point
        newNode.name = "Node_" + col + "_" + row;

        // move the node
        newNode.transform.localScale = scale;
        newNode.transform.localPosition = pos;
        
        NodeList.Add(newNode);
    }

    /// <summary>
    /// Create a grid of targets in the game world, based on the grid dimension specifications.
    /// </summary>
    void CreateTargets()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // position the node on field
                // calc based on grid dimensions and scale parameter
                float adjX = (c - (cols - 1) * 0.5f) * gridScale.x;
                float adjY = (r - (rows - 1) * 0.5f) * gridScale.y;

                // In the case that either the grid's specified number of collumns or rows is only 1, ignore the below because it results in a divide by zero.
                if (cols > 1)
                    adjX /= ((cols - 1));

                if (rows > 1)
                    adjY /= ((rows - 1));

                CreateNode(c, r, new Vector3(adjX + gridOffset.x, adjY + gridOffset.y, Depth), targetScaleVector);
            }
        }
    }

    /// <summary>
    /// Destroy the grid of targets in the game world.
    /// </summary>
    public void DestroyTargets()
    {
        for (int i = 0; i < NodeList.Count; ++i)
        {
            Destroy(NodeList[i]);
        }

        NodeList.Clear();
    }

    /// <summary>
    /// Load the next target in the pre-defined spawn order list.
    /// </summary>
    public void LoadNextTargetInSpawnOrder()
    {
        if(canSpawn)
        {
            if (spawnOrderIndex >= spawnOrder.Count)
            {
                if (repeatSpawnOrder && repIndex < numRepetitions)
                {
                    spawnOrderIndex = 0;
                    repIndex++;
                    SetNodeAndSpawn();
                }
            }
            else
            {
                SetNodeAndSpawn();
            }
        }
    }

    /// <summary>
    /// Set the next node to be spawned, and spawn it. Also increment the spawnOrderIndex counter.
    /// </summary>
    private void SetNodeAndSpawn()
    {
        ActiveNode = spawnOrder[spawnOrderIndex];

        // If there is an error with ordering, correct them now before spawning the next target.
        if (ActiveNode < 0)
            ActiveNode = 0;
        else
            if (ActiveNode >= NodeList.Count)
            ActiveNode = NodeList.Count - 1;

        ActiveNodeObject = NodeList[ActiveNode];

        // Prime the spawn order index variable towards the next target.
        spawnOrderIndex++;

        if(canSpawn)
            gridMoveWaitTimerDt = gridMoveWaitTime;

        canSpawn = false;
    }

    /// <summary>
    /// Stop moving the grid, if it is currently moving.
    /// </summary>
    public void StopGridMovement()
    {
        gridMove = false;
        gridReload = true;
    }

    /// <summary>
    /// Using the recorded reach distances, calculate the average reach of the survivor during the game.
    /// </summary>
    public void RecalculateAverageReach()
    {
        // Add the current depth to the list.
        reachDistances.Add(Depth);

        float total = 0.0f;

        for (int i = 0; i < reachDistances.Count; i++)
            total += reachDistances[i];

        total /= reachDistances.Count;

        avgReach = total;

        // Reset the grid further back again, since the survivor might do better for the next punch.
        Depth += 0.02f;

        // First check that the grid is not furtheraway than the arm length of the survivor.
        if (Depth > (DBcons.GetComponent<LoginControl>().config.armLength) / 1000.0f)
            Depth = (DBcons.GetComponent<LoginControl>().config.armLength) / 1000.0f;

        // After the arm length check, check that the grid is also not further away than the specified extension threshold.
        if (Depth > DBcons.GetComponent<LoginControl>().config.extensionThreshold / 1000.0f)
            Depth = (DBcons.GetComponent<LoginControl>().config.extensionThreshold / 1000.0f);
    }
}
