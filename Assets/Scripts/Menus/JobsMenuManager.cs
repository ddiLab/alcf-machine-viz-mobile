using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;


/// <summary>
/// Manages the Jobs menu visibility, interactivity, and functionality. Providing 
/// functions that change the visible menus and scroll views in the menu. As well as,
/// going back to the previous menu's to be able to interact even further with the data.
/// </summary>
public class JobsMenuManager : MonoBehaviour
{
    /// <sumarry>
    /// Holds the script that manages the Cooley visualization.
    /// </summary>
    [SerializeField]
    private CooleyManager cooleyManager;

    /// <summary>
    /// Holds the main panel GameObject for the Jobs menu.
    /// </summary>
    [SerializeField]
    private GameObject panelGO;

    /// <summary>
    /// Holds the panel that displays info about a running job that has been filtered.
    /// </summary>
    [SerializeField]
    private GameObject filteredRunningJobPanelGO;

    /// <summary>
    /// Holds the panel that displays info about a queued job.
    /// </summary>
    [SerializeField]
    private GameObject queuedJobPanelGO;

    /// <summary>
    /// Holds the panel that displays info about a reserved job.
    /// </summary>
    [SerializeField]
    private GameObject reservedJobPanelGO;

    /// <summary>
    /// Holds the button that opens the main panel of the Job menu.
    /// </summary>
    [SerializeField]
    private GameObject openButtonGO;

    /// <summary>
    /// Holds the button that resets all the nodes to be visible, after filtering.
    /// </summary>
    [SerializeField]
    private GameObject showAllNodesButtonGO;

    /// <summary>
    /// Holds the button that lets you select to view all the running jobs in the main panel.
    /// </summary>
    [SerializeField]
    private GameObject viewRunningJobsButtonGO;

    /// <summary>
    /// Holds the button that lets you select to view all the queued jobs in the main panel.
    /// </summary>
    [SerializeField]
    private GameObject viewQueuedJobsButtonGO;

    /// <summary>
    /// Holds the button that lets you select to view all the reserved jobs in the main panel.
    /// </summary>
    [SerializeField]
    private GameObject viewReservedJobsButtonGO;

    /// <summary>
    /// Holds the scroll view in which all the running jobs are displayed on.
    /// </summary>
    [SerializeField]
    private GameObject runningJobsScrollViewGO;

    /// <summary>
    /// Holds the scroll view in which all the queued jobs are displayed on.
    /// </summary>
    [SerializeField]
    private GameObject queuedJobsScrollViewGO;

    /// <summary>
    /// Holds the scroll view in which all the reserved jobs are displayed on.
    /// </summary>
    [SerializeField]
    private GameObject reservedJobsScrollViewGO;

    /// <summary>
    /// Holds the content GameObject of the runningJobsScrollViewGO.
    /// </summary>
    [SerializeField]
    private GameObject runningJobsContentGO;

    /// <summary>
    /// Holds the content GameObject of the queuedJobsScrollViewGO.
    /// </summary>
    [SerializeField]
    private GameObject queuedJobsContentGO;

    /// <summary>
    /// Holds the content GameObject of the reservedJobsScrollViewGO.
    /// </summary>
    [SerializeField]
    private GameObject reservedJobsContentGO;

    /// <summary>
    /// Holds the GameObject that is displayed if there are no running jobs at the moment.
    /// </summary>
    [SerializeField]
    private GameObject noRunningJobsTextGO;

    /// <summary>
    /// Holds the GameObject that is displayed if there are no queued jobs at the moment.
    /// </summary>
    [SerializeField]
    private GameObject noQueuedJobsTextGO;

    /// <summary>
    /// Holds the GameObject that is displayed if there are no reserved jobs at the moment.
    /// </summary>
    [SerializeField]
    private GameObject noReservedJobsTextGO;

    /// <summary>
    /// Holds the title text for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelTitleText;

    /// <summary>
    /// Holds the first runtime text slot for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelRunTimeText_1;

    /// <summary>
    /// Holds the second runtime text slot for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelRunTimeText_2;

