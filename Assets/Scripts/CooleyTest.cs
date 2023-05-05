using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooleyTest : MonoBehaviour
{
    public CooleyManager cooleyManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject testingGO;

        cooleyManager.GetData();
        if (cooleyManager.IsMachineRunning())
        {
            testingGO = new GameObject("TestingGO");
            testingGO.transform.SetPositionAndRotation(new Vector3(1f, 1f, 1f), Quaternion.identity);

            SetupCooleyViz(true);

            SetupCooleyViz(false, testingGO.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupCooleyViz(bool initializeViz, Transform imageTransform = null)
    {
        if (initializeViz)
        {
            cooleyManager.CreateGameObjects();
        }
        else
        {
            cooleyManager.UpdateGameObjects(imageTransform);
        }
    }
}
