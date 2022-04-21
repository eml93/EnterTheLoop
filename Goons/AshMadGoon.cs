namespace EnterTheLoop.Goons
{
    public class AshMadGoon : Goon
    {
        private readonly string NAME = "AshMad";
        public AshMadGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new AshMadGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            return HasPerkBeenTriggered;
        }
    }
}