    /// <summary>
    /// Holds the walltime text for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelWalltimeText;

    /// <summary>
    /// Holds the maximum walltime text for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private Slider filteredRunningPanelMaxWalltimeProgresBar;

    /// <summary>
    /// Holds the assigned walltime text for the filteredRunningJobPanelGO.
    /// </summary>
    [SerializeField]
    private Slider filteredRunningPanelJobWalltimeProgresBar;

    /// <summary>
    /// Holds the title text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelTitleText;

    /// <summary>
    /// Holds the queue text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelQueueText;

    /// <summary>
    /// Holds the node partitions text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelPartitionsText;

    /// <summary>
    /// Holds the start time text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelStartTimeText;

    /// <summary>
    /// Holds the time duration text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelDurationText;

    /// <summary>
    /// Holds the T-minus text for the reservedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI reservedPanelTMinusText;

    /// <summary>
    /// Holds the title text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelTitleText;

    /// <summary>
    /// Holds the score text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelScoreText;

    /// <summary>
    /// Holds the queue text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelQueueText;

    /// <summary>
    /// Holds the walltime text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelWalltimeText;

    /// <summary>
    /// Holds the queued time text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelQueuedTimeText;

    /// <summary>
    /// Holds the number of nodes text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelNodesText;

    /// <summary>
    /// Holds the job mode text for the queuedJobPanelGO.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI queuedPanelModeText;

    /// <summary>
    /// Holds the prefab button that represents a running job in the scroll view.
    /// </summary>
    [SerializeField]
    private GameObject runningJobButtonPrefab;

    /// <summary>
    /// Holds the prefab button that represents a queued job in the scroll view.
    /// </summary>
    [SerializeField]
    private GameObject queuedJobButtonPrefab;

    /// <summary>
    /// Holds the prefab button that represents a reserved job in the scroll view.
    /// </summary>
    [SerializeField]
    private GameObject reservedJobButtonPrefab;


    /// <summary>
    /// Start is called before the first frame update. Initializes the Jobs menu to its
    /// inital state.
    /// </summary>
    void Start()
    {
        // Selects running jobs to be shown in the job menu by default
        ChangeJobTypeView("running");
    }


    /// <summary>
    /// Opens the Jobs menu panel, and hides the Jobs menu button.
    /// </summary>
    public void OpenJobsMenu()
    {
        panelGO.SetActive(true);
        openButtonGO.SetActive(false);
    }


    /// <summary>
    /// Closes the Jobs menu panel, and shows the Jobs menu button.
    /// </summary>
    public void CloseJobsMenu()
    {
        panelGO.SetActive(false);
        openButtonGO.SetActive(true);
    }
    

    /// <summary>
    /// Shows the job info panel for a filtered running job.
    /// </summary>
    public void OpenFilteredRunningJobsInfoPanel()
    {
        // Closes the Jobs menu
        CloseJobsMenu();


        filteredRunningJobPanelGO.SetActive(true);
    }


    /// <summary>
    /// Hides the job info panel for a filtered running job, and goes back to
    /// the Jobs menu panel.
    /// </summary>
    public void CloseFilteredRunningJobsInfoPanel()
    {
        filteredRunningJobPanelGO.SetActive(false);
        OpenJobsMenu();

        // Updates the Cooley manager to know that there isn't any info panel open so that
        // it doesn't update them in it's main loop
        cooleyManager.CurrentInfoPanel["type"] = "none";
        cooleyManager.CurrentInfoPanel["id"] = "none";
    }


    /// <summary>
    /// Shows the job info panel for a queued job.
    /// </summary>
    public void OpenQueuedJobsInfoPanel()
    {
        queuedJobPanelGO.SetActive(true);
    }


    /// <summary>
    /// Hides the job info panel for a queued job, and goes back to the Jobs menu panel.
    /// </summary>
    public void CloseQueuedJobsInfoPanel()
    {
        queuedJobPanelGO.SetActive(false);

        // Updates the Cooley manager to know that there isn't any info panel open so that
        // it doesn't update them in it's main loop
        cooleyManager.CurrentInfoPanel["type"] = "none";
        cooleyManager.CurrentInfoPanel["id"] = "none";
    }


