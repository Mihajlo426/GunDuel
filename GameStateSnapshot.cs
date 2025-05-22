namespace GunDuel
{
    struct GameStateSnapshot
    {
        int ammo1;
        int ammo2;
        Move move1;
        Move move2;

        public GameStateSnapshot(int a1, int a2, Move m1, Move m2)
        {
            ammo1 = a1;
            ammo2 = a2;
            move1 = m1;
            move2 = m2;
        }
    }
}