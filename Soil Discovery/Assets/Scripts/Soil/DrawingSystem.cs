using UnityEngine;

public class DrawingSystem : MonoBehaviour
{
    public RenderTexture canvasTexture;
    public Material brushMaterial; // A simple shader that draws a circle

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Map hit UV coordinates to Texture space
                Vector2 uv = hit.textureCoord;
                DrawAt(uv);
            }
        }
    }

    void DrawAt(Vector2 uv)
    {
        // Use Graphics.Blit to draw the brush onto the canvas
        RenderTexture temp = RenderTexture.GetTemporary(canvasTexture.width, canvasTexture.height);
        brushMaterial.SetVector("_BrushPos", new Vector4(uv.x, uv.y, 0, 0));
        Graphics.Blit(canvasTexture, temp, brushMaterial);
        Graphics.Blit(temp, canvasTexture);
        RenderTexture.ReleaseTemporary(temp);
    }
}