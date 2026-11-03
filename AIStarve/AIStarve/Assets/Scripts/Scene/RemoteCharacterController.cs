using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class RemoteCharacterController : MonoBehaviour
{
    #region Constants
    private static readonly int ANIM_WALKING = Animator.StringToHash("Walk");
    private static readonly int ANIM_DIRECTION = Animator.StringToHash("Direction");
    #endregion

    #region Fields
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Vector2 _targetPosition;
    private SceneManagement.Direction _moveDirection;
    private float _moveSpeed = 3f;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _targetPosition = transform.position;
    }

    public void SetMoveParameters(Vector2 position, SceneManagement.Direction direction)
    {
        _targetPosition = position;
        _moveDirection = direction;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, _targetPosition) > 0.1f)
        {
            _rigidbody.MovePosition(Vector2.MoveTowards(
                _rigidbody.position, 
                _targetPosition, 
                _moveSpeed * Time.fixedDeltaTime));

            UpdateAnimation((int)_moveDirection);
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetBool(ANIM_WALKING, false);
        }
    }

    private void UpdateAnimation(int direction)
    {
        int? animDirection = null;
        animDirection = direction;

        _animator.SetBool(ANIM_WALKING, animDirection.HasValue);
        if (animDirection.HasValue)
            _animator.SetInteger(ANIM_DIRECTION, animDirection.Value);
    }
}
