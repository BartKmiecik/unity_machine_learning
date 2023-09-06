
using System.Collections;
using UnityEngine;

public class ScreenshotAndMove : MonoBehaviour
{
    public Transform targetObject;
    public int numberOfScreenshots = 100;
    public string screenshotFolderPath = "Screenshots";
    public Material carPaint;
    private float minFov = 20;
    private float maxFov = 90;

    private int screenshotCount = 0;
    private Camera cam;

    private void Start()
    {
        // Create the folder if it doesn't exist
        System.IO.Directory.CreateDirectory("Assets/" + screenshotFolderPath);
        cam = Camera.main;
        StartCoroutine(SsAndMove());
    }

    IEnumerator SsAndMove()
    {
        while(screenshotCount < numberOfScreenshots)
        {
            yield return new WaitForSeconds(.3f);
            TakeScreenshot();
            yield return new WaitForSeconds(.3f);
            ChangeMaterialColor();
            //ChangeFov();
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
        float fov = ChangeFov();
        transform.position = randomPosition;

        if (targetObject != null)
        {
            transform.LookAt(targetObject);
        }

        if(fov > 50)
        {
            float test = fov * Vector3.Distance(cam.transform.position, targetObject.transform.position);
            transform.Translate(Vector3.forward * (test / 100));
        }
    }

    private float ChangeFov()
    {
        float fov = Random.Range(minFov, maxFov);
        cam.fieldOfView = fov;
        return fov;
    }

    private void ChangeMaterialColor()
    {
        Vector3 randColor = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        carPaint.color = new Color(randColor.x, randColor.y, randColor.z);
    }
}
