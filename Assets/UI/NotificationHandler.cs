using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour {
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _notification;
    private Queue<string> _queue = new Queue<string>();
    private float _messageShownAt;
    private const float MESSAGE_TIME = 5f;

    private void Update ()
    {
        if(_notification.activeSelf && Time.time > _messageShownAt + MESSAGE_TIME)
        {
            Debug.Log("Hiding notifcation...");
            _notification.SetActive(false);
        }

        if (_queue.Count != 0)
        {
            _notification.SetActive(true);
            _text.text = _queue.Dequeue();
            Debug.Log("Showing notification " + _text.text);
            _messageShownAt = Time.time;
        }
	}

    public void ShowNotification(string text)
    {
        Debug.Log(string.Format("Adding notfiication '{0}'",text));
        _queue.Enqueue(text);
    }
}
