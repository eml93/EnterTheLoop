namespace EnterTheLoop.Goons
{
    public class DefenderGoon : Goon
    {
        private readonly string NAME = "Defender";
        private bool hasPerkBeenTriggered;

        public DefenderGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            whenDoesPerkTrigger = PerkTrigger.AtStart;
            hasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new DefenderGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
             if (trigger.Equals(whenDoesPerkTrigger) && !hasPerkBeenTriggered) {
                hasPerkBeenTriggered = true;
            }
        }
    }
}