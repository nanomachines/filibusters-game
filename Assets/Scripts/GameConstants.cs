using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class GameConstants : MonoBehaviour
    {
        public static int VERSION_MAJOR = 0;
        public static int VERSION_MINOR = 1;
        public static string VERSION_STRING = VERSION_MAJOR.ToString() + "." +
                VERSION_MINOR.ToString();

        public static int AMOUNT_OF_COINS_TO_WIN = 3;
        public static int MAX_ONLINE_PLAYERS_IN_GAME = 4;

        public enum WeaponId { FISTS, VETO, MAGIC_BULLET, ANARCHY, LIBEL_AND_SLANDER }
    }
}
