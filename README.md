# Interactive AR Visualization of Cooley from ALCF on Mobile

## About the App
This application uses Unity's AR Foundation to create an interactive Augmented Reality visualization of the Cooley supercomputer from Argonne's Leadership Computing Facility (ALCF).

This app is able to detect an image that represents Cooley (`Assets/Images/Cooley.jpg`), and then create a visualization of the nodes and racks of the machine. Each node is colored based on the job that is being ran on that node, white if it is not being used, and black and grey if the node is down.

The user is also able to interact with the visualization by clicking on the `Jobs Menu` button and filtering the jobs that are being displayed on the visualization while also getting more information about the job in the pop-up info panel. Furthermore, they can view more jobs that are queued or reserved, and more information about these in their own info panels.

Lastly, the app has a `Settings` button that lets the user control whether the planes that are detected by the camera are visualized, if Occlusion should be enabled or not, and if the visualized racks of the Cooley machine should be visible.

## Project Setup
In order to work on this project, you should be able to download/clone this repo locally, and open it using Unity Hub. This should automatically have everything setup for development. However if there are any difficulties I recommend watching the following [video tutorial](https://www.youtube.com/watch?v=FWyTf3USDCQ). 

## App Deployment
In order to deploy the app on a mobile device I recommend watching the following videos:
- [iOS tutorial](https://www.youtube.com/watch?v=-Hr4-XNCf8Y)
- [Android tutorial](https://www.youtube.com/watch?v=NAv7HuXtupE)

Lastly, one thing to note is that newer Android phones and iOS devices that are equipped with a LiDAR scanner can provide better Occlusion results. Older devices might not support this future if enabled **(Not tested)**.

## More Information
View Cooley's data [here](https://status.alcf.anl.gov/cooley/activity.json). \
Read more about Cooley [here](https://www.alcf.anl.gov/alcf-resources/cooley). \
Read more about AR Foundation [here](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/manual/index.html). \
Read more about Occlusion [here](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/manual/features/occlusion.html).