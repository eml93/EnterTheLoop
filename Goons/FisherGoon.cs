namespace EnterTheLoop.Goons
{
    public class FisherGoon : Goon
    {
        private readonly string NAME = "Fisher";
        public FisherGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new FisherGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            return HasPerkBeenTriggered;
        }
    }
}