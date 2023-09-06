using UnityEngine;
using System.IO;
using System.Collections;

public class TextureGenerator : MonoBehaviour
{
    int width = 300;
    int height = 300;
    public Cubemap previewTexture;
    int modifier = 1;
    private int currentImage = 0;
    private int dataSetSize = 10;
    private void Start()
    {
        StartCoroutine(ChangeTextAndSS());
    }


    IEnumerator ChangeTextAndSS()
    {
        yield return new WaitForSeconds(.3f);
        while (currentImage < dataSetSize)
        {
            string baseFileName = "temporary_texture";
            string fileName = baseFileName;

            // Check if a file with the same name already exists, and add a modifier if needed
            while (File.Exists($"Assets/HDRIS/{fileName}.png"))
            {
                Debug.Log("File exist");
                fileName = $"{Path.GetFileNameWithoutExtension(baseFileName)}_{modifier}";
                ++modifier;
            }

            // Create a new texture with the specified dimensions
            Texture2D texture = new Texture2D(width, height);

            // Generate a black and white pattern
            Color[] pixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    /*
                    if (random.Next(100) < 10)
                    {
                        grayscaleValue = random.Next(256) / 256f;
                        // Create a grayscale color using the random value
                        Color pixelColor = new Color(grayscaleValue, grayscaleValue, grayscaleValue);
                        last_color = pixelColor;
                        pixels[y * width + x] = pixelColor;
                    }
                    else
                    {
                        Color pixelColor;
                        pixelColor = last_color;
                        pixels[y * width + x] = pixelColor;
                    }*/
                    pixels[y * width + x] = Color.black;
                }
            }
            for (int i = 0; i < UnityEngine.Random.Range(1, 20); i++)
            {
                if (UnityEngine.Random.Range(0, 11) < 5)
                {
                    GenerateRandomGradientOnColor(pixels);
                }
                else
                {
                    GenerateRandomSquareGradients(pixels);
                }
            }

            previewTexture.SetPixels(pixels, CubemapFace.PositiveX);
            previewTexture.Apply();
            texture.SetPixels(pixels);
            texture.Apply();
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes($"Assets/PreviewTexture.png", pngData);
            File.WriteAllBytes($"Assets/HDRIS/{fileName}.png", pngData);
            UnityEditor.AssetDatabase.Refresh();
            yield return new WaitForSeconds(.3f);
            TakeScreenshot();
            yield return new WaitForSeconds(.3f);
            currentImage++;
        }
       
    }

    //0/1 y 2/3 x
    private Color[,] GenerateSquareGradient(int width, int height, int dir)
    {
        Color[,] gradient = new Color[width, height];
        for (int w = 0; w < width-1; w++)
        {
            for(int y = 0; y < height-1; y++)
            {
                float grad = 0f;
                switch (dir)
                {
                    case 0: {
                            grad = (float)y / (float)height;
                            break;
                        }
                    case 1:
                        {
                            grad = -((float)y / (float)height)+1.0f;
                            break;
                        }
                    case 2:
                        {
                            grad = (float)w / (float)width;
                            break;
                        }
                    case 3:
                        {
                            grad = -((float)w / (float)width) + 1.0f;
                            break;
                        }
                        default:
                        {
                            grad = (float)y / (float)height;
                            break;
                        }
                }
                gradient[w, y ] = new Color(grad, grad, grad); 
            }
        }
        return gradient;
    }

    private void GenerateRandomSquareGradients(Color[] pixels)
    {
        Color[,] gradient = GenerateRoundGradient(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300));
        int offsetX = UnityEngine.Random.Range(-100, 200);
        int offsetY = UnityEngine.Random.Range(-100, 200);
        Color empty = new Color();
        for (int y = 0; y < gradient.GetLength(1); y++)
        {
            for (int x = 0; x < gradient.GetLength(0); x++)
            {
                if (gradient[x, y] != null && gradient[x, y] != empty)
                {
                    if ((y + offsetY) < height && (y + offsetY) > 0 && (x + offsetX) < width &&  (x + offsetX) > 0)
                    {
                        pixels[(y + offsetY) * width + x + offsetX] = gradient[x, y];
                    }
                }
            }
        }
    }

    
    private Color[,] GenerateRoundGradient(int width, int height)
    {
        Color[,] gradient = new Color[width, height];
        for (int x = 0; x < width-1; x++)
        {
            for (int y = 0; y < height-1; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width/2, height/2));
                float grayscaleValue = Mathf.Clamp01(1 - (distance / width));

                Color pixelColor = new Color(grayscaleValue, grayscaleValue, grayscaleValue, grayscaleValue);

                gradient[x, y ] = pixelColor;
            }
        }

        return gradient;
    }

    private void GenerateRandomGradientOnColor(Color[] pixels)
    {
        Color[,] gradient = GenerateSquareGradient(UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 300), UnityEngine.Random.Range(0, 6));
        int offsetX = UnityEngine.Random.Range(-100, 200);
        int offsetY = UnityEngine.Random.Range(-100, 200);
        Color empty = new Color();
        for (int y = 0; y < gradient.GetLength(1); y++)
        {
            for (int x = 0; x < gradient.GetLength(0); x++)
            {
                if (gradient[x, y] != null && gradient[x, y] != empty)
                {
                    if ((y + offsetY) < height && (y + offsetY) > 0 && (x + offsetX) < width && (x + offsetX) > 0)
                    {
                        pixels[(y + offsetY) * width + x + offsetX] = gradient[x, y];
                    }
                }
            }
        }
    }

    private void TakeScreenshot()
    {
        string screenshotFilename = $"Assets/Screenshots/screenshot_{modifier-1}.png";
        ScreenCapture.CaptureScreenshot(screenshotFilename);
        //Debug.Log($"Screenshot {modifier-1} taken and saved as {screenshotFilename}");
    }


}
