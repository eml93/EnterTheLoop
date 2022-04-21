namespace EnterTheLoop.Goons
{
    public class CrierGoon : Goon
    {
        private readonly string NAME = "Crier";
        public CrierGoon(int dmg, float hearts, string perkDesc) : base(dmg, hearts, perkDesc)
         {
            Name = NAME;
        }

        public override Goon Copy()
        {
            return new CrierGoon(startingDmg, startingHearts, perkDesc);
        }

        public override bool TriggerPerk(PerkTrigger trigger)
        {
            return HasPerkBeenTriggered;
        }
    }
}