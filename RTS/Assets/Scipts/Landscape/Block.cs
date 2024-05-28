using UnityEngine;
using UnityEngine.Rendering;

public enum RadiationLevels
{
    Low,
    Medium,
    High
}

[RequireComponent(typeof(Renderer))]
public class Block : MonoBehaviour
{
    public double infectScore;
    public RadiationLevels RadiationLevel;

    private Renderer blockRenderer;
    private Color[] colors;
    private bool altKeyPressed;

    void Awake()
    {
        infectScore = RadiationLevel switch
        {
            RadiationLevels.Low => 0,
            RadiationLevels.Medium => 21,
            _ => 41
        };
    }

    void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        colors = new Color[3]
        {
            new Color(1.0f, 0f, 0f, 0.1f),
            new Color(1.0f, 0.8f, 0f, 0.1f),
            new Color(0f, 1.0f, 0f, 0.1f)
        };
    }

    void LateUpdate()
    {
        var altPressed = Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);
        if (altPressed == altKeyPressed) return;
        altKeyPressed = altPressed;
        UpdateBlockColor();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Block>(out var block) && 
            (block.RadiationLevel == RadiationLevels.Medium || block.RadiationLevel == RadiationLevels.High))
        {
            var delta = block.RadiationLevel == RadiationLevels.High ? 0.1 : 0.05;
            ChangeRadiationScore(delta);
            block.ChangeRadiationScore(-delta + 0.000001);
        }
    }

    public void ChangeRadiationScore(double delta)
    {
        infectScore += delta;
        RadiationLevel = infectScore switch
        {
            < 20 => RadiationLevels.Low,
            > 40 => RadiationLevels.High,
            _ => RadiationLevels.Medium
        };

        if (altKeyPressed)
        {
            UpdateBlockColor();
        }
    }

    private void UpdateBlockColor()
    {
        blockRenderer.material.color = altKeyPressed
            ? RadiationLevel switch
            {
                RadiationLevels.Low => colors[2],
                RadiationLevels.High => colors[0],
                _ => colors[1]
            }
            : Color.white;
    }
}
