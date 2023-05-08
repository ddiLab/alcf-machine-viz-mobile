using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;


/// <summary>
/// Handles what happens when an image that represents Cooley is detected. More specifically,
/// it starts the Cooley machine visualization, and handles the updating of its position in the
/// AR environment.
/// </summary>
public class TrackedImageHandler : MonoBehaviour
{
    /// <summary>
    /// Holds the GameObject created when Cooley is deteceted.
    /// </summary>
    private GameObject cooleyImageFoundGO;

    /// <summary>
    /// Holds the text component of the above GO.
    /// </summary>
    private TextMeshPro cooleyImageFoundText;


    /// <summary>
    /// Holds the script that manages the Cooley visualization.
    /// </summary>
    [SerializeField]
    private CooleyManager cooleyManager;

    /// <summary>
    /// Holds the script that does the image tracking.
    /// </summary>
    [SerializeField]
    private ARTrackedImageManager m_TrackedImageManager;

    /// <summary>
    /// Holds the prefab used to show that Cooley has been detected.
    /// </summary>
    [SerializeField]
    private GameObject imageFoundPrefab;

    
    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;


    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;


    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Loops through a list of images detected that the manager was told given
        foreach (var newImage in eventArgs.added)
        {
            // Handles added event


            if (newImage.referenceImage.name.Equals("Cooley"))
            {
                // The detected image is the one for Cooley

                // Creates the GameObject that lets the user know Cooley was detected
                cooleyImageFoundGO = Instantiate(imageFoundPrefab);
                cooleyImageFoundText = cooleyImageFoundGO.transform.Find("Text (TMP)").GetComponent<TextMeshPro>();

                cooleyImageFoundGO.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                
                // Initial data fetch
                cooleyManager.GetData();

                // Sets up initial visualization
                SetupCooleyViz(true);
            }
            
        }

        // Loops through the same images as above, only if this is the second or plus time detecting them
        foreach (var updatedImage in eventArgs.updated)
        {
            // Handles updated event


            if (updatedImage.referenceImage.name.Equals("Cooley"))
            {
                // The detected image is the one for Cooley

                // Updates the image found GameObjects position and angles
                cooleyImageFoundGO.transform.position = updatedImage.transform.localPosition;
                cooleyImageFoundGO.transform.localEulerAngles = updatedImage.transform.localEulerAngles;

                // Updates the visualizations position based on the images position
                SetupCooleyViz(false, updatedImage.transform);
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handles removed event
        }
    }


    /// <summary>
    /// Uses the CooleyManager script in order to setup the visualization of the Cooley
    /// super computer. If this is the first call, then the visualization is initialized,
    /// otherwise it updates the visualizations position.
    /// </summary>
    /// <param name="initializeViz">Specifies if the visualization needs to be initialized or updated.</param>
    /// <param name="imageTransform">Holds the new position of the visualization. Only used if the visualization is being updated.</param>
    private void SetupCooleyViz(bool initializeViz, Transform imageTransform = null)
    {
        Color textColor; //< Holds the color of the text that is displayed when the machine is detected

        
        if (cooleyManager.IsMachineRunning())
        {
            // The machine is online and running

            if (initializeViz)
            {
                // Initial call to the function, therefore we create the visualization
                cooleyManager.CreateGameObjects();
            }
            else
            {
                // This is an update call, therefore we update the position of the visualization
                cooleyManager.UpdateGameObjects(imageTransform);
            }

            
            // Displays the message when the machine is detected with the correct color
            cooleyImageFoundText.text = "Cooley Viz!";
            ColorUtility.TryParseHtmlString("#128F7C", out textColor);
            cooleyImageFoundText.color = textColor;
        }
        else
        {
            // The machine is offline or under maintenanace

            // Displays the maintenance message when the image of Cooley is detected
            cooleyImageFoundText.text = "Cooley is under\nmaintenance";
            ColorUtility.TryParseHtmlString("#FFCA00", out textColor);
            cooleyImageFoundText.color = textColor;
        }
    }
}