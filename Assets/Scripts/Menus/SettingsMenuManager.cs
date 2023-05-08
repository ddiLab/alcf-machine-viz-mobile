using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;


/// <summary>
/// Manages the Settings menu visibility, interactivity, and functionality. Providing 
/// functions that perform the requests that the users give using the menu's buttons.
/// </summary>
public class SettingsMenuManager : MonoBehaviour
{
    /// <summary>
    /// Holds the script tht manages the occlusion in the AR environment.
    /// </summary>
    private AROcclusionManager arOcclusionManager;

    /// <summary>
    /// Holds the script that manages the planes created in the AR envrionment.
    /// </summary>
    private ARPlaneManager arPlaneManager;

    /// <summary>
    /// Holds whether to display the visualization of the planes in the AR environment.
    /// </summary>
    private bool arPlanesVisible;


    /// <summary>
    /// Holds the script that manages the Cooley visualization.
    /// </summary>
    [SerializeField]
    private CooleyManager cooleyManager;

    /// <summary>
    /// Holds the script that manages the Jobs Menu.
    /// </summary>
    [SerializeField]
    private JobsMenuManager jobsMenuManager;

    /// <summary>
    /// Holds the AR camera GameObject in order to have access to the occlusion manager.
    /// </summary>
    [SerializeField]
    private GameObject arCameraGO;

    /// <summary>
    /// Holds the AR session GameObject in order to have access to the plane manager.
    /// </summary>
    [SerializeField]
    private GameObject arSessionOriginGO;

    /// <summary>
    /// Holds the panel GameObjects of the Settings menu.
    /// </summary>
    [SerializeField]
    private GameObject panelGO;

    /// <summary>
    /// Holds the button GameObject that opens the Settings menu.
    /// </summary>
    [SerializeField]
    private GameObject openButtonGO;

    /// <summary>
    /// Holds the button that let's a user toggle the virtual representation of Cooley's racks.
    /// </summary>
    [SerializeField]
    private GameObject toggleRacksButtonGO;

    /// <summary>
    /// Holds the prefab that visualizes the planes in the AR environment.
    /// </summary>
    [SerializeField]
    private GameObject arPlanePrefab;


    /// <summary>
    /// Start is called before the first frame update. Initializes the private variables, as well as
    /// ensures that the phone stays in portrait mode.
    /// </summary>
    void Start()
    {
        arOcclusionManager = arCameraGO.GetComponent<AROcclusionManager>();
        arPlaneManager = arSessionOriginGO.GetComponent<ARPlaneManager>();
        arPlanesVisible = true;

        // Ensures the phone is in portait mode
        Screen.orientation = ScreenOrientation.Portrait;
    }


    /// <summary>
    /// Opens the Settings menu panel.
    /// </summary>
    public void OpenSettingsMenu()
    {
        panelGO.SetActive(true);
        openButtonGO.SetActive(false);

        // Makes the Jobs Menu button invisible
        jobsMenuManager.SetActiveJobsMenuButton(false);
    }


    /// <summary>
    /// Closes the Settings menu panel.
    /// </summary>
    public void CloseSettingsMenu()
    {
        panelGO.SetActive(false);
        openButtonGO.SetActive(true);

        // Checks if Cooley has been detected and running
        if (GameObject.Find("Cooley") != null && cooleyManager.IsMachineRunning())
        {
            // Shows the Jobs Menu button since Cooley is running
            jobsMenuManager.SetActiveJobsMenuButton(true);
        }
    }


    /// <summary>
    /// Updates the interactivity of the Toggle Racks button.
    /// </summary>
    /// <param name="newStatus">The new interactivity status of the button.</param>
    public void SetToggleRacksButtonInteractable(bool newStatus)
    {
        toggleRacksButtonGO.GetComponent<Toggle>().interactable = newStatus;
    }


    /// <summary>
    /// Toggles the occlusion script in the AR environment.
    /// </summary>
    public void ToggleOcclusion()
    {
        // Checks the current status of the occlusion script
        if (arOcclusionManager.requestedEnvironmentDepthMode.ToString().Equals("Disabled"))
        {
            // Occlusion is currently disabled, therefore it is toggled on
            arOcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Fastest;
        }
        else
        {
            // Occlusion is currently enabled, therefore it is toggled off
            arOcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Disabled;
        }
    }


    /// <summary>
    /// Toggles the plane visualization in the AR environment.
    /// </summary>
    public void TogglePlanePrefab()
    {
        // Checks if the planes are currently visible
        if (arPlanesVisible)
        {
            // They are visible

            // Removes the prefab from the manager script
            arPlaneManager.planePrefab = null;

            // Loops through all the visualized planes and makes them invisible
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }

            // Updates the variable for future function calls
            arPlanesVisible = false;
        }
        else
        {
            // They are invisible

            // Provides the prefab to the manager script
            arPlaneManager.planePrefab = arPlanePrefab;

            // Loops through all the previously visualized planes and makes them visible
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }

            // Updates the variable for future function calls
            arPlanesVisible = true;
        }  
    }


    /// <summary>
    /// Toggles the Cooley racks from the machine visualization.
    /// </summary>
    public void ToggleCooleyRacks()
    {
        // Checks if the machine is running and if it has been detected
        if (cooleyManager.IsMachineRunning() && GameObject.Find("Cooley") != null)
        {
            // Calls the function from the Cooley Manager to handle it
            cooleyManager.ToggleRacks();
        }
    }
}