using System.Runtime.InteropServices;
using Players;

namespace GunDuel
{
    class GameState
    {
        public const int MaxRounds = 7;
        private int round;
        private Dictionary<PlayerIndex, int> ammo;
        private Dictionary<PlayerIndex, int> scoreboard;

        public GameState()
        {
            round = 0;

            ammo = new Dictionary<PlayerIndex, int>();
            ammo[PlayerIndex.Player1] = 0;
            ammo[PlayerIndex.Player2] = 0;

            scoreboard = new Dictionary<PlayerIndex, int>();
            scoreboard[PlayerIndex.Player1] = 0;
            scoreboard[PlayerIndex.Player2] = 0;
        }

        public void NewRound()
        {
            round++;
            ammo[PlayerIndex.Player1] = 0;
            ammo[PlayerIndex.Player2] = 0;
        }

        public void RecordRoundResult(PlayerIndex winner)
        {
            if (winner == PlayerIndex.Undefined) return;
            scoreboard[winner]++;
        }

        public bool IsGameOver()
        {
            if (round >= MaxRounds) return true;
            if ((scoreboard[PlayerIndex.Player1] > (MaxRounds / 2)) ||
                (scoreboard[PlayerIndex.Player2] > (MaxRounds / 2))) return true;
            return false;
        }

        public PlayerIndex GetWinner()
        {
            if (scoreboard[PlayerIndex.Player1] > scoreboard[PlayerIndex.Player2]) return PlayerIndex.Player1;
            if (scoreboard[PlayerIndex.Player2] > scoreboard[PlayerIndex.Player1]) return PlayerIndex.Player2;
            return PlayerIndex.Undefined;
        }

        public int GetAmmo(PlayerIndex id)
        {
            if (id == PlayerIndex.Player1 || id == PlayerIndex.Player2)
            {
                return ammo[id];
            }

            return -1;
        }

        public void AddAmmo(PlayerIndex id)
        {
            ammo[id]++;
        }

        public bool TryUseAmmo(PlayerIndex id, int amount)
        {
            if (ammo[id] >= amount)
            {
                ammo[id] -= amount;
                return true;
            }

            ammo[id] = 0;
            return false;
        }
    }
}