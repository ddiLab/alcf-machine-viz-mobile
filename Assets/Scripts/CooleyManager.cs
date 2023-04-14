using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;


/// <summary>
/// Represents the Cooley supercomputer at Argonne National Laboratory, and provides
/// methods to be able to display the current running, queued, and reserved jobs in
/// an AR envrionment.
/// </summary>
public class CooleyManager : MonoBehaviour
{
    /// <summary>
    /// URL location for Cooley data.
    /// </summary>
    private const string DATA_URL = "https://status.alcf.anl.gov/cooley/activity.json";

    /// <summary>
    /// Specifies the height at which the image representing Cooley is placed in the physical world.
    /// </summary>
    private const float IMAGE_PHYSICAL_HEIGHT = 0.9f;

    /// <summary>
    /// Specifies the max height of one of the racks.
    /// </summary>
    private const float RACK_HEIGHT = 2.0f;

    /// <summary>
    /// Specifies the max width of one of the racks.
    /// </summary>
    private const float RACK_WIDTH = 0.7f;

    /// <summary>
    /// Vertical distance between nodes in one rack.
    /// </summary>
    private const float Y_STEP = 0.3f;

    /// <summary>
    /// Horizontal distance between nodes in one rack.
    /// </summary>
    private const float Z_STEP = 0.2f;

    /// <summary>
    /// Timer duration for fetching new data.
    /// </summary>
    private const float DATA_TIMER_VALUE = 10;

    /// <summary>
    /// Timer duration for displaying new color on active node in the legend.
    /// </summary>
    private const float LEGEND_ACTIVE_NODE_TIMER_VALUE = 1;


    /// <summary>
    /// Used to fetch data from a given URL.
    /// </summary>
    private WebClient request = new WebClient();

    /// <summary>
    /// JObject holding all information fetched for Cooley.
    /// </summary>
    private JObject fetchedData;

    /// <summary>
    /// JToken holding information for all of Cooleys nodes.
    /// </summary>
    private JToken nodesInfo;

    /// <summary>
    /// JToken holding infromation for all the currently running jobs on Cooley.
    /// </summary>
    private JToken runningJobsInfo;

    /// <summary>
    /// Jtoken holding information for all the currently queued jobs on Cooley.
    /// </summary>
    private JToken queuedJobsInfo;

    /// <summary>
    /// JToken holding infromation for all the currently reserved jobs on Cooley.
    /// </summary>
    private JToken reservedJobsInfo;

    /// <summary>
    /// GameObject holding the Content GameObject.
    /// </summary>
    /// <remarks>
    /// Used to be able to position the computer in the global AR environment.
    /// </remarks>
    private GameObject cooleyGO;

    /// <summary>
    /// GameObject holding the racks and nodes GameObjects.
    /// </summary>
    /// <remarks>
    /// Used to be able to position the racks and nodes in a more simple manner with local 
    /// positioning withing the cooleyGO.
    /// </remarks>
    private GameObject contentGO;

    /// <summary>
    /// Holds the GameObject that represents the legend for Cooley.
    /// </summary>
    private GameObject legendGO;

    /// <summary>
    /// Holds the position of the image representing Cooley in the phyiscal world, and is used as an offset 
    /// for positioning the computer.
    /// </summary>
    private Vector3 positionOffset;

    /// <summary>
    /// Holds the angle of the image representing Cooley in the phyiscal world, and is used as an offset for 
    /// rotating the computer.
    /// </summary>
    private Quaternion angleOffset;
    
    /// <summary>
    /// Holds the left over time of the timer to fetch new data.
    /// </summary>
    private float dataCountdown;

    /// <summary>
    /// Holds the left over time of the timer to display a new color on the active node in the legend.
    /// </summary>
    private float legendActiveNodeCountdown;


    /// <summary>
    /// Holds the Prefab that represents an active node.
    /// </summary>
    public GameObject activeNodePrefab;

    /// <summary>
    /// Holds the Prefab that represents an inactive node.
    /// </summary>
    public GameObject inactiveNodePrefab;

