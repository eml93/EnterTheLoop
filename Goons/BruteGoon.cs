namespace EnterTheLoop.Goons
{
    public class BruteGoon : Goon
    {
        private readonly string NAME = "Brute";
        private bool hasPerkBeenTriggered;
        public BruteGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            PerkCondition = goonQueueCount => goonQueueCount == 1;
            hasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new BruteGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
            if (trigger.Equals(PerkTrigger.OnTurn) && !hasPerkBeenTriggered) {
                Dmg *= 2;
                hasPerkBeenTriggered = true;
            }
        }
    }
}