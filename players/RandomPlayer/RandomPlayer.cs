using GunDuel;

namespace Players
{
    class RandomPlayer : Player
    {
        private readonly static Random rng = new();

        public override string GetName()
        {
            return "Random";
        }

        public override Move PickMove()
        {
            return (Move)rng.NextInt64(4);
        }
    }
}