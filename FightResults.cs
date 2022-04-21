namespace EnterTheLoop
{
    public class FightResults
    {
        private Fight fight;
        private Fight fight2;
        private List<TurnHistory> turnHistory;
        private int totalTurnCount;
        private int finalCharacterHp;
        private int totalCharacterHpLost;
        private bool usedHeal;
        private bool isCharacterDead;
        private Character character;
        private Queue<Goon> goonQueue;
        private int totalDirectHits;
        private int totalPartialHits;

        public FightResults(Fight f, List<TurnHistory> turnHistory, int totalTurnCount, Character c, Queue<Goon> goonQueue)
        {
            this.fight = f;
            this.turnHistory = turnHistory;
            this.totalTurnCount = totalTurnCount;
            this.character = c;
            this.goonQueue = goonQueue;
            this.finalCharacterHp = character.Hp;
            this.isCharacterDead = character.IsDead;

            List<CharacterHistory> characterHistories = turnHistory.OfType<CharacterHistory>().ToList();
            List<GoonHistory> goonHistories = turnHistory.OfType<GoonHistory>().ToList();

            this.totalCharacterHpLost = goonHistories.Sum(gh => gh.CharacterDamageTaken);
            this.usedHeal = characterHistories.Select(ch => ch.UsedHeal).Contains(true);
            this.totalDirectHits = characterHistories.Sum(ch => ch.DirectHitsDealt);
            this.totalPartialHits = characterHistories.Sum(ch => ch.PartialHitsDealt);
        }

        public FightResults(Fight f, Fight f2, List<TurnHistory> turnHistory, int totalTurnCount, Character c, Queue<Goon> goonQueue) {
            
        }

        public override string ToString()
        {
            return $"Fight {fight.GetName()} had {totalTurnCount} total turns, with {(isCharacterDead ? "the Goons winning" : "the Character winning")}.\n" +
            $"The Character dealt {totalDirectHits} direct hits and {totalPartialHits} partial hits. The Goons dealt the Character {totalCharacterHpLost} dmg.";
        }

        public List<TurnHistory> TurnHistory { get => turnHistory; set => turnHistory = value; }
        public int TotalTurnCount { get => totalTurnCount; set => totalTurnCount = value; }
        public int FinalCharacterHp { get => finalCharacterHp; set => finalCharacterHp = value; }
        public int TotalCharacterHpLost { get => totalCharacterHpLost; set => totalCharacterHpLost = value; }
        public bool UsedHeal { get => usedHeal; set => usedHeal = value; }
        public bool IsCharacterDead { get => isCharacterDead; set => isCharacterDead = value; }
        public Character Character { get => character; set => character = value; }
        public Queue<Goon> GoonQueue { get => goonQueue; set => goonQueue = value; }
    }
}