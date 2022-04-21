namespace EnterTheLoop.Goons
{
    public class DefenderGoon : Goon
    {
        private readonly string NAME = "Defender";

        public DefenderGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            whenDoesPerkTrigger = PerkTrigger.AtStart;
        }

        public override Goon Copy()
        {
            return new DefenderGoon(startingDmg, startingHearts, perkDesc);
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