    /// <summary>
    /// Shows the job info panel for a reserved job.
    /// </summary>
    public void OpenReservedJobsInfoPanel()
    {
        reservedJobPanelGO.SetActive(true);
    }


    /// <summary>
    /// Hides the job info panel for a reserved job, and goes back to the Jobs menu panel.
    /// </summary>
    public void CloseReservedJobsInfoPanel()
    {
        reservedJobPanelGO.SetActive(false);

        // Updates the Cooley manager to know that there isn't any info panel open so that
        // it doesn't update them in it's main loop
        cooleyManager.CurrentInfoPanel["type"] = "none";
        cooleyManager.CurrentInfoPanel["id"] = "none";
    }


    /// <summary>
    /// Shows the Jobs menu button when Cooley is detected, or when the menu is closed.
    /// </summary>
    public void SetActiveJobsMenuButton(bool newStatus)
    {
        openButtonGO.SetActive(newStatus);
    }

    public void SetActiveShowAllNodesButton(bool newStatus)
    {
        showAllNodesButtonGO.SetActive(newStatus);
    }


    /// <summary>
    /// Changes the visibility of the scroll views in the Job menu, depending on the view
    /// the user selected.
    /// </summary>
    /// <param name="jobType">The desired scroll view job type to be visible.</param>
    public void ChangeJobTypeView(string jobType)
    {
        if (jobType.Equals("running"))
        {
            // Makes the running jobs button not interactable, and leaves the others
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = false;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = true;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = true;

            // Shows all the running jobs buttons
            runningJobsScrollViewGO.SetActive(true);
            queuedJobsScrollViewGO.SetActive(false);
            reservedJobsScrollViewGO.SetActive(false);
        }
        else if (jobType.Equals("queued"))
        {
            // Makes the queued jobs button not interactable, and leaves the others
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = true;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = false;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = true;

            // Shows all the queued jobs buttons
            runningJobsScrollViewGO.SetActive(false);
            queuedJobsScrollViewGO.SetActive(true);
            reservedJobsScrollViewGO.SetActive(false);
        }
        else if (jobType.Equals("reserved"))
        {
            // Makes the reserved jobs button not interactable, and leaves the others
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = true;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = true;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = false;

            // Shows all the reserved jobs buttons
            runningJobsScrollViewGO.SetActive(false);
            queuedJobsScrollViewGO.SetActive(false);
            reservedJobsScrollViewGO.SetActive(true);
        }
    }


    /// <summary>
    /// Updates the buttons in the running jobs scroll view, if there is any. Otherwise,
    /// it displays a message that notifies the user there are no jobs at the moment.
    /// </summary>
    /// <param name="newJobsDict">
    /// A 2D dictionary that holds the job ID of the jobs as the key, and the second dictionary holds
    /// the project name, and the job color of the job.
    /// </param>
    public void UpdateRunningJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        // Checks if there are no jobs running
        if (newJobsDict.Count == 0)
        {
            // Displays no jobs text and terminates further execution of the function
            noRunningJobsTextGO.SetActive(true);
            return;
        }

        GameObject newRunningJobButtonGO; //< Holds the button GameObject of the current job
        Color buttonColor;                //< Holds the color of the current job

        noRunningJobsTextGO.SetActive(false);

