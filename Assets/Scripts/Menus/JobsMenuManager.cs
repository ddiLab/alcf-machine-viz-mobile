using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobsMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGO;

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


    // Start is called before the first frame update
    void Start()
    {

        viewRunningJobsButtonGO.GetComponent<Button>().interactable = true;
        viewQueuedJobsButtonGO.GetComponent<Button>().interactable = true;
        viewReservedJobsButtonGO.GetComponent<Button>().interactable = true;

        runningJobsScrollViewGO.SetActive(false);
        queuedJobsScrollViewGO.SetActive(false);
        reservedJobsScrollViewGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenJobsMenu()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        panelGO.SetActive(true);
        openButtonGO.SetActive(false);
    }

    public void CloseJobsMenu()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        panelGO.SetActive(false);
        openButtonGO.SetActive(true);
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
}
