using EnterTheLoop;
using Microsoft.VisualBasic.FileIO;

class Program {
    #pragma warning disable 8602, 8600

    private static Dictionary<String, Goon> goonMap = new Dictionary<string, Goon>();
    private static Dictionary<String, Fight> fightMap = new Dictionary<string, Fight>();
    private static List<Fight> fightLvl1Deck = new List<Fight>();
    private static List<AggregatedFightMetrics> allFightMetrics = new List<AggregatedFightMetrics>();
    private static Dictionary<int, List<AggregatedFightMetrics>> fightMetricsByDmg = new Dictionary<int, List<AggregatedFightMetrics>>(); 
    private static Dictionary<int, int> acceptableHpLossByFightLvl = new Dictionary<int, int>();
    private static int fightLvlTested;

    private static readonly string EnterTheLoopNamespace = "EnterTheLoop.";
    private static readonly string GoonNamespace = "Goons.";
    private static readonly string GoonPath = @"<EnterPath>\Goons.csv";
    private static readonly string FightLvl1Path = @"<EnterPath>\FightsLvl1.csv";
    private static readonly string FightLvl2Path = @"<EnterPath>\FightsLvl2.csv";
    private static readonly string FightLvl3Path = @"<EnterPath>\FightsLvl3.csv";
    
    public static void Main(String[] args) {
         if (args[0].Equals("help")) {
            Console.WriteLine("Welcome to the Fight Simulator! Use this program to test a character's chance of survival in various levels of Fights.");   
            Console.WriteLine("There are three Fight Levels (1, 2, and 3), each with increasing difficulty. Each Fight has one or two Goons within it.\n" + 
                                "Goons deal your character Damage and have their own amount of HP, respresented by Hearts.\n" +
                                "Your character automatically attacks the Goons in the Fight, roll 1d6. On a 4 or below, it deals Half a Heart of Damage. On 5 or 6, it deals a Full Heart." + 
                                "\n\nThis program is used to test the viability of a basic character against varying different Fight matchups." +
                                "\nFights are either 'singles' (meaning 1 Fight card is drawn) or 'doubles' (meaning 2 Fight Cards are drawn).");   
            Console.WriteLine("To get started, enter the code and replace the GoonPath and FightLvl1,2,3Paths with your own local path to the CSVs included with the program.");
            Console.WriteLine("\nRun using 'dotnet run'. Type 'singles' or 'doubles' to test single Fights or Fights in pairs; " +
                                "then type the amount of iterations you want to test (default is 50);\n" +
                                "then the Fight Lvl to load (1, 2, or 3). Keep in mind that each level is harder than the last.\n" +
                                "For example, 'dotnet run doubles 50 1' will run doubles of Fight Lvl 1 50 times. 'dotnet run singles 1000 2' will run singles of Fight Lvl 2 1000 times." +
                                "The aggregated metrics of all the Fights will be displayed at the bottom, along with how long the calcuations took.");       
            return;
        } 

        DateTime startTime = DateTime.Now;
        
        acceptableHpLossByFightLvl[1] = 5;
        acceptableHpLossByFightLvl[2] = 8;
        acceptableHpLossByFightLvl[3] = 11;

        bool doubles = args[0].Equals("doubles"); 
        int numOfInterations = Int32.Parse(args.Length > 1 ? args[1] : "50");
        fightLvlTested = Int32.Parse(args.Length > 2 ? args[2] : "1"); 
        String specificFight = args.Length > 3 ? args[3] : String.Empty; 

        readGoonsIntoGoonMap();
        if (fightLvlTested == 1) {
            readFightsIntoFightMap(FightLvl1Path);
        } else if (fightLvlTested == 2) {
            readFightsIntoFightMap(FightLvl2Path);
        } else if (fightLvlTested == 3) {
            readFightsIntoFightMap(FightLvl3Path);
        } else {
            readFightsIntoFightMap(FightLvl1Path);
        }

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
    }

    private static void TestFights(List<Fight> allFights, int numOfIterations)
    {
        List<FightResults> leftFightResults;
        List<FightResults> rightFightResults;

        for (int dmg = 1; dmg < 6; dmg++) {
            Character c = new Character(dmg);
            leftFightResults = new List<FightResults>();
            rightFightResults = new List<FightResults>();

            for (int i = 0; i < numOfIterations; i++) {
                leftFightResults.Add(FightController.StartFight(c, allFights, "Left"));
                rightFightResults.Add(FightController.StartFight(c, allFights, "Right"));
            }

            AggregatedFightMetrics leftFightMetrics = new AggregatedFightMetrics(leftFightResults, allFights);
            AggregatedFightMetrics rightFightMetrics = new AggregatedFightMetrics(rightFightResults, allFights);
            
            allFightMetrics.Add(leftFightMetrics);
            allFightMetrics.Add(rightFightMetrics);
        }
    }
    
    private static void AggregateTheAggregates()
    {
        AggregatedFightMetrics longestAvgFight = allFightMetrics.MaxBy(fm => fm.AvgTurnToComplete);
        AggregatedFightMetrics mostDmgingAvgFight = allFightMetrics.MaxBy(fm => fm.AvgDmgDealt);
        AggregatedFightMetrics mostUnbalancedFight = allFightMetrics.MaxBy(fm => fm.mostUnbalancedFight);

        List<AggregatedFightMetrics> acceptableFights = allFightMetrics.Where(fm => 
                                                            fm.AvgDmgDealt <= acceptableHpLossByFightLvl[fightLvlTested])
                                                        .ToList();
        List<AggregatedFightMetrics> unacceptableFights = allFightMetrics.Where(fm => 
                                                            fm.AvgDmgDealt > acceptableHpLossByFightLvl[fightLvlTested])
                                                        .ToList();

        Console.WriteLine($"The longest Fight to complete, on average, was {longestAvgFight.FightName}, taking {longestAvgFight.AvgTurnToComplete} turns");
        Console.WriteLine($"The most damaging Fight complete, on average, was {mostDmgingAvgFight.FightName}, taking {mostDmgingAvgFight.AvgDmgDealt} HP");
        Console.WriteLine($"The overall turn count was {allFightMetrics.Average(fm => (int) fm.AvgTurnToComplete)} turns");
        Console.WriteLine($"The overall dmg average was {allFightMetrics.Average(fm => fm.AvgDmgDealt)} HP");
        Console.WriteLine($"The most unbalanced Fight was {mostUnbalancedFight.FightName}, with the highest dmg dealt {mostUnbalancedFight.MostDmgDealt} and the lowest {mostUnbalancedFight.LeastDmgDealt}");
        Console.WriteLine($"-----------------------");
        Console.WriteLine($"The acceptable amount of HP lost per fight at this level is {acceptableHpLossByFightLvl[fightLvlTested]}.\nThere are {unacceptableFights.Count} unacceptable fights [{(String.Join(", ", unacceptableFights.Select(fm => fm.FightName + " " + fm.AvgDmgDealt)))}]");
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
        using(TextFieldParser csvParser = new TextFieldParser(GoonPath)) {
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

    
    private static void readFightsIntoFightMap(string fightPath)
    {
        using(TextFieldParser csvParser = new TextFieldParser(fightPath)) {
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