using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanelGO;
    public GameObject arCameraGO;
    public GameObject arSessionOriginGO;
    public GameObject openSettingsButtonGO;
    public GameObject arPlanePrefab;

    private AROcclusionManager arOcclusionManager;
    private ARPlaneManager arPlaneManager;
    private bool arPlanesVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        arOcclusionManager = arCameraGO.GetComponent<AROcclusionManager>();
        arPlaneManager = arSessionOriginGO.GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenSettings()
    {
        settingsPanelGO.SetActive(true);
        openSettingsButtonGO.SetActive(false);
    }


    public void CloseSettings()
    {
        settingsPanelGO.SetActive(false);
        openSettingsButtonGO.SetActive(true);
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
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            arPlanesVisible = false;
        }
        else
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }
            arPlanesVisible = true;
        }  
    }
}