        // Loops through each of the jobs in the dictionary
        foreach (var (currJobId, currJobInfo) in newJobsDict)
        {
            // Creates a new button for the current job and places it as a child in the scroll views content GO
            newRunningJobButtonGO = Instantiate(runningJobButtonPrefab, runningJobsContentGO.transform);
            newRunningJobButtonGO.transform.name = currJobId;

            // Converts the hex color of the job to a Color object
            ColorUtility.TryParseHtmlString(currJobInfo["hexColor"], out buttonColor);
            newRunningJobButtonGO.transform.GetComponent<Image>().color = buttonColor;

            // Updates the button text
            newRunningJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobId + "\n" + currJobInfo["projectName"];
            
            // Adds an event listener to the button that calls the FilterNodeGameObjectsByJobId method from CooleyManager
            newRunningJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.FilterNodeGameObjectsByJobId(currJobId));
        }
    }


    /// <summary>
    /// Updates the buttons in the queued jobs scroll view, if there is any. Otherwise,
    /// it displays a message that notifies the user there are no jobs at the moment.
    /// </summary>
    /// <param name="newJobsDict">
    /// A 2D dictionary that holds the job ID of the jobs as the key, and the second dictionary holds
    /// the project name, and the score of the job.
    /// </param>
    public void UpdateQueuedJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        // Checks if there are no jobs running
        if (newJobsDict.Count == 0)
        {   
            // Displays no jobs text and terminates further execution of the function
            noQueuedJobsTextGO.SetActive(true);
            return;
        }

        GameObject newQueuedJobButtonGO; //< Holds the button GameObject of the current job

        noQueuedJobsTextGO.SetActive(false);

        // Loops through each of the jobs in the dictionary
        foreach (var (currJobId, currJobInfo) in newJobsDict)
        {   
            // Creates a new button for the current job and places it as a child in the scroll views content GO
            newQueuedJobButtonGO = Instantiate(queuedJobButtonPrefab, queuedJobsContentGO.transform);
            newQueuedJobButtonGO.transform.name = currJobId;

            // Updates the button text
            newQueuedJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobId + "\n" + currJobInfo["projectName"];

            // Adds an event listener to the button that calls the ShowQueuedJobInfo method from CooleyManager
            newQueuedJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.ShowQueuedJobInfo(currJobId));    
        }
    }


    /// <summary>
    /// Updates the buttons in the reserved jobs scroll view, if there is any. Otherwise,
    /// it displays a message that notifies the user there are no jobs at the moment.
    /// </summary>
    /// <param name="newJobsDict">
    /// A 2D dictionary that holds the job name of the jobs as the key, and the second dictionary holds
    /// the queued, and the node partitions of the job.
    /// </param>
    public void UpdateReservedJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        // Checks if there are no jobs running
        if (newJobsDict.Count == 0)
        {
            // Displays no jobs text and terminates further execution of the function
            noReservedJobsTextGO.SetActive(true);
            return;
        }

        GameObject newReservedJobButtonGO; //< Holds the button GameObject of the current job

        noReservedJobsTextGO.SetActive(false);  

        // Loops through each of the jobs in the dictionary
        foreach (var (currJobName, currJobInfo) in newJobsDict)
        {
            // Creates a new button for the current job and places it as a child in the scroll views content GO
            newReservedJobButtonGO = Instantiate(reservedJobButtonPrefab, reservedJobsContentGO.transform);
            newReservedJobButtonGO.transform.name = currJobName;

            // Updates button text
            newReservedJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobName + "\n" + currJobInfo["nodePartitions"];
            
            // Adds an event listener to the button that calls the ShowReservedJobInfo method from CooleyManager
            newReservedJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.ShowReservedJobInfo(currJobName));
        }
    }


    /// <summary>
    /// Updates the text on the filtered running jobs info panel, with the given data. In addition,
    /// it updates the two graphs displaying the time the job has been running compared to the max
    /// walltime and the assigned walltime.
    /// </summary>
    /// <param name="jobId">The job ID of the job.</param>
    /// <param name="projectName">The project name of the job.</param>
    /// <param name="runTime">The current run time of the job.</param>
    /// <param name="walltime">The assigned walltime of the job.</param>
    public void UpdateFilteredRunningJobPanel(string jobId, string projectName, string runTime, string walltime)
    {
        DateTime currentRunTime = DateTime.ParseExact(runTime, "HH:mm:ss", CultureInfo.InvariantCulture); //< Holds the current run time converted to a DateTime object
        DateTime maxWalltime = DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture); //< Holds the maximum walltime converted to a DateTime object 
        DateTime jobWalltime = DateTime.ParseExact(walltime, "HH:mm:ss", CultureInfo.InvariantCulture);   //< Holds the assigned walltime to the job converted to a DateTime object

        // Tells the CooleyManager that the running job info panel is open, so that it updates it every time new data is fetched
        cooleyManager.CurrentInfoPanel["type"] = "running";
        cooleyManager.CurrentInfoPanel["id"] = jobId;

        // Updates all the text fields in the info panel
        filteredRunningPanelTitleText.text = jobId + "\n" + projectName;
        filteredRunningPanelRunTimeText_1.text = runTime;
        filteredRunningPanelRunTimeText_2.text = runTime;
        filteredRunningPanelWalltimeText.text = walltime;

        // Updates the value of the progres bar visualization that visualizes the current run time versus max walltime
        filteredRunningPanelMaxWalltimeProgresBar.value = (float) (currentRunTime.TimeOfDay.TotalMilliseconds / maxWalltime.TimeOfDay.TotalMilliseconds);
        
        // Updates the value of the progres bar visualization that visualizes the current run time versus assigned walltime
        filteredRunningPanelJobWalltimeProgresBar.value = (float) (currentRunTime.TimeOfDay.TotalMilliseconds / jobWalltime.TimeOfDay.TotalMilliseconds);
    }


    /// <summary>
    /// Updates the text on the queued jobs info panel, with the given data. In addition,
    /// it opens the queued jobs info panel.
    /// </summary>
    /// <param name="jobId">The job ID of the job.</param>
    /// <param name="name">The project name of the job.</param>
    /// <param name="score">The score of the job.</param>
    /// <param name="queue">The queue that the job is in.</param>
    /// <param name="walltime">The assigned walltime of the job.</param>
    /// <param name="queuedTime">The current time the job has been in the queue.</param>
    /// <param name="nodes">The number of nodes the job needs.</param>
    /// <param name="mode">The mode of the job.</param>
    public void UpdateQueuedJobPanel(string jobId, string name, string score, string queue, string walltime, string queuedTime, string nodes, string mode)
    {
        OpenQueuedJobsInfoPanel();

        // Tells the CooleyManager that the queued job info panel is open, so that it updates it every time new data is fetched
        cooleyManager.CurrentInfoPanel["type"] = "queued";
        cooleyManager.CurrentInfoPanel["id"] = jobId;

        // Updates all the text fields in the info panel
        queuedPanelTitleText.text = jobId + "\n" + name;
        queuedPanelScoreText.text = score;
        queuedPanelQueueText.text = queue;
        queuedPanelWalltimeText.text = walltime;
        queuedPanelQueuedTimeText.text = queuedTime;
        queuedPanelNodesText.text = nodes;
        queuedPanelModeText.text = mode;
    }


    /// <summary>
    /// Updates the text on the reservsed jobs info panel, with the given data. In addition,
    /// it opens the reserved jobs info panel.
    /// </summary>
    /// <param name="name">The project name of the job.</param>
    /// <param name="queue">The queue that the job is in.</param>
    /// <param name="partitions">The node partitions of the job.</param>
    /// <param name="formatedStartTime">The start time of the job in HH:mm:ss format.</param>
    /// <param name="formatedDuration">The duration of the job in HH:mm:ss format.</param>
    /// <param name="tMinus">The t-minus time of the job in HH:mm:ss format.</param>
    public void UpdateReservedJobPanel(string name, string queue, string partitions, string formatedStartTime, string formatedDuration, string tMinus)
    {
        OpenReservedJobsInfoPanel();

        // Tells the CooleyManager that the reserved job info panel is open, so that it updates it every time new data is fetched
        cooleyManager.CurrentInfoPanel["type"] = "reserved";
        cooleyManager.CurrentInfoPanel["id"] = name;
        
        // Updates all the text fields in the info panel
        reservedPanelTitleText.text = name;
        reservedPanelQueueText.text = queue;
        reservedPanelPartitionsText.text = partitions;
        reservedPanelStartTimeText.text = formatedStartTime;
        reservedPanelDurationText.text = formatedDuration;
        reservedPanelTMinusText.text = tMinus;
    }
}