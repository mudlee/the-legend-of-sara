using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class TreasureHandler : MonoBehaviour {
    [SerializeField] private TreasureInfo _treasureInfo;
    [SerializeField] private GameObject _pickupAnim;
    private SoundPlayer _soundPlayer;

    private void Start ()
    {
        GetComponent<SpriteRenderer>().sprite = _treasureInfo.sprite;
        _soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if(playerController)
        {
            _soundPlayer.Play(Sound.TREASURE_PICKUP);
            playerController.TreasureFound(_treasureInfo.point, _treasureInfo.heal);
            GameObject anim = Instantiate(_pickupAnim) as GameObject;
            anim.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
