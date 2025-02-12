using UnityEngine;
using System.Collections;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.75f;

    public SpriteRenderer _spriteRenderer;
    private Material _material;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");


    private void Start()
    {
        _material = _spriteRenderer.material;

        StartCoroutine(Vanish());
    }



    private IEnumerator Vanish()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1f, (elapsedTime / _dissolveTime));

            _material.SetFloat(_dissolveAmount, lerpedDissolve);

            yield return null;
        }
    }
}
