//I don't know if is ti correct or it will work 



using System;

public class TriggerEnemy : MonoBehaviour
{
    private Render _render;

    void Start()
    {
        _render = GetComponenmt<Render>();
    }

    private void OnTriggerEnter(collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyAI_2 enemy = other.getComponent<EnemyAI_2>();

            if (enemy != null)
            {
                _render.enabled = true;
            }
        }
    }
}
