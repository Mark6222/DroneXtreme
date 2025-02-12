using UnityEngine;
using System.IO;
public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer textureRenderer;
    [SerializeField] private string folderName = "SavedTextures";
    [SerializeField] private string fileName = "saved_texture.png";
    public Texture2D texture2D;

    public void DrawTexture(Texture2D texture)
    {
        texture2D = texture;
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        SaveTextureToFile(texture, fileName);
    }

    private void SaveTextureToFile(Texture2D texture, string fileName)
    {
        string folderPath = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        byte[] textureBytes = texture.EncodeToPNG();
        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllBytes(filePath, textureBytes);
        Debug.Log($"Texture saved to: {filePath}");
    }
}
