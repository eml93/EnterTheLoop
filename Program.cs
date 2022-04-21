using EnterTheLoop;
using Microsoft.VisualBasic.FileIO;

class Program {

    private static Dictionary<String, Goon> goonMap = new Dictionary<string, Goon>();
    private static Dictionary<String, Fight> fightMap = new Dictionary<string, Fight>();
    private static List<Fight> fightLvl1Deck = new List<Fight>();
    private static List<AggregatedFightMetrics> allFightMetrics = new List<AggregatedFightMetrics>();

    private static Character c = new Character();
    private static readonly string EnterTheLoopNamespace = "EnterTheLoop.";
    private static readonly string GoonNamespace = "Goons.";
    
    public static void Main(String[] args) {
        DateTime startTime = DateTime.Now;
        readGoonsIntoGoonMap();
        readFightsIntoFightMap();
        createFightDeck();
        // shuffleFightDeck();

        bool doubles = args[0].Equals("doubles"); 
        int numOfInterations = Int32.Parse(args.Length > 1 ? args[1] : "50");
        String specificFight = args.Length > 2 ? args[2] : String.Empty; 
        
        if (doubles) {
            if (!String.IsNullOrEmpty(specificFight)) {
                String[] fights = specificFight.Split(",");
                TestFights(new List<Fight>() {fightMap[fights[0]], fightMap[fights[1]]}, numOfInterations);
            } else {
                foreach (Fight f1 in fightMap.Values) {
                    foreach (Fight f2 in fightMap.Values) {
                        if (f1.GetName().Equals(f2.GetName()) && f1.CopiesInDeck == 1) {
                            continue;
                        }
                        TestFights(new List<Fight>() {f1, f2}, numOfInterations);
                    }
                }
            } 
        } else {
            if (!String.IsNullOrEmpty(specificFight)) {
                TestFights(new List<Fight>() {fightMap[specificFight]}, numOfInterations);
            } else {
                foreach (Fight f in fightMap.Values) {
                    TestFights(new List<Fight>() {f}, numOfInterations);
                }
            }
        }

        AggregateTheAggregates(); 

        Console.WriteLine();           
        Console.WriteLine($"Total calculations took {(DateTime.Now - startTime).TotalSeconds} seconds");           

        // //debug output
        // foreach(Goon g in goonMap.Values) {
        //     Console.WriteLine(g);
        // }

        // foreach(Fight f in fightMap.Values) {
        //     Console.WriteLine(f);
        // }

        // Console.WriteLine("Deck size -- " + fightLvl1Deck.Count);
        // Console.WriteLine("Top Card is " + fightLvl1Deck[0]);


    }

    private static void TestFights(List<Fight> allFights, int numOfIterations)
    {
        List<FightResults> leftFightResults = new List<FightResults>();
        List<FightResults> rightFightResults = new List<FightResults>();

        for (int i = 0; i < numOfIterations; i++) {
            leftFightResults.Add(FightController.StartFight(c, allFights, "Left"));
            rightFightResults.Add(FightController.StartFight(c, allFights, "Right"));
        }

        AggregatedFightMetrics leftFightMetrics = new AggregatedFightMetrics(leftFightResults, allFights);
        AggregatedFightMetrics rightFightMetrics = new AggregatedFightMetrics(rightFightResults, allFights);
        allFightMetrics.Add(leftFightMetrics);
        allFightMetrics.Add(rightFightMetrics);

        Console.WriteLine(leftFightMetrics);
        Console.WriteLine(rightFightMetrics);
    }
    
    private static void AggregateTheAggregates()
    {
        AggregatedFightMetrics longestAvgFight = allFightMetrics.MaxBy(fm => fm.AvgTurnToComplete);
        AggregatedFightMetrics mostDmgingAvgFight = allFightMetrics.MaxBy(fm => fm.AvgDmgDealt);

        List<AggregatedFightMetrics> acceptableFights = allFightMetrics.Where(fm => fm.AvgDmgDealt <= 5).ToList();
        List<AggregatedFightMetrics> unacceptableFights = allFightMetrics.Where(fm => fm.AvgDmgDealt > 5).ToList();

        Console.WriteLine($"The longest Fight to complete, on average, was {longestAvgFight.FightName}, taking {longestAvgFight.AvgTurnToComplete} turns");
        Console.WriteLine($"The most damaging Fight complete, on average, was {mostDmgingAvgFight.FightName}, taking {mostDmgingAvgFight.AvgDmgDealt} HP");
        Console.WriteLine($"The overall turn count was {allFightMetrics.Average(fm => (int) fm.AvgTurnToComplete)} turns");
        Console.WriteLine($"The overall dmg average was {allFightMetrics.Average(fm => fm.AvgDmgDealt)} HP");
        Console.WriteLine($"There are {unacceptableFights.Count} unacceptable fights [{(String.Join(", ", unacceptableFights.Select(fm => fm.FightName + " " + fm.AvgDmgDealt)))}]");
        Console.WriteLine($"This makes the percentage of acceptable fights {((double)acceptableFights.Count / (double)allFightMetrics.Count) * 100}%, {acceptableFights.Count} / {allFightMetrics.Count}");
    }

    private static void createFightDeck()
    {
        foreach (Fight fight in fightMap.Values)
        {
            for (int i = 0; i < fight.CopiesInDeck; i++)
            {
                fightLvl1Deck.Add(fight.copy());
            }
        }
    }

    private static void readGoonsIntoGoonMap()
    {
        var path = @"C:\Users\capho\Documents\Coding\EnterTheLoop\Data\Goons.csv";
        using(TextFieldParser csvParser = new TextFieldParser(path)) {
            csvParser.SetDelimiters(new string[] {","});

            // skips first row
            csvParser.ReadLine();

            while(!csvParser.EndOfData) {
                string[] fields = csvParser.ReadFields();

                string name = fields[1];
                int dmg = Int32.Parse(fields[2]);
                float hearts = float.Parse(fields[3]);
                string perkDesc = fields[4];

                Type t = Type.GetType(EnterTheLoopNamespace + GoonNamespace + name + "Goon");
                if (t == null) {
                    throw new Exception($"Goon {name}Goon not found!");
                }
                Goon goon = (Goon) Activator.CreateInstance(t, dmg, hearts, perkDesc);

                // Goon goon = new Goon(fields[1], fields[2], fields[3], fields[4]);

                goonMap.Add(goon.Name, goon);
            }
        }
    }

    
    private static void readFightsIntoFightMap()
    {
        var path = @"C:\Users\capho\Documents\Coding\EnterTheLoop\Data\FightsLvl1.csv";
        using(TextFieldParser csvParser = new TextFieldParser(path)) {
            csvParser.SetDelimiters(new string[] {","});

            // skips first row
            csvParser.ReadLine();

            while(!csvParser.EndOfData) {
                string[] fields = csvParser.ReadFields();

                List<Goon> goonsInFight = new List<Goon>();
                for(int i = 3; i <= 5; i++) {
                    if (goonMap.ContainsKey(fields[i])) {
                        goonsInFight.Add(goonMap[fields[i]]);
                    }
                }

                Fight fight = new Fight(fields[0], fields[1], fields[2], goonsInFight);

                fightMap.Add($"{fight.Lvl}-{fight.Number}", fight);
            }
        }
    }

        private static void shuffleFightDeck()
    {
        fightLvl1Deck = fightLvl1Deck.OrderBy(i => Guid.NewGuid()).ToList();
    }
}