using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class DepositStatusController : MonoBehaviour {

        SpriteRenderer mSpriteRenderer;
        Color green = new Color(70 / 255f, 227 / 255f, 110 / 255f, 0.5f);
        Color red = new Color(227 / 255f, 70 / 255f, 70 / 255f, 0.5f);

        // Use this for initialization
        void Start() {
            mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update() {
            DepositManager dm = transform.parent.gameObject.GetComponent<DepositManager>();
            if (dm.isDepositing())
            {
                mSpriteRenderer.color = green;
            }
            else if (dm.isBlocked())
            {
                mSpriteRenderer.color = red;
            }
            else
            {
                mSpriteRenderer.color = new Color(0, 0, 0, 0);
            }
        }
    }
}