    /// <summary>
    /// Holds the Prefab that represents the legend for Cooley.
    /// </summary>
    public GameObject legendPrefab;

    /// <summary>
    /// Holds the Prefab that represents a visible rack of Cooley.
    /// </summary>
    public GameObject visibleRackPrefab;

    /// <summary>
    /// Holds the Prefab that represents an invisible rack of Cooley.
    /// </summary>
    public GameObject invisibleRackPrefab;

    /// <summary>
    /// Holds the prefab that represents a running job button for the jobs menu.
    /// </summary>
    public GameObject runningJobButtonPrefab;

    /// <summary>
    /// Holds the GameObject that has all the running jobs as children for the scroll view.
    /// </summary>
    public GameObject runningJobsContentGO;

    /// <summary>
    /// Holds the button that let's a user display all the nodes after filtering them based on job ids.
    /// </summary>
    public GameObject showAllNodesButtonGO;

    ///
    ///
    ///
    public SettingsMenuManager settingsMenuManager;


    ///
    ///
    ///
    public JobsMenuManager jobsMenuManager;

    /// <summary>
    /// Holds a dictionary of all the GameObjects that represent a node in Cooley where a key is the node ID and the value is the
    /// GameObject representing that node ID.
    /// </summary>
    public Dictionary<string, GameObject> nodesDict = new Dictionary<string, GameObject>();


    public SortedDictionary<string, Dictionary<string, string>> runningJobsDict = new SortedDictionary<string, Dictionary<string, string>>();

    /// <summary>
    /// Holds an array of all the GameObjects that represent an invisible rack of Cooley.
    /// </summary>
    public GameObject[] visibleRacksArr;

    /// <summary>
    /// Holds an array of all the GameObjects that represent a visible rack of Cooley.
    /// </summary>
    public GameObject[] invisibleRacksArr;

    /// <summary>
    /// Number of racks that Cooley has.
    /// </summary>
    public int numOfRacks;

    /// <summary>
    /// Number of nodes that are present in each rack.
    /// </summary>
    public int nodesPerRack;


    /// <summary>
    /// Indicates whether the racks of the computer are currently displayed virtually in the AR environment.
    /// </summary>
    public bool RacksVisible { get; private set; }


    // Start is called before the first frame update
    // Used to initialize the coundown timers and visiblity of GameObjects.
    void Start()
    {
        RacksVisible = true;

        // Disables the toggle racks button in settings when the app starts
        settingsMenuManager.SetToggleRacksButtonInteractable(false);
        
        jobsMenuManager.SetActiveJobsMenuButton(false);
        jobsMenuManager.SetActiveShowAllNodesButton(false);


        //legendActiveNodeCountdown = LEGEND_ACTIVE_NODE_TIMER_VALUE;

        // Hide show all nodes button in job menu since all nodes are show initially
        //showAllNodesButtonGO.SetActive(false);
    }


    // Update is called once per frame
    // Used to update countdown timers and Cooley data.
    void Update()
    {
        if (GameObject.Find("Cooley") != null)
        {
            // Decrements countdown timer for the data fetching
            if (dataCountdown > 0)
            {
                dataCountdown -= Time.deltaTime;
            }

            // Updates data and resets timer
            if (dataCountdown <= 0)
            {
                // Resets countdown timer for us
                GetData();

                UpdateNodesColors();

                UpdateJobsMenu();
            }


            // Decrements countdown timer for updating the legend
            /*
            if (legendActiveNodeCountdown > 0)
            {
                legendActiveNodeCountdown -= Time.deltaTime;
            }

            // Updates legend active node color and resets timer
            if (legendActiveNodeCountdown <= 0)
            {
                Color nextColor = Random.ColorHSV(); // Random new color

                // Ensures the color isn't white
                if (nextColor.Equals(Color.white))
                    nextColor = Random.ColorHSV();
           
                legendActiveNodeCountdown = LEGEND_ACTIVE_NODE_TIMER_VALUE;

                legendGO.transform.Find("Content").Find("Nodes").Find("AllocatedNode").Find("Node").GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            }
            */
                
        }
    }


