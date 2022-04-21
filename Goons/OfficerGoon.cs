namespace EnterTheLoop.Goons
{
    public class OfficerGoon : Goon
    {
        private readonly string NAME = "Officer";
        public OfficerGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            whenDoesPerkTrigger = PerkTrigger.AtStart;
        }

        public override Goon Copy()
        {
            return new OfficerGoon(startingDmg, startingHearts, perkDesc);
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