using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class NPCHandler : MonoBehaviour
{
    [SerializeField] private NPCInfo _npcInfo;
    private AudioSource _audioSource;
    private Animator _animator;
    //private const float SPEED = 1f;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        InvokeRepeating("Scream",Random.Range(2f,10f),Random.Range(1f,5f));
    }

    private void Update()
    {
        /*float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _animator.SetBool("WalkRight", horizontal > 0);
        _animator.SetBool("WalkLeft", horizontal < 0);
        _animator.SetBool("WalkTop", vertical > 0);
        _animator.SetBool("WalkBottom", vertical < 0);

        _velocity.Set(horizontal * SPEED, vertical * SPEED);
        _rigidbody.velocity = _velocity;*/
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
