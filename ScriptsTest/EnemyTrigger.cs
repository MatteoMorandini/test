using System;

public class TriggerEnemy : MonoBehaviour
{
    private void OnTriggerEnter(collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyAI_2 enemy = other.getComponent<EnemyAI_2>();

            if (enemy != null)
            {
                enemy.ActivateEnemy();
            }
        }
    }
}
