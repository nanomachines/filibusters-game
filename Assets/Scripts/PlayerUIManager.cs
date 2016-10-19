using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

namespace Filibusters
{
    public class PlayerUIManager : MonoBehaviour 
    {
        private Slider mHealthBarControl;
        private Image mHealthBar;

        void Start()
        {
            // Assign Coin and Votes Text elements
            GameObject child = Utility.GetChildWithTag(gameObject, Tags.HEALTH_BAR);
            mHealthBarControl = child.GetComponent<Slider>();
            mHealthBar = mHealthBarControl.fillRect.gameObject.GetComponentInChildren<Image>();

            // Check for null elements
            Assert.IsNotNull(mHealthBarControl, "No health bar slider attached to the player ui! Tag a slider object with a HealthBar tag.");
            Assert.IsNotNull(mHealthBar, "No health bar image attached to the player ui! Tag a Image object parented under the Slider RectTransform with a HealthBar tag.");

            EventSystem.OnUpdateHealthBarEvent += UpdateHealthBar;
        }

        void UpdateHealthBar(int health)
        {
            mHealthBarControl.value = health;
        }
    }

}
