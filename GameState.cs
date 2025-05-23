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
        private Dictionary<int, List<GameStateSnapshot>> moveHistory;

        public GameState()
        {
            round = 0;

            ammo = new Dictionary<PlayerIndex, int>();
            ammo[PlayerIndex.Player1] = 0;
            ammo[PlayerIndex.Player2] = 0;

            scoreboard = new Dictionary<PlayerIndex, int>();
            scoreboard[PlayerIndex.Player1] = 0;
            scoreboard[PlayerIndex.Player2] = 0;

            moveHistory = new Dictionary<int, List<GameStateSnapshot>>();
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

        public void RecordMoves(Move m1, Move m2)
        {
            GameStateSnapshot snap = new GameStateSnapshot(ammo[PlayerIndex.Player1], ammo[PlayerIndex.Player2], m1, m2);
            if (!moveHistory.ContainsKey(round)) moveHistory[round] = new List<GameStateSnapshot>();
            moveHistory[round].Add(snap);
        }

        public Dictionary<int, List<GameStateSnapshot>> GetMoveHistory()
        {
            return moveHistory;
        }

        public int GetRound()
        {
            return round;
        }

        public int GetScore(PlayerIndex ind)
        {
            return scoreboard[ind];
        }
    }
}