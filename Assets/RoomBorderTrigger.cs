using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomBorderTrigger : MonoBehaviour {
    [SerializeField] private GameManager.RoomBorderTrigger trigger;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _gameManager.RoomBorderCrossed(trigger);
    }
}