    /// <summary>
    /// Fetches the data for Cooley using the URL provided in the class members.
    /// </summary>
    /// <remarks>
    /// Resets the countdown timer withing the script in order to avoid situations where this method gets called
    /// at the same time by different scripts.
    /// </remarks>
    public void GetData()
    {
        // Fetches fetchedData
        string resultJson = request.DownloadString(DATA_URL);

        dataCountdown = DATA_TIMER_VALUE;
        Debug.Log("Timer reset");

        // Saves it in the class variable
        fetchedData = JsonConvert.DeserializeObject<JObject>(resultJson);

        // Checks if the computer is NOT under maintenance
        if (fetchedData.GetValue("maint") == null)
        {
            // Saves information about the computer
            numOfRacks = (int) fetchedData["dimensions"]["racks"];
            nodesPerRack = (int) fetchedData["dimensions"]["nodecards"];

            // Saves all info on nodes
            nodesInfo = fetchedData["nodeinfo"];

            // Saves all info on running jobs
            runningJobsInfo = fetchedData["running"];

            // Saves all info on queued jobs
            queuedJobsInfo = fetchedData["queued"];

            // Saves all info on reserved jobs
            reservedJobsInfo = fetchedData["reservation"];

            Debug.Log(fetchedData["queued"]);
            Debug.Log(fetchedData["reservation"]);
        }
    }


    /// <summary>
    /// Checks if Cooley is currently running jobs.
    /// </summary>
    /// <returns>
    /// True if Cooley is not under maintenance. Otherwise, false.
    /// </returns>
    /// <remarks>
    /// Calls GetData() in order to get the most updated data for Cooley.
    /// </remarks>
    public bool IsMachineRunning()
    {
        // Checks if cooley is under maintenance
        if (fetchedData.GetValue("maint") != null)
        {
            // Disables the toggle racks button in settings
            settingsMenuManager.SetToggleRacksButtonInteractable(false);
            return false;
        }

        // Enables the toggle racks button in settings
        settingsMenuManager.SetToggleRacksButtonInteractable(true);
        return true;
    }


