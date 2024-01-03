using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private Image targetMaterial; // Reference to the material you want to change
    public Texture newTexture; // The texture you want to set

    void Start()
    {
        targetMaterial = GetComponent<Image>();

        // Replace the main texture of the material
        targetMaterial.GetComponent<Material>().mainTexture = newTexture;
    }
}