using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    private SpriteRenderer myRenderer = null;
    private BoxCollider2D boxCollider = null;
    private Rigidbody2D myRigidBody = null;
    private CoinManager coinManager = null;

    [SerializeField]
    private int waitSecs = 1;
    [SerializeField]
    private GameObject[] spawnPoints;

    public void Die()
    {
        coinManager.ResetCoins();
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        StartCoroutine(DieAndRespawn(spawnPoint));
    }

    IEnumerator DieAndRespawn(GameObject spawn)
    {
        myRenderer.enabled = false;
        boxCollider.enabled = false;
        myRigidBody.isKinematic = true;
        this.transform.position = spawn.transform.position;
        yield return new WaitForSeconds(waitSecs);
        myRenderer.enabled = true;
        boxCollider.enabled = true;
        myRigidBody.isKinematic = false;
    }

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        coinManager = GetComponent<CoinManager>();
        Die();
    }

    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
	}
}
