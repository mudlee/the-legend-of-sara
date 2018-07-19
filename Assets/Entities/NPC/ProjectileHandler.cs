using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileHandler : MonoBehaviour {
    [SerializeField] private ProjectileInfo _projectileInfo;

    private void Start ()
    {
        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized * _projectileInfo.speed;
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if(playerController != null)
        {
            playerController.Damage(_projectileInfo.damage, _projectileInfo.slowDown);
        }

        Destroy(this.gameObject);
    }
}
