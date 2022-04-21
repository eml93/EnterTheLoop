using EnterTheLoop;
using Microsoft.VisualBasic.FileIO;

class Program {

    private static Dictionary<String, Goon> goonMap = new Dictionary<string, Goon>();
    private static Dictionary<String, Fight> fightMap = new Dictionary<string, Fight>();
    private static List<Fight> fightLvl1Deck = new List<Fight>();

    private static Character c = new Character();
    
    public static void Main(String[] args) {
        readGoonsIntoGoonMap();
        readFightsIntoFightMap();
        createFightDeck();
        // shuffleFightDeck();

        String numOfInterations = args.Length == 0 ? "50" : args[0];

        foreach (Fight f in fightMap.Values) {
            SingleFight(f, Int32.Parse(numOfInterations));
        }            

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

    private static void SingleFight(Fight fight, int numOfIterations)
    {
        List<FightResults> allFightResults = new List<FightResults>();

        for (int i = 0; i < numOfIterations; i++) {
            allFightResults.Add(FightController.IndividualFight(c, fight));
        }

        AggregatedFightMetrics fightMetrics = new AggregatedFightMetrics(allFightResults, fight);

        Console.WriteLine(fightMetrics);
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
        var path = @"C:\Users\capho\Downloads\Goons.csv";
        using(TextFieldParser csvParser = new TextFieldParser(path)) {
            csvParser.SetDelimiters(new string[] {","});

            // skips first row
            csvParser.ReadLine();

            while(!csvParser.EndOfData) {
                string[] fields = csvParser.ReadFields();

                Goon goon = new Goon(fields[1], fields[2], fields[3], fields[4]);

                goonMap.Add(goon.Name, goon);
            }
        }
    }

    
    private static void readFightsIntoFightMap()
    {
         var path = @"C:\Users\capho\Downloads\Fights.csv";
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

                fightMap.Add($"{fight.Lvl} -- {fight.Number}", fight);
            }
        }
    }

        private static void shuffleFightDeck()
    {
        fightLvl1Deck = fightLvl1Deck.OrderBy(i => Guid.NewGuid()).ToList();
    }

}