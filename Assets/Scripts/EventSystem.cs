﻿using UnityEngine;

namespace Filibusters
{
    public static class EventSystem
    {
        public delegate void DeathListener(int playerViewId, Vector3 pos);
        public static event DeathListener OnDeathEvent;
        public static void OnDeath(int playerViewId, Vector3 pos)
        {
            if (OnDeathEvent != null)
            {
                OnDeathEvent(playerViewId, pos);
            }
        }

        public delegate void KillListener(int killerOwnerId);
        public static event KillListener OnKilledEvent;
        public static void OnPlayerKilled(int killerOwnerId)
        {
            if (OnKilledEvent != null)
            {
                OnKilledEvent(killerOwnerId);
            }
        }

        public delegate void PlayerHitListener(int playerViewId, Vector3 pos);
        public static event PlayerHitListener OnPlayerHitEvent;
        public static void OnPlayerHit(int playerViewId, Vector3 pos)
        {
            if (OnPlayerHitEvent != null)
            {
                OnPlayerHitEvent(playerViewId, pos);
            }
        }

        public delegate void JumpListener(Vector3 pos);
        public static event JumpListener OnJumpEvent;
        public static void OnJump(Vector3 pos)
        {
            if (OnJumpEvent != null)
            {
                OnJumpEvent(pos);
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

        public delegate void CoinDepositListener(int ownerId, int newDepositBalance, Vector3 pos);
        public static event CoinDepositListener OnCoinDepositedEvent;
        public static void OnCoinDeposited(int ownerId, int newDepositBalance, Vector3 pos)
        {
            if (OnCoinDepositedEvent != null)
            {
                OnCoinDepositedEvent(ownerId, newDepositBalance, pos);
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

        public delegate void WeaponFireListener(GameConstants.WeaponId weaponId, Vector3 pos);
        public static event WeaponFireListener OnWeaponFiredEvent;
        public static void OnWeaponFired(GameConstants.WeaponId weaponId, Vector3 pos)
        {
            if (OnWeaponFiredEvent != null)
            {
                OnWeaponFiredEvent(weaponId, pos);
            }
        }

        public delegate void WeaponMisfireListener(GameConstants.WeaponId weaponId);
        public static event WeaponMisfireListener OnWeaponMisfiredEvent;
        public static void OnWeaponMisfired(GameConstants.WeaponId weaponId)
        {
            if (OnWeaponMisfiredEvent != null)
            {
                OnWeaponMisfiredEvent(weaponId);
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

        public delegate void GameOverJiggleListener(bool isWinner);
        public static event GameOverJiggleListener OnGameOverJiggleEvent;
        public static void OnGameOverJiggle(bool isWinner)
        {
            if (OnGameOverJiggleEvent != null)
            {
                OnGameOverJiggleEvent(isWinner);
            }
        }

        public delegate void LeadingPlayerUpdateListener(int leadingPlayerNum);
        public static event LeadingPlayerUpdateListener OnLeadingPlayerUpdatedEvent;
        public static void OnLeadingPlayerUpdated(int leadingPlayerNum)
        {
            if (OnLeadingPlayerUpdatedEvent != null)
            {
                OnLeadingPlayerUpdatedEvent(leadingPlayerNum);
            }
        }

        public delegate void HostOrJoinFailedListener();
        public static event HostOrJoinFailedListener OnHostOrJoinFailedEvent;
        public static void OnHostOrJoinFailed()
        {
            if (OnHostOrJoinFailedEvent != null)
            {
                OnHostOrJoinFailedEvent();
            }
        }

        public delegate void SuicideListener(int playerNum);
        public static event SuicideListener OnSuicideEvent;
        public static void OnSuicide(int playerNum)
        {
            if (OnSuicideEvent != null)
            {
                OnSuicideEvent(playerNum);
            }
        }
    }
}
