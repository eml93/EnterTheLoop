namespace EnterTheLoop.Goons
{
    public class GrowlerGoon : Goon
    {
        private readonly string NAME = "Growler";
        public GrowlerGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
        {
            Name = NAME;
            whenDoesPerkTrigger = PerkTrigger.OnDamage;
        }

        public override Goon Copy()
        {
            return new GrowlerGoon(startingDmg, startingHearts, perkDesc);
        }

        protected override void TriggerPerk(PerkTrigger trigger)
        {
            if (trigger.Equals(whenDoesPerkTrigger)) {
                Dmg += 1;
            }
        }
    }
}