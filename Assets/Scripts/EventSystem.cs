﻿namespace Filibusters
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

        public delegate void JumpListener();
        public static event JumpListener OnJumpEvent;
        public static void OnJump()
        {
            if (OnJumpEvent != null)
            {
                OnJumpEvent();
            }
        }
    }
}