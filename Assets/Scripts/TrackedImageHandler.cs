using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class TrackedImageHandler : MonoBehaviour
{

    public ARTrackedImageManager m_TrackedImageManager;
    public GameObject imageFoundPrefab;

    private GameObject cooleyImageFoundGO;
    private TextMeshPro cooleyImageFoundText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event

            if (newImage.referenceImage.name.Equals("Cooley"))
            {
                cooleyImageFoundGO = Instantiate(imageFoundPrefab);
                cooleyImageFoundText = cooleyImageFoundGO.transform.Find("Text (TMP)").GetComponent<TextMeshPro>();

                cooleyImageFoundGO.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                cooleyImageFoundText.text = "Cooley Viz!";
            }
            
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event

            if (updatedImage.referenceImage.name.Equals("Cooley"))
            {
                cooleyImageFoundGO.transform.position = updatedImage.transform.localPosition;
                cooleyImageFoundGO.transform.localEulerAngles = updatedImage.transform.localEulerAngles;

                cooleyImageFoundText.text = "Cooley Viz!";
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }
}
