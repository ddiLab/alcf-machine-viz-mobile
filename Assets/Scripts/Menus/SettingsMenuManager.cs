using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject panelGO;
    public GameObject arCameraGO;
    public GameObject arSessionOriginGO;
    public GameObject openButtonGO;

    /// <summary>
    /// Holds the button that let's a user toggle the virtual representation of Cooley's racks.
    /// </summary>
    public GameObject toggleRacksButtonGO;
    
    public GameObject arPlanePrefab;

    

    public CooleyManager cooleyManager;
    public JobsMenuManager jobsMenuManager;

    private AROcclusionManager arOcclusionManager;
    private ARPlaneManager arPlaneManager;
    private bool arPlanesVisible = true;

    private Vector3 initialOpenButtonGO;

    // Start is called before the first frame update
    void Start()
    {
        arOcclusionManager = arCameraGO.GetComponent<AROcclusionManager>();
        arPlaneManager = arSessionOriginGO.GetComponent<ARPlaneManager>();

        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenSettingsMenu()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        panelGO.SetActive(true);
        openButtonGO.SetActive(false);

        jobsMenuManager.SetActiveJobsMenuButton(false);
    }


    public void CloseSettingsMenu()
    {
        //Screen.orientation = ScreenOrientation.AutoRotation;
        panelGO.SetActive(false);
        openButtonGO.SetActive(true);

        if (GameObject.Find("Cooley") != null && cooleyManager.IsMachineRunning())
        {
            jobsMenuManager.SetActiveJobsMenuButton(true);
        }
    }


    public void SetToggleRacksButtonInteractable(bool newStatus)
    {
        toggleRacksButtonGO.GetComponent<Toggle>().interactable = newStatus;
    }


    public void ToggleOcclusion()
    {
        if (arOcclusionManager.requestedEnvironmentDepthMode.ToString().Equals("Disabled"))
        {
            arOcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Fastest;
        }
        else
        {
            arOcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Disabled;
        }
    }

    public void TogglePlanePrefab()
    {
        if (arPlanesVisible)
        {
            arPlaneManager.planePrefab = null;
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            arPlanesVisible = false;
        }
        else
        {
            arPlaneManager.planePrefab = arPlanePrefab;
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }
            arPlanesVisible = true;
        }  
    }


    public void ToggleCooleyRacks()
    {
        if (cooleyManager.IsMachineRunning() && GameObject.Find("Cooley") != null)
        {
            cooleyManager.ToggleRacks();
        }
    }
}
