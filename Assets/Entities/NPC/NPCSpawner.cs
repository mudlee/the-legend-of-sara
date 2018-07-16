using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _npcToSpawn;

    private void Awake()
    {
        EventManager.StartListening(EventManager.Event.RESET_GAME, () =>
        {
            GameObject npc = Instantiate(_npcToSpawn) as GameObject;
            npc.transform.position = transform.position;
        });
    }
}
