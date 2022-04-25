namespace EnterTheLoop.Goons
{
    public class BHGoon : Goon
    {
        private readonly string NAME = "BH";

        public BHGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new BHGoon(startingDmg, startingHearts, perkDesc);
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