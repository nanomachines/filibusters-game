using UnityEngine;

namespace Filibusters
{
    public static class EventSystem
    {
        public delegate void DeathListener(int playerViewId);
        public static event DeathListener OnDeathEvent;
        public static void OnDeath(int playerViewId)
        {
            if (OnDeathEvent != null)
            {
                OnDeathEvent(playerViewId);
            }
        }

        public delegate void PlayerHitListener(int playerViewId);
        public static event PlayerHitListener OnPlayerHitEvent;
        public static void OnPlayerHit(int playerViewId)
        {
            if (OnPlayerHitEvent != null)
            {
                OnPlayerHitEvent(playerViewId);
            }
        }

        public delegate void JumpListener();
        public static event JumpListener OnJumpEvent;
        public static void OnJump()
        {
            if (OnJumpEvent != null)
            {
                OnJumpEvent();
            }
        }

        public delegate void CoinCollectionListener(int actorId);
        public static event CoinCollectionListener OnCoinCollectedEvent;
        public static void OnCoinCollected(int actorId)
        {
            if (OnCoinCollectedEvent != null)
            {
                OnCoinCollectedEvent(actorId);
            }
        }

        public delegate void CoinCountUpdatedListener(int actorId, int newCoinCount);
        public static event CoinCountUpdatedListener OnCoinCountUpdatedEvent;
        public static void OnCoinCountUpdated(int actorId, int newCoinCount)
        {
            if (OnCoinCountUpdatedEvent != null)
            {
                OnCoinCountUpdatedEvent(actorId, newCoinCount);
            }
        }

        public delegate void CoinDepositListener(int ownerId, int newDepositBalance);
        public static event CoinDepositListener OnCoinDepositedEvent;
        public static void OnCoinDeposited(int ownerId, int newDepositBalance)
        {
            if (OnCoinDepositedEvent != null)
            {
                OnCoinDepositedEvent(ownerId, newDepositBalance);
            }
        }

        public delegate void EquipWeaponListener(int actorId, GameConstants.WeaponId weaponId);
        public static event EquipWeaponListener OnEquipWeaponEvent;
        public static void OnEquipWeapon(int actorId, GameConstants.WeaponId weaponId)
        {
            if (OnEquipWeaponEvent != null)
            {
                OnEquipWeaponEvent(actorId, weaponId);
            }
        }

        public delegate void WeaponFireListener(int actorId, GameConstants.WeaponId weaponId);
        public static event WeaponFireListener OnWeaponFiredEvent;
        public static void OnWeaponFired(int actorId, GameConstants.WeaponId weaponId)
        {
            if (OnWeaponFiredEvent != null)
            {
                OnWeaponFiredEvent(actorId, weaponId);
            }
        }

        public delegate void WeaponMisfireListener(int actorId, GameConstants.WeaponId weaponId);
        public static event WeaponMisfireListener OnWeaponMisfiredEvent;
        public static void OnWeaponMisfired(int actorId, GameConstants.WeaponId weaponId)
        {
            if (OnWeaponMisfiredEvent != null)
            {
                OnWeaponMisfiredEvent(actorId, weaponId);
            }
        }

        public delegate void GameOverListener(int winningActorId);
        public static event GameOverListener OnGameOverEvent;
        public static void OnGameOver(int winningActorId)
        {
            if (OnGameOverEvent != null)
            {
                OnGameOverEvent(winningActorId);
            }
        }

        public delegate void WeaponDropListener(int actorId);
        public static event WeaponDropListener OnWeaponDropEvent;
        public static void OnWeaponDrop(int actorId)
        {
            if (OnWeaponDropEvent!= null)
            {
                OnWeaponDropEvent(actorId);
            }
        }

        public delegate void UpdateHealthBarListener(int playerHealth);
        public static event UpdateHealthBarListener OnUpdateHealthBarEvent;
        public static void OnUpdateHealthBar(int playerHealth)
        {
            if (OnUpdateHealthBarEvent != null)
            {
                OnUpdateHealthBarEvent(playerHealth);
            }
        }

        public delegate void AllPlayersReadyListener();
        public static event AllPlayersReadyListener OnAllPlayersReadyEvent;
        public static void OnAllPlayersReady()
        {
            if (OnAllPlayersReadyEvent != null)
            {
                OnAllPlayersReadyEvent();
            }
        }

        public delegate void AllPlayersNotReadyListener();
        public static event AllPlayersNotReadyListener OnAllPlayersNotReadyEvent;
        public static void OnAllPlayersNotReady()
        {
            if (OnAllPlayersNotReadyEvent != null)
            {
                OnAllPlayersNotReadyEvent();
            }
        }

        public delegate void DepositBeginListener(int viewId);
        public static event DepositBeginListener OnDepositBeginEvent;
        public static void OnDepositBegin(int viewId)
        {
            if (OnDepositBeginEvent != null)
            {
                OnDepositBeginEvent(viewId);
            }
        }

        public delegate void DepositEndListener();
        public static event DepositEndListener OnDepositEndEvent;
        public static void OnDepositEnd()
        {
            if (OnDepositEndEvent != null)
            {
                OnDepositEndEvent();
            }
        }
    }
}
