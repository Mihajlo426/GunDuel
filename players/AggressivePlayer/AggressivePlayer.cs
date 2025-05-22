using GunDuel;

namespace Players
{
    class AggressivePlayer : Player
    {
        public override string GetName()
        {
            return "Aggro";
        }

        public override Move PickMove()
        {
            if (GetMyAmmo() == 0) return Move.Charge;
            return Move.Shoot;
        }
    }
}