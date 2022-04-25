namespace EnterTheLoop.Goons
{
    public class CellGoon : Goon
    {
        private readonly string NAME = "Cell";

        public CellGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
          {
            Name = NAME;
            WhenDoesPerkTrigger = PerkTrigger.OnDeath;
            HasPerkBeenTriggered = false;
        }

        public override Goon Copy()
        {
            return new CellGoon(startingDmg, startingHearts, perkDesc);
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