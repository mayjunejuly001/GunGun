using UnityEngine;
using System.Collections;

public class ExplosiveEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float animationDuration = 0.5f;
    public float startAlpha = 1f; 
    private Vector3 initialScale;

    private void Awake()
    {
       
        initialScale = transform.localScale; 
    }

    private void Start()
    {
        StartCoroutine(AnimateEffect());
    }

    IEnumerator AnimateEffect()
    {
        float elapsedTime = 0f;
        Color spriteColor = spriteRenderer.color;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration; 

           
            spriteColor.a = Mathf.Lerp(startAlpha, 0f, t);
            spriteRenderer.color = spriteColor;

      
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

      
        spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0f);
        transform.localScale = Vector3.zero;

        Destroy(gameObject); 
    }

    public void setRendererScale(float scale)
    {
        spriteRenderer.transform.localScale = Vector3.one * scale;
    }
}
