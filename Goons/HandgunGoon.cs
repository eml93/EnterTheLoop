namespace EnterTheLoop.Goons
{
    public class HandgunGoon : Goon
    {
        private readonly string NAME = "Handgun";
        public HandgunGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new HandgunGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
            // Do Nothing
        }
    }
}