namespace EnterTheLoop
{
    public class GoonHistory : TurnHistory
    {
        #pragma warning disable 8618
        private Goon attackingGoon;
        private Character characterAttacked;
        private int queueIndex;
        private int dmgDealt;
        private bool isCharacterDead;
        private int characterDamageTaken;

        public GoonHistory(int turnCounter, Goon attackingGoon, Character characterAttacked, int queueIndex, int dmgDealt, bool isCharacterDead, int characterDamageTaken)
        {
            this.turnCounter = turnCounter;
            this.AttackingGoon = attackingGoon;
            this.CharacterAttacked = characterAttacked;
            this.QueueIndex = queueIndex;
            this.DmgDealt = dmgDealt;
            this.IsCharacterDead = isCharacterDead;
            this.CharacterDamageTaken = characterDamageTaken;
        }

        public Goon AttackingGoon { get => attackingGoon; set => attackingGoon = value; }
        public Character CharacterAttacked { get => characterAttacked; set => characterAttacked = value; }
        public int QueueIndex { get => queueIndex; set => queueIndex = value; }
        public int DmgDealt { get => dmgDealt; set => dmgDealt = value; }
        public bool IsCharacterDead { get => isCharacterDead; set => isCharacterDead = value; }
        public int CharacterDamageTaken { get => characterDamageTaken; set => characterDamageTaken = value; }
    }
}