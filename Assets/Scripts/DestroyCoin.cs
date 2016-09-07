using UnityEngine;
using System.Collections;

public class DestroyCoin : MonoBehaviour {

    private SpriteRenderer myRenderer = null;
    private BoxCollider2D boxCollider = null;

    [SerializeField]
    private int waitSecs = 10;

    IEnumerator HideAndRespawn()
    {
        myRenderer.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(waitSecs);
        myRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SoundManager.instance.Play("coin");
            CoinManager coinManager = other.gameObject.GetComponent<CoinManager>();
            coinManager.AddCoin();
            StartCoroutine(HideAndRespawn());
        }
    }
}
