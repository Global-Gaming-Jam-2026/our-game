using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerDeathState : StateBase
{
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] string _gameOverSceneName = "GameOver"; 

    float _spriteAlpha;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        Controller.Body.bodyType = RigidbodyType2D.Kinematic;
        Controller.SFXPlayer.PlaySFX(_deathSFX);

        Sequence deathSequence = DOTween.Sequence();

        deathSequence.Append(DOTween.To(() => _spriteAlpha, x => _spriteAlpha = x, 1, 1.5f)
            .OnUpdate(() =>
            {
                _playerSprite.material.SetFloat("_DissolveAmount", _spriteAlpha);
            }));

        deathSequence.AppendInterval(0.5f);

        deathSequence.OnComplete(() =>
        {
            Controller.gameObject.SetActive(false);
            SceneManager.LoadScene(_gameOverSceneName);
        });
    }

    public override void OnStateFixedUpdate()
    {
        Controller.Body.linearVelocity = Vector2.Lerp(Controller.Body.linearVelocity, Vector2.zero, 10 * Time.fixedDeltaTime);
    }
}