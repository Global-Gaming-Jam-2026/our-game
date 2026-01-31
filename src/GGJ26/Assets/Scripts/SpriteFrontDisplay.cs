using UnityEngine;

public class SpriteFrontDisplay : MonoBehaviour
{
    SpriteRenderer _renderer;
    ParticleSystem[] _eyeFireSystems;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _eyeFireSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void Update()
    {
        float dot = Vector3.Dot(transform.forward, Camera.main.transform.forward);
        Color spriteColor = _renderer.color;
        spriteColor.a = (dot < 0f) ? 1 : 0;
        _renderer.color = spriteColor;

        if (dot < 0)
        {
            for (int i = 0; i < _eyeFireSystems.Length; i++)
            {
                _eyeFireSystems[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < _eyeFireSystems.Length; i++)
            {
                _eyeFireSystems[i].Stop();
            }
        }
    }
}
