using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;

public class JobsMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGO;

    [SerializeField]
    private GameObject filteredRunningJobPanelGO;

    [SerializeField]
    private GameObject queuedJobPanelGO;

    [SerializeField]
    private GameObject reservedJobPanelGO;

    [SerializeField]
    private GameObject openButtonGO;

    [SerializeField]
    private GameObject showAllNodesButtonGO;

    [SerializeField]
    private GameObject viewRunningJobsButtonGO;

    [SerializeField]
    private GameObject viewQueuedJobsButtonGO;

    [SerializeField]
    private GameObject viewReservedJobsButtonGO;

    [SerializeField]
    private GameObject runningJobsScrollViewGO;

    [SerializeField]
    private GameObject queuedJobsScrollViewGO;

    [SerializeField]
    private GameObject reservedJobsScrollViewGO;

    [SerializeField]
    private GameObject runningJobsContentGO;

    [SerializeField]
    private GameObject queuedJobsContentGO;

    [SerializeField]
    private GameObject reservedJobsContentGO;

    [SerializeField]
    private GameObject noRunningJobsTextGO;

    [SerializeField]
    private GameObject noQueuedJobsTextGO;

    [SerializeField]
    private GameObject noReservedJobsTextGO;

    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelTitleText;

    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelRunTimeText_1;

    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelRunTimeText_2;

    [SerializeField]
    private TextMeshProUGUI filteredRunningPanelWalltimeText;

    [SerializeField]
    private Slider filteredRunningPanelMaxWalltimeProgresBar;

    [SerializeField]
    private Slider filteredRunningPanelJobWalltimeProgresBar;

    [SerializeField]
    private TextMeshProUGUI reservedPanelTitleText;

    [SerializeField]
    private TextMeshProUGUI reservedPanelQueueText;

    [SerializeField]
    private TextMeshProUGUI reservedPanelPartitionsText;

    [SerializeField]
    private TextMeshProUGUI reservedPanelStartTimeText;

    [SerializeField]
    private TextMeshProUGUI reservedPanelDurationText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelTitleText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelScoreText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelQueueText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelWalltimeText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelQueuedTimeText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelNodesText;

    [SerializeField]
    private TextMeshProUGUI queuedPanelModeText;

    [SerializeField]
    private GameObject runningJobButtonPrefab;

    [SerializeField]
    private GameObject queuedJobButtonPrefab;

    [SerializeField]
    private GameObject reservedJobButtonPrefab;


    [SerializeField]
    private CooleyManager cooleyManager;


    // Start is called before the first frame update
    void Start()
    {
        // Selects running jobs to be shown in the job menu by default
        ChangeJobTypeView("running");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenJobsMenu()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        panelGO.SetActive(true);
        openButtonGO.SetActive(false);
    }

    public void CloseJobsMenu()
    {
        //Screen.orientation = ScreenOrientation.AutoRotation;
        panelGO.SetActive(false);
        openButtonGO.SetActive(true);
    }
    
    public void OpenFilteredRunningJobsInfoPanel()
    {
        CloseJobsMenu();
        filteredRunningJobPanelGO.SetActive(true);
    }

    public void CloseFilteredRunningJobsInfoPanel()
    {
        OpenJobsMenu();
        filteredRunningJobPanelGO.SetActive(false);
    }

    public void OpenQueuedJobsInfoPanel()
    {
        queuedJobPanelGO.SetActive(true);
    }

    public void CloseQueuedJobsInfoPanel()
    {
        queuedJobPanelGO.SetActive(false);
    }

    public void OpenReservedJobsInfoPanel()
    {
        reservedJobPanelGO.SetActive(true);
    }

    public void CloseReservedJobsInfoPanel()
    {
        reservedJobPanelGO.SetActive(false);
    }

    public void SetActiveJobsMenuButton(bool newStatus)
    {
        openButtonGO.SetActive(newStatus);
    }

    public void SetActiveShowAllNodesButton(bool newStatus)
    {
        showAllNodesButtonGO.SetActive(newStatus);
    }

    public void ChangeJobTypeView(string jobType)
    {
        if (jobType.Equals("running"))
        {
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = false;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = true;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = true;

            runningJobsScrollViewGO.SetActive(true);
            queuedJobsScrollViewGO.SetActive(false);
            reservedJobsScrollViewGO.SetActive(false);
        }
        else if (jobType.Equals("queued"))
        {
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = true;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = false;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = true;

            runningJobsScrollViewGO.SetActive(false);
            queuedJobsScrollViewGO.SetActive(true);
            reservedJobsScrollViewGO.SetActive(false);
        }
        else if (jobType.Equals("reserved"))
        {
            viewRunningJobsButtonGO.GetComponent<Button>().interactable = true;
            viewQueuedJobsButtonGO.GetComponent<Button>().interactable = true;
            viewReservedJobsButtonGO.GetComponent<Button>().interactable = false;

            runningJobsScrollViewGO.SetActive(false);
            queuedJobsScrollViewGO.SetActive(false);
            reservedJobsScrollViewGO.SetActive(true);
        }
    }

    public void UpdateRunningJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        
        if (newJobsDict.Count == 0)
        {
            noRunningJobsTextGO.SetActive(true);
            return;
        }

        GameObject newRunningJobButtonGO;
        Color buttonColor;

        noRunningJobsTextGO.SetActive(false);

        foreach (var (currJobId, currJobInfo) in newJobsDict)
        {
            newRunningJobButtonGO = Instantiate(runningJobButtonPrefab, runningJobsContentGO.transform);
            newRunningJobButtonGO.transform.name = currJobId;

            ColorUtility.TryParseHtmlString(currJobInfo["hexColor"], out buttonColor);

            newRunningJobButtonGO.transform.GetComponent<Image>().color = buttonColor;

            newRunningJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobId + "\n" + currJobInfo["projectName"];
            
            newRunningJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.FilterNodeGameObjectsByJobId(currJobId));
        }
    }

    public void UpdateQueuedJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        if (newJobsDict.Count == 0)
        {
            noQueuedJobsTextGO.SetActive(true);
            return;
        }

        GameObject newQueuedJobButtonGO;

        noQueuedJobsTextGO.SetActive(false);

        foreach (var (currJobId, currJobInfo) in newJobsDict)
        {
            newQueuedJobButtonGO = Instantiate(queuedJobButtonPrefab, queuedJobsContentGO.transform);
            newQueuedJobButtonGO.transform.name = currJobId;

            newQueuedJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobId + "\n" + currJobInfo["projectName"] + "\n" + currJobInfo["score"];
        
            newQueuedJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.ShowQueuedJobInfo(currJobId));
        }
    }

    public void UpdateReservedJobsMenu(SortedDictionary<string, Dictionary<string, string>> newJobsDict)
    {
        if (newJobsDict.Count == 0)
        {
            noReservedJobsTextGO.SetActive(true);
            return;
        }

        GameObject newReservedJobButtonGO;

        noReservedJobsTextGO.SetActive(false);  

        foreach (var (currJobName, currJobInfo) in newJobsDict)
        {
            newReservedJobButtonGO = Instantiate(reservedJobButtonPrefab, reservedJobsContentGO.transform);
            newReservedJobButtonGO.transform.name = currJobName;


            newReservedJobButtonGO.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = currJobName + "\n" + currJobInfo["nodePartitions"];
            
            newReservedJobButtonGO.transform.GetComponent<Button>().onClick.AddListener(() => cooleyManager.ShowReservedJobInfo(currJobName));
        }
    }

    public void UpdateFilteredRunningJobPanel(string jobId, string projectName, string runTime, string walltime)
    {
        DateTime currentRunTime = DateTime.ParseExact(runTime, "HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime maxWalltime = DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime jobWalltime = DateTime.ParseExact(walltime, "HH:mm:ss", CultureInfo.InvariantCulture);

        filteredRunningPanelTitleText.text = jobId + "\n" + projectName;
        filteredRunningPanelRunTimeText_1.text = runTime;
        filteredRunningPanelRunTimeText_2.text = runTime;
        filteredRunningPanelWalltimeText.text = walltime;

        filteredRunningPanelMaxWalltimeProgresBar.value = (float) (currentRunTime.TimeOfDay.TotalMilliseconds / maxWalltime.TimeOfDay.TotalMilliseconds);
        
        filteredRunningPanelJobWalltimeProgresBar.value = (float) (currentRunTime.TimeOfDay.TotalMilliseconds / jobWalltime.TimeOfDay.TotalMilliseconds);
    }

    public void UpdateQueuedJobPanel(string jobId, string name, string score, string queue, string walltime, string queuedTime, string nodes, string mode)
    {
        OpenQueuedJobsInfoPanel();

        queuedPanelTitleText.text = jobId + "\n" + name;
        queuedPanelScoreText.text = score;
        queuedPanelQueueText.text = queue;
        queuedPanelWalltimeText.text = walltime;
        queuedPanelQueuedTimeText.text = queuedTime;
        queuedPanelNodesText.text = nodes;
        queuedPanelModeText.text = mode;
    }

    public void UpdateReservedJobPanel(string name, string queue, string partitions, string formatedStartTime, string formatedDuration)
    {
        OpenReservedJobsInfoPanel();
        
        reservedPanelTitleText.text = name;
        reservedPanelQueueText.text = queue;
        reservedPanelPartitionsText.text = partitions;
        reservedPanelStartTimeText.text = formatedStartTime;
        reservedPanelDurationText.text = formatedDuration;
    }
}
