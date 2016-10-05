using UnityEngine;
namespace Filibusters
{
    public class GameConstants
    {
        public static int VERSION_MAJOR = 0;
        public static int VERSION_MINOR = 1;
        public static string VERSION_STRING = VERSION_MAJOR.ToString() + "." +
                VERSION_MINOR.ToString();

        public static int AMOUNT_OF_COINS_TO_WIN = 3;
        public static int MAX_ONLINE_PLAYERS_IN_GAME = 4;

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
}
