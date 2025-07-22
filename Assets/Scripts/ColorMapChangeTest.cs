using UnityEngine;
using UnityEngine.InputSystem;

public class ColorMapChangeTest : MonoBehaviour
{
    public Material material;

    public Texture2D texture1;
    public Texture2D texture2;

    private bool textureOne = true;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (textureOne)
                material.SetTexture("_BaseMap", texture2);
            else material.SetTexture("_BaseMap", texture1);

            textureOne = !textureOne;
        }
    }
}
