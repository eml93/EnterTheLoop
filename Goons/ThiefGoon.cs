namespace EnterTheLoop.Goons
{
    public class ThiefGoon : Goon
    {
        private readonly string NAME = "Thief";

        public ThiefGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
            {
            Name = NAME;
            WhenDoesPerkTrigger = PerkTrigger.OnTurn;
            HasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new ThiefGoon(startingDmg, startingHearts, perkDesc);
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