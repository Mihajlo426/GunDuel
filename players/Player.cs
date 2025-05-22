using GunDuel;

namespace Players
{
    abstract class Player
    {
        #pragma warning disable CS8618 // Rethrow to preserve stack details
        private GameState gameState;
        #pragma warning restore CS8618 // Rethrow to preserve stack details
        private PlayerIndex id;

        public void Initialize(PlayerIndex idx, GameState state)
        {
            id = idx;
            gameState = state;
        }

        public abstract string GetName();
        public abstract Move PickMove();

        protected int GetMyAmmo()
        {
            return gameState.GetAmmo(id);
        }

        protected int GetOpponentAmmo()
        {
            PlayerIndex opponent = (id == PlayerIndex.Player1 ? PlayerIndex.Player2 : PlayerIndex.Player1);
            return gameState.GetAmmo(opponent);
        }
    }
}