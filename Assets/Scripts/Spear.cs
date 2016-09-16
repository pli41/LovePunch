using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{

    [SerializeField]
    float damage;
    bool isDetached = false;

    // Use this for initialization
    void Start()
    {

    }

    public bool checkDetached()
    {
        return isDetached;
    }

    public void DetachFromEnemy()
    {
        isDetached = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (checkDetached())
        {
            // if it hits enemy and is detached (meaning it is thrown), deal damage to enemy
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
                Destroy(gameObject, 2f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
