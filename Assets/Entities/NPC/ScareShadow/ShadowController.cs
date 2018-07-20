using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class ShadowController : MonoBehaviour {
    private const int TRIGGER_RADIUS = 2;
    private GameObject _player;
    private AudioSource _audioSource;
    private Animator _animator;
    private bool _awaken;

    private void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, TRIGGER_RADIUS);
#endif
    }

    private void Update () {
        if (_awaken)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < TRIGGER_RADIUS)
        {
            _awaken = true;
            _animator.SetTrigger("WakeUp");
            _audioSource.Play();
        }
    }
}
