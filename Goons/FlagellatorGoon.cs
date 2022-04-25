namespace EnterTheLoop.Goons
{
    public class FlagellatorGoon : Goon
    {
        private readonly string NAME = "Flagellator";
        public FlagellatorGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            WhenDoesPerkTrigger = PerkTrigger.OnTurn;
            HasPerkBeenTriggered = true;
        }

        public override Goon Copy()
        {
            return new FlagellatorGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            // TODO need a better system for perks -- this needs to tell the FightController to skip it!
            if (trigger.Equals(whenDoesPerkTrigger) && !HasPerkBeenTriggered) {
                HasPerkBeenTriggered = true;
            } else if (trigger.Equals(whenDoesPerkTrigger) && HasPerkBeenTriggered) {
                HasPerkBeenTriggered = false;
            }

            return HasPerkBeenTriggered;
        }
    }
}