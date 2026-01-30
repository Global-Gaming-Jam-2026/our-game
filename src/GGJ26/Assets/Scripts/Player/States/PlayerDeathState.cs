using DG.Tweening;
using UnityEngine;

public class PlayerDeathState : StateBase
{
    [SerializeField] SpriteRenderer _playerSprite;

    float _spriteAlpha;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        DOTween.To(() => _spriteAlpha, x => _spriteAlpha = x, 1, 1.5f).OnUpdate(() =>
        {
            _playerSprite.material.SetFloat("_DissolveAmount", _spriteAlpha);
        }).OnComplete(() => Controller.gameObject.SetActive(false));

        Controller.Body.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnStateFixedUpdate()
    {
        Controller.Body.linearVelocity = Vector2.Lerp(Controller.Body.linearVelocity, Vector2.zero, 10 * Time.fixedDeltaTime);
    }
}