    /// <summary>
    /// Creates clones of Prefabs that present nodes of Cooley and the racks of Cooley, while also
    /// saving their GameObjects in order to position and rotate them later.
    /// <para>
    /// It creates empty GameObjects called "Cooley" and "Content". "Content" stores the GameObject
    /// clones of all the racks and nodes of Cooley, and it is used later to make it easier to position the
    /// nodes and racks correctly using local position. Furthermore, "Content" is placed as a child within
    /// "Cooley" and "Cooley" is used to position in the global AR envrionment, and also to correctly angle
    /// the rest of the GameObjects that represent the computer.
    /// </para>
    /// </summary>
    public void CreateGameObjects()
    {
        string nodeId;    // Holds the ID of the current node
        string nodeState; // Holds the state of the current node


        // Creates GameObjects used for positioning and rotating purposes
        cooleyGO = new GameObject("Cooley");

        contentGO = new GameObject("Content");
        contentGO.AddComponent<MeshRenderer>();
        contentGO.transform.parent = cooleyGO.transform;

        /*
        legendGO = Instantiate(legendPrefab, new Vector3(0.5f, RACK_HEIGHT, -0.4f), Quaternion.identity);
        legendGO.transform.name = "Legend";
        legendGO.transform.eulerAngles = new Vector3(0, -90, 0);
        legendGO.transform.localScale = new Vector3(4, 4, 1);
        legendGO.transform.parent = contentGO.transform;
        */

        visibleRacksArr = new GameObject[numOfRacks];
        invisibleRacksArr = new GameObject[numOfRacks];

        // Clones the racks Prefab depending on how many racks the computer has
        for (int i = 0; i < numOfRacks; i++)
        {
            visibleRacksArr[i] = Instantiate(visibleRackPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            invisibleRacksArr[i] = Instantiate(invisibleRackPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            visibleRacksArr[i].transform.name = "Visible Rack" + i;
            invisibleRacksArr[i].transform.name = "Invisible Rack" + i;


            // Rotates racks to face towards perpendicular to the image representing Cooley
            visibleRacksArr[i].transform.eulerAngles = new Vector3(0, 90, 0);
            invisibleRacksArr[i].transform.eulerAngles = new Vector3(0, 90, 0);

            // Places the racks object as a child of the "Content" GameObject
            visibleRacksArr[i].transform.parent = contentGO.transform;
            invisibleRacksArr[i].transform.parent = contentGO.transform;

            visibleRacksArr[i].SetActive(RacksVisible);
            invisibleRacksArr[i].SetActive(!RacksVisible);
        }

   
        foreach (var node in nodesInfo)
        {
            // Ommits the .cooley part of the node ID
            nodeId = node.ToString().Substring(1, 5);

            // Grabs the state of the current node
            nodeState = ((JObject) node.First)["state"].ToString();

            if (nodeState.Equals("idle") || nodeState.Equals("allocated"))
            {
                // The current node is active, and is either iddling or has a job allocated
                nodesDict[nodeId] = Instantiate(activeNodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                // The current node is inactive ("down")
                nodesDict[nodeId] = Instantiate(inactiveNodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }

            nodesDict[nodeId].transform.name = nodeId;

            // Rotates nodes to face perpendicular to the image represneting Cooley
            nodesDict[nodeId].transform.eulerAngles = new Vector3(0, -90, 0);

            nodesDict[nodeId].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

            // Places the node object as a child of the "Content" GameObject
            nodesDict[nodeId].transform.parent = contentGO.transform;
        }

        CreateJobsMenu();
    }


    /// <summary>
    /// Positions the racks and nodes of Cooley in respect to the image that represents Cooley.
    /// <para>
    /// The racks and nodes are always placed 2 meters left of the image, and 
    /// facing perpendicular to the images angle.
    /// </para>
    /// </summary>
    /// <param name="image">Transform component of the image detected that represents the Cooley computer.</param>
    public void UpdateGameObjects(Transform imageTF)
    {
        string nodeId;                // Holds the ID of the current node
        //string nodeState;             // Holds the state of the current node
        int rowNum = 0;               // Holds the number of the current row that the current node is being placed in
        int colNum = 0;               // Holds the number of the current column that the current node is being placed in
        int rackNum = 0;              // Holds the number of the current rack that nodes are being placed in
        float currentY = RACK_HEIGHT; // Holds the current 'y' (vertical) positioning of the current node
        float currentZ = 0;           // Holds the current 'z' (horizontal) positioning of the current node

        // Saves the position and rotation of the image and uses it as an offset
        positionOffset = imageTF.position;
        angleOffset = imageTF.rotation;

        // Initially sets the "Cooley" GameObject to be placed on top of the image and holding the same rotation
        cooleyGO.transform.position = positionOffset;
        cooleyGO.transform.eulerAngles = angleOffset.eulerAngles;

        // Moves the "Content" GameObject left of the image 2 meters, and adjusts the vertical position, in order for the
        // racks and nodes to be level wit the physical ground
        contentGO.transform.localPosition = new Vector3(-2, -IMAGE_PHYSICAL_HEIGHT, -IMAGE_PHYSICAL_HEIGHT);


        for (int i = 0; i < numOfRacks; i++)
        {
            // Seperates the racks to be placed in a row next to each other
            visibleRacksArr[i].transform.localPosition = new Vector3(0, 0, i * RACK_WIDTH);
            invisibleRacksArr[i].transform.localPosition = new Vector3(0, 0, i * RACK_WIDTH);
        }


        foreach (var node in nodesInfo)
        {

            // Jumps to the next row of the current rack
            if (colNum == 3)
            {
                colNum = 0;

                // Resets the horizontal positioning
                currentZ = 0 + (rackNum * RACK_WIDTH);
                
                // Moves vertically down to the next row
                currentY -= Y_STEP;

                // Updates the row counter
                rowNum++;
            }

            // Jumps to the next rack
            if (rowNum == 7)
            {
                // Resets row counter for next rack
                rowNum = 0;
                rackNum++;

                // Resets the vertical positioning
                currentY = RACK_HEIGHT;

                // Moves to the first column of the current rack
                currentZ = 0 + (rackNum * RACK_WIDTH);
            }


            // Ommits the .cooley part of the node ID
            nodeId = node.ToString().Substring(1, 5);

            //nodeState = ((JObject)node.First)["state"].ToString();

            // Positions all active nodes in their current position
            //if (!nodeState.Equals("down"))
            //{
                nodesDict[nodeId].transform.localPosition = new Vector3(-0.2f, currentY, currentZ + 0.1f); // Adds 0.1 meters between each nodes in a rack
                nodesDict[nodeId].transform.Find("Id").GetComponent<TextMeshPro>().text = "\n\n" + nodeId;
            //}

            // Increments the column and the horizontal positioning
            colNum++;
            currentZ += Z_STEP;
        }

        UpdateNodesColors();
        
        // Rotates the "Cooley" GameObject to always be facing to the right (perpendicular to the images rotation) no matter how the image is rotates in the 'y'.
        cooleyGO.transform.localEulerAngles = new Vector3(0, imageTF.transform.localEulerAngles.x > 90 ? Mathf.Abs(imageTF.transform.localEulerAngles.y) + Mathf.Abs(imageTF.transform.localEulerAngles.z) : imageTF.transform.localEulerAngles.y - imageTF.transform.localEulerAngles.z, 0);
    }


    /// <summary>
    /// Updates the color of each node depending on the current job being ran on the node.
    /// </summary>
    /// <remarks>
    /// Nodes that have a status of 'allocated' have a field that has a hex representation of the color that represents 
    /// the job that is ran on the node, in some cases when a new job gets nodes allocated to it, it doesn't always
    /// assign a hex color field for the 'allocated' node and it doesn't appear in the runningJobs variable since the job
    /// has not been started yet, and this function checks for this error.
    /// If the error occurs then the color assigned to the node is the same as if the node is in 'idle' status, since the job
    /// hasn't started yet.
    /// </remarks>
    private void UpdateNodesColors()
    {
        Color nodeColor;  // Holds the RGBA representation of the color for the current node
        string nodeId;    // Holds the ID of the current node
        string nodeState; // Holds the state of the current node


        foreach (var node in nodesInfo)
        {
            // Ommits the .cooley part of the node ID
            nodeId = node.ToString().Substring(1, 5);

            nodeState = ((JObject)node.First)["state"].ToString();

            // Checks if the node is an active node ('allocated' or 'idle')
            if (!nodeState.Equals("down"))
            {

                if (nodeState.Equals("allocated") && ((JObject)node.First)["color"] != null)
                {
                    // The current node is in 'allocated' status and also has a valid hex color specified in the data fetched
                    ColorUtility.TryParseHtmlString(((JObject)node.First)["color"].ToString(), out nodeColor);
                }
                else
                {
                    // The current node is in 'idle' status
                    ColorUtility.TryParseHtmlString("#FFFFFF", out nodeColor);
                }
                    
                nodesDict[nodeId].transform.Find("Node").GetComponent<MeshRenderer>().material.color = nodeColor;
            }
        }
    }


    private bool JobMenuCheckUpdate()
    {
        bool jobIdFound = false;

        foreach (var currJob in runningJobsInfo)
        {
            foreach (var (currJobId, currJobInfo) in runningJobsDict)
            {
                if (currJob["jobid"].ToString().Equals(currJobId))
                {
                    jobIdFound = true;
                }
            }
            
            if (!jobIdFound)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Starts the creation of the Jobs Menu that lets the user browse through the running, queued, and reserved jobs.
    /// </summary>
    public void CreateJobsMenu()
    {
        GameObject newRunningJobButtonGO;
        Dictionary<string, string> tempDict = new Dictionary<string, string>();
        Color buttonColor;

        tempDict.Clear();
        foreach (var currJob in runningJobsInfo)
        {
            tempDict = new Dictionary<string, string>();

            tempDict.Add("projectName", currJob["project"].ToString());
            tempDict.Add("hexColor", currJob["color"].ToString());

            runningJobsDict.Add(currJob["jobid"].ToString(), tempDict);
        }

        foreach (var (currJobId, currJobInfo) in runningJobsDict)
        {
            //Debug.Log("runningJobsDict key: " + currJobId);
            //Debug.Log(currJobInfo["projectName"]);
            //Debug.Log(currJobInfo["hexColor"]);


            newRunningJobButtonGO = Instantiate(runningJobButtonPrefab, runningJobsContentGO.transform);
            newRunningJobButtonGO.transform.name = currJobId;

            ColorUtility.TryParseHtmlString(currJobInfo["hexColor"], out buttonColor);

            newRunningJobButtonGO.transform.GetComponent<Image>().color = buttonColor;

            newRunningJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobId + "\n" + currJobInfo["projectName"];
            
            newRunningJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => FilterNodeGameObjectsByJobId(currJobId));
        }

        jobsMenuManager.SetActiveJobsMenuButton(true);

        // Work on updating the nodes, checking if updating is needed
        // Add a script to manage the job menu, opening closing etc.
        // Work on adding extra buttons for job menu, and show all nodes one too

    }

    /// <summary>
    /// Saves all the running jobs and their identifying colors, and uses the
    /// JobsMenuController script to initialize the menu if 'initializeMenu' = true,
    /// otherwise it gives the script updated data on the current jobs.
    /// </summary>
    /// <param name="initializeMenu"></param>
    private void UpdateJobsMenu()
    {
        if (!JobMenuCheckUpdate())
        {
            Debug.Log("No need to update Jobs Menu");
            return;
        }
        Debug.Log("Update needed for Jobs Menu");
        CreateJobsMenu();
    }


    /// <summary>
    /// Checks the current visibility of the racks and saves the opposite value,
    /// while also updating the racks of their current visibility status.
    /// </summary>
    public void ToggleRacks()
    {
        // Upadate racks visibility
        RacksVisible = !RacksVisible;

        for (int i = 0; i < numOfRacks; i++)
        {
            visibleRacksArr[i].SetActive(RacksVisible);
            invisibleRacksArr[i].SetActive(!RacksVisible);
        }
    }


    /// <summary>
    /// Filters all the nodes to only show the ones that are allocated to the specified 'jobId'.
    /// </summary>
    /// <param name="jobId">The job ID to look for while filtering.</param>
    public void FilterNodeGameObjectsByJobId(string jobId)
    {
        List<string> nodesToShow = new List<string>(); // Holds a list of all the nodes that are allocated to the given job ID
        

        foreach(var job in runningJobsInfo)
        {
            // Checks if the current job ID is the given job ID
            if (job["jobid"].ToString().Equals(jobId))
            {
                // Grabs the 'location' field from the data that holds a list of all the nodes allocated to the given job ID
                foreach(var node in job["location"])
                {
                    nodesToShow.Add(node.ToString().Substring(0, 5));
                }

                // No need to continue loop since all the nodes have been found
                break;
            }
        }


        // Makes all of the nodes invisible
        foreach (var nodeGameObject in nodesDict)
        {
            // Accounts for nodes without a gameobject
            if(nodeGameObject.Value != null)
                nodeGameObject.Value.SetActive(false);
        }

        // Makes the nodes for the given job ID visibile only
        foreach(var node in nodesToShow)
        {
            nodesDict[node].SetActive(true);
        }

        jobsMenuManager.SetActiveShowAllNodesButton(true);
    }

    /// <summary>
    /// Ensures that all the node GameObjects are visible.
    /// </summary>
    public void ShowAllNodeGameObjects()
    {
        foreach (var nodeGameObject in nodesDict)
        {
            // Accounts for nodes without a gameobject
            if (nodeGameObject.Value != null)
                nodeGameObject.Value.SetActive(true);
        }

        // Hides the Show All Nodes button, since all nodes are now visibile
        jobsMenuManager.SetActiveShowAllNodesButton(false);
    }
}
