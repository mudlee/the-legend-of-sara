using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class TreasureHandler : MonoBehaviour {
    [SerializeField] private TreasureInfo _treasureInfo;
    [SerializeField] private GameObject _pickupAnim;

	private void Start ()
    {
        GetComponent<SpriteRenderer>().sprite = _treasureInfo.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if(playerController)
        {
            playerController.TreasureFound(_treasureInfo.point);
            GameObject anim = Instantiate(_pickupAnim) as GameObject;
            anim.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
