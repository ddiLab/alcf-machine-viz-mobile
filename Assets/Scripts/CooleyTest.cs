using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This simulates what happens in the TrackedImageHandler.cs script, to be able to test the
/// app without having to deploy it on a mobile device, and by simulating an "image" that represents
/// Cooley. It is used to test withing Unity, and save time with deployement. 
/// </summary>
/// <remark>
/// To get accurate tests for the app you must deploy it on a mobile device to ensure everything works
/// as intented.
/// </remark>
public class CooleyTest : MonoBehaviour
{
    /// <summary>
    /// Holds the script that manages the Cooley machine visualization
    /// </summary>
    [SerializeField]
    private CooleyManager cooleyManager;


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        GameObject testingImageGO; //< Holds the GameObject that simulates the image

        // Grabs the initial data for the machine
        cooleyManager.GetData();

        // Checks if the machine is running
        if (cooleyManager.IsMachineRunning())
        {
            // Creates the GameObject that will simulate as the detected image
            testingImageGO = new GameObject("TestImageGO");
            testingImageGO.transform.SetPositionAndRotation(new Vector3(1f, 1f, 1f), Quaternion.identity);

            // Does the initial setup of the Cooley visualization
            SetupCooleyViz(true);

            // Does the first update of the Cooley visualization while providing the position of the simulated image
            SetupCooleyViz(false, testingImageGO.transform);
        }
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Sets up the visualization of Cooley depending if this a setup or update call, by using the
    /// Cooley Manager script.
    /// </summary>
    /// <param name="initalizeViz">Specifies if the visualization needs to be initialized or updated.</param>
    /// <param name="imageTransform">Holds the new position of the visualization. Only used if the visualization is being updated.</param>
    private void SetupCooleyViz(bool initializeViz, Transform imageTransform = null)
    {
        if (initializeViz)
        {
            // Does the initialization of the visualization
            cooleyManager.CreateGameObjects();
        }
        else
        {
            // Updates the position of the visualization
            cooleyManager.UpdateGameObjects(imageTransform);
        }
    }
}