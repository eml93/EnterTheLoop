namespace EnterTheLoop.Goons
{
    public class MeatHookGoon : Goon
    {
        private readonly string NAME = "MeatHook";
        public MeatHookGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new MeatHookGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
            // Do Nothing
        }
    }
}