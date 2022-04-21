namespace EnterTheLoop.Goons
{
    public class AssassinGoon : Goon
    {
        private readonly string NAME = "Assassin";
        public AssassinGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            PerkCondition = goonQueueCount => goonQueueCount == 1;
            whenDoesPerkTrigger = PerkTrigger.OnTurn;
        }

        public override Goon Copy()
        {
            return new AssassinGoon(startingDmg, startingHearts, perkDesc);
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