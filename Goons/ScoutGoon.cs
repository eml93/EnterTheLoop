namespace EnterTheLoop.Goons
{

    public class ScoutGoon : Goon
    {
        private readonly string NAME = "Scout";
        public ScoutGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new ScoutGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            return HasPerkBeenTriggered;
        }
    }
}