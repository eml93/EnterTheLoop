namespace EnterTheLoop
{
    public abstract class Goon
    {
        private String name;
        private int dmg;
        protected int startingDmg;
        private float hearts; 
        protected float startingHearts;
        protected String perkDesc; 
        private bool isDead;
        protected PerkTrigger whenDoesPerkTrigger;
        private Func<int, bool> perkCondition;
        private bool hasPerkBeenTriggered;
        public string Name { get => name; set => name = value; }
        public int Dmg { get => dmg; set => dmg = value; }
        public float Hearts { get => hearts; set => hearts = value; }
        public string PerkDesc { get => perkDesc; set => perkDesc = value; }
        public PerkTrigger WhenDoesPerkTrigger { get => whenDoesPerkTrigger; set => whenDoesPerkTrigger = value; }
        public Func<int, bool> PerkCondition { get => perkCondition; set => perkCondition = value; }
        public bool HasPerkBeenTriggered { get => hasPerkBeenTriggered; set => hasPerkBeenTriggered = value; }

        public Goon(int dmg, float hearts, string perkDesc)
        {
            this.dmg = dmg;
            this.startingDmg = Dmg;
            this.hearts = hearts;
            this.startingHearts = Hearts;
            this.perkDesc = perkDesc;
            this.isDead = false;
            this.hasPerkBeenTriggered = false;

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

            TriggerPerk(PerkTrigger.OnDamage);

            return isDead;
        }

        public abstract bool TriggerPerk(PerkTrigger trigger);

        internal void Reset() {
            hearts = startingHearts;
            dmg = startingDmg;
            isDead = false;
        }

        public abstract Goon Copy();
    }

    public enum PerkTrigger {
        OnDamage,
        AtStart,
        OnTurn
    }
}