using System.Collections;
using UnityEngine;

public class ScreenshotAndMove : MonoBehaviour
{
    public Transform targetObject;
    public int numberOfScreenshots = 100;
    public string screenshotFolderPath = "Screenshots";

    private int screenshotCount = 0;

    private void Start()
    {
        // Create the folder if it doesn't exist
        System.IO.Directory.CreateDirectory("Assets/" + screenshotFolderPath);
        StartCoroutine(SsAndMove());
    }

    IEnumerator SsAndMove()
    {
        while(screenshotCount < numberOfScreenshots)
        {
            yield return new WaitForSeconds(.3f);
            TakeScreenshot();
            yield return new WaitForSeconds(.3f);
            MoveCameraRandomly();
            screenshotCount++;
        }

    }

    private void TakeScreenshot()
    {
        string screenshotFilename = $"Assets/{screenshotFolderPath}/screenshot_{screenshotCount}.png";
        ScreenCapture.CaptureScreenshot(screenshotFilename);
        Debug.Log($"Screenshot {screenshotCount} taken and saved as {screenshotFilename}");
    }

    private void MoveCameraRandomly()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(5f, 20f), Random.Range(-10f, 10f));
        transform.position = randomPosition;

        if (targetObject != null)
        {
            transform.LookAt(targetObject);
        }
    }
}
