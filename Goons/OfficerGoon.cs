namespace EnterTheLoop.Goons
{
    public class OfficerGoon : Goon
    {
        private readonly string NAME = "Officer";
        private bool hasPerkBeenTriggered;
        public OfficerGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            whenDoesPerkTrigger = PerkTrigger.AtStart;
            hasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new OfficerGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
             if (trigger.Equals(whenDoesPerkTrigger) && !hasPerkBeenTriggered) {
                hasPerkBeenTriggered = true;
            }
        }
    }
}