namespace EnterTheLoop
{
    public class CharacterHistory : TurnHistory
    {
        private Character c;
        private int directHitsDealt;
        private int partialHitsDealt;
        private List<Goon> goonsDamaged = new List<Goon>();
        private bool usedHeal;
        private bool killedGoon;

        public CharacterHistory(int turnCounter, Character c, Goon currentGoon, int[] attackInHits, bool usedHeal, bool killedGoon)
        {
            this.turnCounter = turnCounter;
            this.c = c;
            this.goonsDamaged.Add(currentGoon);
            this.directHitsDealt = attackInHits[0];
            this.partialHitsDealt = attackInHits[1];
            this.usedHeal = usedHeal;
            this.killedGoon = killedGoon;
        }

        public Character C { get => c; set => c = value; }
        public int DirectHitsDealt { get => directHitsDealt; set => directHitsDealt = value; }
        public int PartialHitsDealt { get => partialHitsDealt; set => partialHitsDealt = value; }
        public List<Goon> GoonsDamaged { get => goonsDamaged; set => goonsDamaged = value; }
        public bool UsedHeal { get => usedHeal; set => usedHeal = value; }
        public bool KilledGoon { get => killedGoon; set => killedGoon = value; }
    }
}