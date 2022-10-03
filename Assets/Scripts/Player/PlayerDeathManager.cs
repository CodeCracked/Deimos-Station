using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public PlayerController Controller;
    public Rigidbody Rigidbody;

    private bool _dead;

    public void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy && !_dead)
        {
            Kill();
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController disable in enemies) disable.SetTask(null);
        }
    }

    public void Kill()
    {
        _dead = true;

        Controller.enabled = false;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;

        Vector2 rand = Random.insideUnitCircle.normalized;
        Rigidbody.AddForce(0.1f * new Vector3(rand.x, 0, rand.y), ForceMode.Impulse);

        DeathScreenManager.Show();
    }
}