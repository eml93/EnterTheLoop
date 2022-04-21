using System.Linq;

namespace EnterTheLoop
{
    public class Fight
    {
        private int lvl;
        private int number;
        private int copiesInDeck;
        private List<Goon> goons;
  
        public int Lvl { get => lvl; set => lvl = value; }
        public int Number { get => number; set => number = value; }
        public List<Goon> Goons { get => goons; set => goons = value; }
        public int CopiesInDeck { get => copiesInDeck; set => copiesInDeck = value; }

        public Fight(String lvl, String number, String copiesInDeck, List<Goon> goons)
        {
            this.lvl = Int32.Parse(lvl);
            this.number = Int32.Parse(number);
            this.CopiesInDeck = Int32.Parse(copiesInDeck);
            this.goons = goons;
        }

        public Fight(int lvl, int number, int copiesInDeck, List<Goon> goons)
        {
            this.lvl = lvl;
            this.number = number;
            this.copiesInDeck = copiesInDeck;
            this.goons = goons;
        }

        public override string ToString()
        {
            return $"Lvl {lvl} -- Number {number} -- Copies in Deck {copiesInDeck} -- Goons {goons.Select(g => g.Name).Aggregate((x, y) => x + ", " + y)}";
        }

        public string GetName() {
            return $"{lvl}-{number}";
        }

        public Fight copy() {
            return new Fight(lvl, number, copiesInDeck, goons);
        }

        public void Reset() {
            foreach (Goon g in goons)
            {
                g.Reset();
            }
        }

    }
}