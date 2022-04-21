namespace EnterTheLoop.Goons
{
    public class BruteGoon : Goon
    {
        private readonly string NAME = "Brute";
        public BruteGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            PerkCondition = goonQueueCount => goonQueueCount == 1;
        }

        public override Goon Copy()
        {
            return new BruteGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            if (trigger.Equals(PerkTrigger.OnTurn) && !HasPerkBeenTriggered) {
                Dmg *= 2;
                HasPerkBeenTriggered = true;
            }

            return HasPerkBeenTriggered;
        }
    }
}