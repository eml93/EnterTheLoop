namespace EnterTheLoop
{
    public class AggregatedFightMetrics
    {
        private List<FightResults> fightResults = new List<FightResults>();
        private List<Fight> allFights;
        private float totalFights;
        private double successRate;
        private double avgTurnToComplete;
        private double avgDmgDealt;

        private int mostDmgDealt;
        private int mostTurnsTaken;
        private double healingRate;
        private String fightName;

        public int MostDmgDealt { get => mostDmgDealt; set => mostDmgDealt = value; }
        public int MostTurnsTaken { get => mostTurnsTaken; set => mostTurnsTaken = value; }
        public double AvgTurnToComplete { get => avgTurnToComplete; set => avgTurnToComplete = value; }
        public double AvgDmgDealt { get => avgDmgDealt; set => avgDmgDealt = value; }
        public List<Fight> AllFights { get => allFights; set => allFights = value; }
        public string FightName { get => fightName; set => fightName = value; }

        public AggregatedFightMetrics(List<FightResults> fightResults, List<Fight> allFights) {
            this.fightResults = fightResults;
            this.allFights = allFights;

            this.fightName = $"{fightResults[0].AllFightsString} ({fightResults[0].PlayerSide})";

            ComputeMetrics();
        }

        private void ComputeMetrics()
        {
            totalFights = fightResults.Count;
            successRate = fightResults.Average(fr => fr.IsCharacterDead ? 0 : 1) * 100;
            healingRate = fightResults.Average(fr => fr.UsedHeal ? 1 : 0) * 100;
            avgTurnToComplete = fightResults.Average(fr => fr.TotalTurnCount);
            avgDmgDealt = fightResults.Average(fr => fr.TotalCharacterHpLost);
            
            mostTurnsTaken = fightResults.MaxBy(fr => fr.TotalTurnCount).TotalTurnCount;
            mostDmgDealt = fightResults.MaxBy(fr => fr.TotalCharacterHpLost).TotalCharacterHpLost;
        }

        public override string ToString()
        {
            return $"After running {fightName} {totalFights} times, some metrics:\n---------\n"+
            $"Success Rate is {successRate}%\n" +
            $"Percent of Fights using Heal is {healingRate}%\n" +
            $"Average number of turns to complete is {avgTurnToComplete}\n" +
            $"Average HP lost by the player is {avgDmgDealt}\n---------\n" + 
            $"Most turns taken is {mostTurnsTaken}\n" + 
            $"Most HP lost by the player is {mostDmgDealt}\n";
        }
    }
}