using UnityEngine;
using System.IO;
using System;
using Google.Protobuf.WellKnownTypes;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class TextureGenerator : MonoBehaviour
{
    int width = 300;
    int height = 300;
    private void Start()
    {
        
        string baseFileName = "temporary_texture";
        string fileName = baseFileName;
        int offsetX = 120;
        int offsetY = 120;

        // Check if a file with the same name already exists, and add a modifier if needed
        int modifier = 1;
        while (File.Exists($"Assets/HDRIS/{fileName}.png"))
        {
            Debug.Log("File exist");
            fileName = $"{Path.GetFileNameWithoutExtension(baseFileName)}_{modifier}";
            modifier++;
        }

        // Create a new texture with the specified dimensions
        Texture2D texture = new Texture2D(width, height);

        // Generate a black and white pattern
        Color[] pixels = new Color[width * height];
        System.Random random = new System.Random();
        float grayscaleValue = random.Next(256) / 256f;

        // Create a grayscale color using the random value
        Color last_color = new Color(grayscaleValue, grayscaleValue, grayscaleValue);
        Color[,] graident = GenerateSquareGradient(100, 100,2);
        Color[,] gradeint2 = GenerateSquareGradient(100, 100, 3);
        //Color[,] gradeint2 = GenerateRoundGradient(100, 100);
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
        for(int i = 0; i < UnityEngine.Random.Range(1, 20); i++)
        {
            if(UnityEngine.Random.Range(0,11) < 5)
            {
                GenerateRandomGradientOnColor(pixels);
            } else
            {
                GenerateRandomSquareGradients(pixels);
            }
        }

        /*
        Color empty = new Color();
        for (int y = 0; y < graident.GetLength(1); y++)
        {
            for (int x = 0; x < graident.GetLength(0); x++)
            {
                if (graident[x, y] != null && graident[x, y] != empty)
                {
                    if((y + offsetY) < height && (x + offsetX) < width)
                    {
                        pixels[(y + offsetY) * width + x + offsetX] = graident[x, y];
                    }
                }
            }
        }
        for (int y = 0; y < gradeint2.GetLength(1); y++)
        {
            for (int x = 0; x < gradeint2.GetLength(0); x++)
            {
                if (gradeint2[x, y] != null && gradeint2[x, y] != empty)
                {
                    if ((y + offsetY/4) < height && (x + offsetX/4) < width)
                    {
                        pixels[(y + offsetY/4) * width + x + offsetX/4] = gradeint2[x, y];
                    }
                }
            }
        }
        */

        // Set the pixel data for the texture
        texture.SetPixels(pixels);

        // Apply changes to the texture
        texture.Apply();

        // Encode the texture to a PNG file
        byte[] pngData = texture.EncodeToPNG();

        // Save the PNG data to the file with the modified name
        File.WriteAllBytes($"Assets/HDRIS/{fileName}.png", pngData);

        // Don't forget to refresh the Unity Asset Database to see the new texture in the editor
        UnityEditor.AssetDatabase.Refresh();

        // Optional: Assign the generated texture to a material or object
        // Renderer renderer = GetComponent<Renderer>();
        // renderer.material.mainTexture = texture;
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
                //Debug.Log($"Width: {w} and {w + startX}, height: {}")
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
                // Calculate the grayscale value based on the distance from the positionX and positionY
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width/2, height/2));
                float grayscaleValue = Mathf.Clamp01(1 - (distance / width));

                // Set the pixel color to grayscale
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
}
