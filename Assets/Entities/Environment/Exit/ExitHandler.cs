using UnityEngine;

public class ExitHandler : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            EventManager.TriggerEvent(EventManager.Event.PLAYER_REACHED_EXIT);
        }
    }
}
