﻿using UnityEngine;
namespace Filibusters
{
    public class GameConstants
    {
        public static readonly int VERSION_MAJOR = 0;
        public static readonly int VERSION_MINOR = 1;
        public static readonly string VERSION_STRING = VERSION_MAJOR.ToString() + "." +
                VERSION_MINOR.ToString();

        public static readonly int MAX_PLAYER_HEALTH = 100;
        public static readonly int AMOUNT_OF_COINS_TO_WIN = 10;
        public static readonly int MAX_ONLINE_PLAYERS_IN_GAME = 4;

        public enum WeaponId
        {
            FISTS = 0,
            VETO = 1,
            MAGIC_BULLET = 2,
            ANARCHY = 3,
            LIBEL_AND_SLANDER = 4
        }
    }

    public enum Aim
    {
        RIGHT = 0,
        RIGHT_UP = 1,
        UP = 2,
        LEFT_UP = 3,
        LEFT = 4,
        LEFT_DOWN = 5,
        DOWN = 6,
        RIGHT_DOWN = 7        
    }

    public class Layers
    {
        public readonly static string PLAYER = "Player";  
    }

    public static class Tags
    {
        public static readonly string PLAYER = "Player";
        public static readonly string RESPAWN = "Respawn";
        public static readonly string DEPOSIT = "Deposit";
        public static readonly string INACTIVE_OVERLAY = "InactiveIndicator";
    }

    public class Scenes
    {
        public static readonly string START_MENU = "Scenes/StartMenu";
        public static readonly string READY_MENU = "Scenes/ReadyMenu";
        public static readonly string MAIN = "Scenes/Main";
        public static readonly string GAME_OVER = "Scenes/GameOver";
    }
}
