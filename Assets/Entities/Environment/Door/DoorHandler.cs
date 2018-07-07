using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class DoorHandler : MonoBehaviour {
    public enum DoorId { ROOM2_DOOR };
    [SerializeField] private DoorId _door;
    private UIHandler _uIHandler;
    private SoundPlayer _soundPlayer;
    private Animator _doorAnimator;
    private BoxCollider2D _collider;
    private Dictionary<DoorId, string[]> _messages = new Dictionary<DoorId, string[]>{
        { DoorId.ROOM2_DOOR, new string[]{
            "I need the answer to life, the Universe and everything.",
            "Didn't you see the The Hitchhiker's Guide to the Galaxy?",
            "Well, OK. Check the floor ;)"
        }}
    };

    private Dictionary<DoorId, int> _tries = new Dictionary<DoorId, int>() {
        { DoorId.ROOM2_DOOR, 0 }
    };

    private void Start()
    {
        _uIHandler = FindObjectOfType<UIHandler>();
        _doorAnimator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _soundPlayer = FindObjectOfType<SoundPlayer>();

        switch (_door) {
            case DoorId.ROOM2_DOOR:
                {
                    EventManager.StartListening(EventManager.Event.ROOM2_DOOR_QUESTION_ANSWERED, () => {
                        _doorAnimator.SetTrigger("OpenTrigger");
                        _collider.enabled = false;
                        _soundPlayer?.Play(Sound.DOOR_OPEN);
                    });
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int index=0;
        _tries.TryGetValue(_door, out index);

        string[] doorMessages;
        _messages.TryGetValue(_door, out doorMessages);
        string message = doorMessages[index];
        _tries[_door] = (index + 1) > doorMessages.Length - 1 ? 0 : index + 1;

        _uIHandler.Notification.ShowNotification(message);
    }
}
