using UnityEngine;

public class PlayerProjectileHandler : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        NPCHandler npc = collision.gameObject.GetComponent<NPCHandler>();
        if (npc != null)
        {
            npc.Damage(10);
        }

        Destroy(this.gameObject);
    }
}
