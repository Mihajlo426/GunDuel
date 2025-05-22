
namespace GunDuel
{
    using Players;

    class GameRunner
    {
        private GameState State;
        private Dictionary<PlayerIndex, Player> players;
        private PlayerIndex winner = PlayerIndex.Undefined;

        public GameRunner()
        {
            State = new GameState();

            Player p1 = new AggressivePlayer();
            Player p2 = new RandomPlayer();

            players = new Dictionary<PlayerIndex, Player>
            {
                [PlayerIndex.Player1] = p1,
                [PlayerIndex.Player2] = p2,
            };

            p1.Initialize(PlayerIndex.Player1, State);
            p2.Initialize(PlayerIndex.Player2, State);
        }

        public void Run()
        {
            while (!State.IsGameOver())
            {
                State.NewRound();
                DisplayRoundHeader();
                PlayRound();
            }

            winner = State.GetWinner();
            Console.WriteLine($"{players[winner].GetName()} WINS EVERYTHING");
        }

        private void PlayRound()
        {
            while (true)
            {
                Move move1 = players[PlayerIndex.Player1].PickMove();
                Move move2 = players[PlayerIndex.Player2].PickMove();
                State.RecordMoves(move1, move2);

                Outcome outcome = this.ResolveTurn(move1, move2);
               
                DisplayMoveAndAmmo(move1, move2);

                switch (outcome)
                {
                    case Outcome.BothDead:
                        State.RecordRoundResult(PlayerIndex.Undefined);
                        Console.WriteLine("DRAW");
                        return;

                    case Outcome.P1Dead:
                        State.RecordRoundResult(PlayerIndex.Player2);
                        Console.WriteLine($"{players[PlayerIndex.Player2].GetName()} WIN");
                        return;

                    case Outcome.P2Dead:
                        State.RecordRoundResult(PlayerIndex.Player1);
                        Console.WriteLine($"{players[PlayerIndex.Player1].GetName()} WIN");
                        return;

                    default:
                        // no elimination: show a quick message, then loop again
                        Console.WriteLine("…no elimination, keep fighting…");
                        Thread.Sleep(2000);
                        continue;
                }
            }
        }

        private Outcome ResolveTurn(Move m1, Move m2)
        {
            // Apply ammo gains
            if (m1 == Move.Charge) State.AddAmmo(PlayerIndex.Player1);
            if (m2 == Move.Charge) State.AddAmmo(PlayerIndex.Player2);

            // Attempt to spend ammo for attacks
            bool shot1 = (m1 == Move.Shoot) && State.TryUseAmmo(PlayerIndex.Player1, 1);
            bool bomb1 = (m1 == Move.Bomb) && State.TryUseAmmo(PlayerIndex.Player1, 3);
            bool shot2 = (m2 == Move.Shoot) && State.TryUseAmmo(PlayerIndex.Player2, 1);
            bool bomb2 = (m2 == Move.Bomb) && State.TryUseAmmo(PlayerIndex.Player2, 3);

            // Determine who lost
            bool p1Dead = (shot2 && m1 != Move.Defend) || bomb2;
            bool p2Dead = (shot1 && m2 != Move.Defend) || bomb1;

            return p1Dead && p2Dead ? Outcome.BothDead
                 : p1Dead ? Outcome.P1Dead
                 : p2Dead ? Outcome.P2Dead
                 : Outcome.None;
        }
        
    private void DisplayRoundHeader()
    {
        Console.Clear();
        Console.WriteLine($"=== Round {State.GetRound()}/{GameState.MaxRounds} ===");
        Console.WriteLine($"Score : {players[PlayerIndex.Player1].GetName()} = {State.GetScore(PlayerIndex.Player1)}, " +
                          $"{players[PlayerIndex.Player2].GetName()} = {State.GetScore(PlayerIndex.Player2)}");
        Console.WriteLine(new string('-', 40));
        // Leave the cursor here—this is where move info will go
    }

    private void DisplayMoveAndAmmo(Move m1, Move m2)
    {
        // Move the cursor to line 4 (zero-based): header is 0,1,2, so we start printing at 3
        Console.SetCursorPosition(0, 3);

        // Overwrite the previous move info by printing blank lines first
        Console.WriteLine(new string(' ', 40));
        Console.WriteLine(new string(' ', 40));

        // Put cursor back and print fresh move/ammo info
        Console.SetCursorPosition(0, 3);
        Console.WriteLine($"{players[PlayerIndex.Player1].GetName()} → {m1,-6}   Ammo: {State.GetAmmo(PlayerIndex.Player1)}");
        Console.WriteLine($"{players[PlayerIndex.Player2].GetName()} → {m2,-6}   Ammo: {State.GetAmmo(PlayerIndex.Player2)}");
    }
    }
}