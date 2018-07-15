using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _npcToSpawn;
    private GameObject _npcRef;

    private void Awake()
    {
        EventManager.StartListening(EventManager.Event.GAME_STARTED, () =>
        {
            print("???");
            _npcRef = Instantiate(_npcToSpawn) as GameObject;
            _npcRef.transform.position = transform.position;
        });
    }
}
