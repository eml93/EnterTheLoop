namespace EnterTheLoop.Goons
{
    public class AssassinGoon : Goon
    {
        private readonly string NAME = "Assassin";
        private bool hasPerkBeenTriggered;
        public AssassinGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            PerkCondition = goonQueueCount => goonQueueCount == 1;
            whenDoesPerkTrigger = PerkTrigger.OnTurn;
            hasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new AssassinGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
             if (trigger.Equals(whenDoesPerkTrigger) && !hasPerkBeenTriggered) {
                hasPerkBeenTriggered = true;
            }
        }
    }
}