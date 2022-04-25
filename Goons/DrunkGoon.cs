namespace EnterTheLoop.Goons
{
    public class DrunkGoon : Goon
    {
        private readonly string NAME = "Drunk";
        public DrunkGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            WhenDoesPerkTrigger = PerkTrigger.OnDeath;
            HasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new DrunkGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            if (trigger.Equals(whenDoesPerkTrigger) && !HasPerkBeenTriggered) {
                HasPerkBeenTriggered = true;
            }

            return HasPerkBeenTriggered;
        }
    }
}