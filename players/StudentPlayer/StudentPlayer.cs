using GunDuel;

namespace Players
{
    /// <summary>
    /// U svakom potezu, igrica ce pozvati metodu PickMove vaseg igraca.
    /// </summary>
    class StudentPlayer : Player
    {
        public override string GetName()
        {
            return "VASE IME";
        }

        public override Move PickMove()
        {
            return Move.Defend;
        }
    }
}