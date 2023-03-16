using UnityEngine;

public class FruitPulsing : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float frequency;
    public float lowestValue;
    public float highestValue;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float value = lowestValue + (Mathf.Sin(frequency * Time.time)/2 + 0.5f) * (highestValue-lowestValue);
        spriteRenderer.color = new Color(value,value,value,1f);
    }
}
