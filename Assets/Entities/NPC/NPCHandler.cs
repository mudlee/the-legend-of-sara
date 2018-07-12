using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCHandler : MonoBehaviour
{
    // CONSTANTS
    private const float SPEED = 0.4f;
    private const int AWARANESS_RADIUS = 5;

    private enum Direction { LEFT, RIGHT, TOP, BOTTOM, DONT_MOVE };

    [SerializeField] private NPCInfo _npcInfo;
    private AudioSource _audioSource;
    private Animator _animator;
    private AnimatorOverrideController _animatorOverrideController;
    private Rigidbody2D _rigidbody;
    private GameObject _player;

    // MOVEMENT
    private Direction _currentDirection;
    private int _remainingMovementTime = 0;
    private Vector2 _velocity = new Vector2();
    private Direction? _collidedDirection;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animatorOverrideController["WalkLeft"] = _npcInfo.walkLeft;
        _animatorOverrideController["WalkRight"] = _npcInfo.walkRight;
        _animatorOverrideController["WalkTop"] = _npcInfo.walkTop;
        _animatorOverrideController["WalkBottom"] = _npcInfo.walkBottom;
        _animator.runtimeAnimatorController = _animatorOverrideController;

        InvokeRepeating("Scream", Random.Range(2f, 10f), Random.Range(1f, 5f));
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if(distance<=AWARANESS_RADIUS)
        {
            print("player close");
        }
    }

    private void FixedUpdate()
    {
        if(_remainingMovementTime == 0)
        {
            UpdateDirection((Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length));
            UpdateAnimProps();
            UpdateVelocity();
        }
        else
        {
            _remainingMovementTime--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collidedDirection = _currentDirection;
        UpdateDirection(Direction.DONT_MOVE);
        UpdateAnimProps();
        UpdateVelocity();
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, AWARANESS_RADIUS);
    }

    private void UpdateDirection(Direction direction)
    {
        if(direction == _collidedDirection)
        {
            return;
        }

        _collidedDirection = null;
        _remainingMovementTime = (int)Random.Range(200f, 500f);
        _currentDirection = direction;
    }

    private void UpdateVelocity()
    {
        switch (_currentDirection)
        {
            case Direction.BOTTOM:
                _velocity.Set(0, -SPEED);
                break;
            case Direction.TOP:
                _velocity.Set(0, SPEED);
                break;
            case Direction.LEFT:
                _velocity.Set(-SPEED, 0);
                break;
            case Direction.RIGHT:
                _velocity.Set(SPEED, 0);
                break;
            case Direction.DONT_MOVE:
                _velocity.Set(0, 0);
                break;
        }

        _rigidbody.velocity = _velocity;
    }

    private void UpdateAnimProps()
    {
        _animator.SetBool("WalkRight", _currentDirection == Direction.RIGHT);
        _animator.SetBool("WalkLeft", _currentDirection == Direction.LEFT);
        _animator.SetBool("WalkTop", _currentDirection == Direction.TOP);
        _animator.SetBool("WalkBottom", _currentDirection == Direction.BOTTOM);
    }

    private void Scream()
    {
        int numOfSounds = _npcInfo.sounds.Length;
        SoundInfo sound = _npcInfo.sounds[Random.Range(0, numOfSounds - 1)];

        _audioSource.loop = sound.loop;
        _audioSource.clip = sound.clip;
        _audioSource.volume = sound.volume;
        _audioSource.Play();
    }
}
