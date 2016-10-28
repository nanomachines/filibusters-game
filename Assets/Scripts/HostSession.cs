using UnityEngine.UI;
using Photon;

namespace Filibusters
{
    public class HostSession : PunBehaviour
    {
        InputField mSessionNameField;

        void Start()
        {
            mSessionNameField = GetComponent<InputField>();
            mSessionNameField.onEndEdit.AddListener(delegate { OnHostGameNameEntered(); });
        }

        public void OnHostGameNameEntered()
        {
            NetworkManager.CreateAndJoinGameSession(mSessionNameField.text.ToLower());
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }
    }
}
