using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class KnockbackTrigger : MonoBehaviour
	{
        private PhotonView mPhotonView;
        private SimplePhysics mCharacterController;

        void Start()
        {
            mPhotonView = GetComponent<PhotonView>();
            mCharacterController = GetComponent<SimplePhysics>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Trigger();
            }
        }

        void Trigger()
        {
            mPhotonView.RPC("TriggerRPC", mPhotonView.owner);
        }

        [PunRPC]
        void TriggerRPC()
        {
            mCharacterController.RunKnockback();
        }
	}
}
