namespace EnterTheLoop
{
    public class Goon
    {
        private String name;
        private int dmg;
        private int startingDmg;
        private float hearts; 
        private float startingHearts;
        private String perkDesc; 
        private bool isDead;
        public string Name { get => name; set => name = value; }
        public int Dmg { get => dmg; set => dmg = value; }
        public float Hearts { get => hearts; set => hearts = value; }
        public string PerkDesc { get => perkDesc; set => perkDesc = value; }

        public Goon(string name, string dmg, string hearts, string perkDesc)
        {
            this.Name = name;
            this.Dmg = Int32.Parse(dmg);
            this.startingDmg = Dmg;
            this.Hearts = float.Parse(hearts);
            this.startingHearts = Hearts;
            this.PerkDesc = perkDesc;
            this.isDead = false;
        }

        public Goon(string name, int dmg, float hearts, string perkDesc)
        {
            this.name = name;
            this.dmg = dmg;
            this.hearts = hearts;
            this.perkDesc = perkDesc;
        }

        public override string ToString()
        {
            return $"{name} -- DMG {dmg} -- Hearts {hearts} -- Perk {perkDesc}";
        }

        internal bool TakeDamage(float totalAttackInHearts)
        {
            hearts -= totalAttackInHearts;

            if (hearts <= 0) {
                isDead = true;
            }

            TriggerPerk();

            return isDead;
        }

        // should replace this by making Goon abstract, but thats for another time
        private void TriggerPerk()
        {
            switch(name) {
                case "Growler":
                    dmg+=1;
                    break;
                case "Bruiser":
                    dmg*=2;
                    break;
            }
        }

        internal void Reset() {
            hearts = startingHearts;
            dmg = startingDmg;
            isDead = false;
        }

        internal Goon Copy() {
            return new Goon(name, dmg, hearts, perkDesc);
        }
    }

    public enum PerkTrigger {
        OnDamage,
        AtStart,
        OnTurn
    }
}