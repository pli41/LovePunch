using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    [SerializeField]
    float damage;
    bool isDetached = false;

    [SerializeField]
    float power = 1.0f;
    [SerializeField]
    float explosionRadius = 4.0f;

	public GameObject explosionEffect;

    Timer bombTimer;

    // Use this for initialization
    void Start ()
    {
        bombTimer = new Timer(5f, Explode, false);
    }

    public bool CheckDetached()
    {
        return isDetached;
    }

    public void DetachFromEnemy()
    {
        isDetached = true;
    }

    //Explode and add explosive force
    public void Explode()
    {
        
		Debug.Log ("EXPLODE");

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
			//Enemy Damage
            if (hit.gameObject.CompareTag("Enemy"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, transform.position, explosionRadius, 2.0F, ForceMode.Impulse);
                hit.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
            }

			//Prince damage
			if (hit.gameObject.CompareTag ("Prince")) 
			{
				hit.gameObject.GetComponent<Prince> ().ReceiveDamage (damage, false);
			}
        }

		GameObject explosion = (GameObject)Instantiate(explosionEffect, transform.position, transform.rotation);
		Destroy (gameObject, 1f);
		Destroy (explosion, 5f);

    }

    // Update is called once per frame
    void Update () {

        if (isDetached == true)
        {
			bombTimer.RunTimer();
        }

	}
}
