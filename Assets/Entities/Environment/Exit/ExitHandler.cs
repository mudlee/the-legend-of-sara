using UnityEngine;

public class ExitHandler : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventManager.TriggerEvent(EventManager.Event.PLAYER_REACHED_EXIT);
    }
}
