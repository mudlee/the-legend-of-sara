﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCHandler : MonoBehaviour
{
    // CONSTANTS
    private const float SPEED = 0.4f;
    private const int AWARANESS_RADIUS_HIGH = 7;
    private const int AWARANESS_RADIUS_LOW = 10;
    private const float FOLLOW_PLAYER_MOVE_TRESHOLD = 2f;
    private const int FIRE_RATE = 2;

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

    // AWARANESS
    private enum AwaranessLevel { LOST, LOW, HIGH };
    private static AwaranessLevel _awaranessLevel;

    // FIRE
    private float _lastFireTime = 0;

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

        InvokeRepeating("Scream", Random.Range(2f, 10f), Random.Range(3f, 5f));

        EventManager.StartListening(EventManager.Event.PLAYER_DIED, () => {
            Destroy(this.gameObject);
        });
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        bool followPlayer = false;

        if (distance <= AWARANESS_RADIUS_HIGH)
        {
            if (NPCHandler._awaranessLevel != AwaranessLevel.HIGH)
            {
                NPCHandler._awaranessLevel = AwaranessLevel.HIGH;
                EventManager.TriggerEvent(EventManager.Event.ENEMY_ATTENTION_HIGH);
            }

            followPlayer = true;

            if(Time.timeSinceLevelLoad > _lastFireTime + FIRE_RATE)
            {
                Fire();
            }
        }
        else if (distance <= AWARANESS_RADIUS_LOW)
        {
            if (NPCHandler._awaranessLevel != AwaranessLevel.LOW)
            {
                NPCHandler._awaranessLevel = AwaranessLevel.LOW;
                EventManager.TriggerEvent(EventManager.Event.ENEMY_ATTENTION_LOW);
            }

            followPlayer = true;
        }
        else if (NPCHandler._awaranessLevel != AwaranessLevel.LOST)
        {
            NPCHandler._awaranessLevel = AwaranessLevel.LOST;
            EventManager.TriggerEvent(EventManager.Event.ENEMY_ATTENTION_LOST);
        }

        if(followPlayer)
        {
            float horizontalDiff = Mathf.Abs(_player.transform.position.x - transform.position.x);
            float verticalDiff = Mathf.Abs(_player.transform.position.y - transform.position.y);

            if (horizontalDiff > FOLLOW_PLAYER_MOVE_TRESHOLD)
            {
                UpdateDirection(_player.transform.position.x < transform.position.x ? Direction.LEFT : Direction.RIGHT);
            }
            else if (verticalDiff > FOLLOW_PLAYER_MOVE_TRESHOLD)
            {
                UpdateDirection(_player.transform.position.y < transform.position.y ? Direction.BOTTOM : Direction.TOP);
            }
            else
            {
                UpdateDirection(Direction.DONT_MOVE);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_remainingMovementTime == 0)
        {
            UpdateDirection((Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length));
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
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, AWARANESS_RADIUS_LOW);

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, AWARANESS_RADIUS_HIGH);
#endif
    }

    private void UpdateDirection(Direction direction)
    {
        if (direction != _collidedDirection)
        {
            _collidedDirection = null;
            _remainingMovementTime = (int)Random.Range(200f, 500f);
            _currentDirection = direction;

            UpdateAnimProps();
            UpdateVelocity();
        }
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
        if(!_audioSource.isPlaying)
        {
            PlaySound(_npcInfo.sounds[Random.Range(0, _npcInfo.sounds.Length - 1)]);
        }
    }

    private void Fire()
    {
        _lastFireTime = Time.timeSinceLevelLoad;
        GameObject projectile = Instantiate(_npcInfo.projectile) as GameObject;
        projectile.transform.position = transform.position;

        PlaySound(_npcInfo.fireSound);
    }

    private void PlaySound(SoundInfo sound)
    {
        _audioSource.Stop();
        _audioSource.loop = sound.loop;
        _audioSource.clip = sound.clip;
        _audioSource.volume = sound.volume;
        _audioSource.Play();
    }
}
