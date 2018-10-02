using UnityEngine;

public class PerlinNoise : MonoBehaviour {

    public int width = 100;
    public int height = 100;

    public float scale = 20;

    private void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor (int x, int y)
    {
        float xCoords = (float)x / width * scale;
        float yCoords = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoords, yCoords);
        return new Color(sample, sample, sample);
    }
}
