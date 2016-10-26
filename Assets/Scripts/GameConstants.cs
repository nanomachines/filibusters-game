using UnityEngine;
using System.Collections.Generic;
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

        public static WeaponAttributes[] WeaponProperties =
        {
            new WeaponAttributes(15, 0.2f),
            new WeaponAttributes(1, 0.7f),
            new WeaponAttributes(7, 0.5f)
        };
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
        public static readonly int BARRIER = LayerMask.NameToLayer("Barrier");
        public static readonly int GROUND = LayerMask.NameToLayer("Ground");
        public static readonly int PLAYER = LayerMask.NameToLayer("Player");
    }

    public class SortingLayers
    {
        public static readonly string PLAYER = "Player";
    }

    public static class Tags
    {
        public static readonly string PLAYER = "Player";
        public static readonly string RESPAWN = "Respawn";
        public static readonly string DEPOSIT = "Deposit";
        public static readonly string INACTIVE_OVERLAY = "InactiveIndicator";
        public static readonly string HEALTH_BAR = "HealthBar";
        public static readonly string VOTE_TEXT = "VoteText";
        public static readonly string COIN_TEXT = "CoinText";
        public static readonly string MAIN_CAMERA = "MainCamera";
        public static readonly string COIN = "Coin";
        public static readonly string PLAYER_UI = "PlayerUI";
        public static readonly string WEAPON = "Gun";
    }

    public class Scenes
    {
        public static readonly string START_MENU = "Scenes/StartMenu";
        public static readonly string HOW_TO_PLAY = "Scenes/HowToPlay";
        public static readonly string READY_MENU = "Scenes/ReadyMenu";
        public static readonly string MAIN = "Scenes/Main";
        public static readonly string GAME_OVER = "Scenes/GameOver";
    }

    public struct WeaponAttributes
    {
        public int mMaxAmmo;
        public float mCoolDown;

        public WeaponAttributes(int ammo, float coolDown)
        {
            mMaxAmmo = ammo;
            mCoolDown = coolDown;
        }
    }
}
