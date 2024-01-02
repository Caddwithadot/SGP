using UnityEngine;
using UnityEngine.Rendering;

public class TextureSheetTest : MonoBehaviour
{
    public Texture2D spriteSheet;
    private Sprite[] animationSprites;

    void Start()
    {
        ExtractSprites();

        // Get the Particle System component
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        // Access the Texture Sheet Animation module
        var textureSheetModule = particleSystem.textureSheetAnimation;

        // Set the Sprite mode to Sprites
        textureSheetModule.mode = ParticleSystemAnimationMode.Sprites;

        //set the time mode to fps
        textureSheetModule.timeMode = ParticleSystemAnimationTimeMode.FPS;

        // Add each sprite to the Texture Sheet Animation module
        foreach (var sprite in animationSprites)
        {
            textureSheetModule.AddSprite(sprite);
        }

        //set the fps accordingly to play 1 cycle
        textureSheetModule.fps = textureSheetModule.spriteCount;
    }

    void ExtractSprites()
    {
        //gets the path where the spritesheet is located
        string path = $"Emotes/{spriteSheet.name}";

        // Get all sprites from the texture
        Sprite[] sprites = Resources.LoadAll<Sprite>(path); // Assuming the texture is in the "Resources" folder

        // Assign the array of sprites to animationSprites
        animationSprites = sprites;
    }
}