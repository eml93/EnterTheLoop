namespace EnterTheLoop
{
    public class Character
    {
        private int hp;
        private bool hasHealing;
        private Gun gun;
        private int hitThreshold;
        private bool isDead;
        private int startingDmg;

        public Character(int dmg) {
            hp = 15;
            hasHealing = true;
            startingDmg = dmg;
            gun = new Gun("Rusty Shotgun", dmg, false);
            hitThreshold = 5;
            IsDead = false;
        }

        public void Reset() {
            hp = 15;
            hasHealing = true;
            gun = new Gun("Rusty Shotgun", startingDmg, false);
            hitThreshold = 5;
            IsDead = false;
        }

        public int[] RollGun() {
            return gun.rollAttack(hitThreshold);
        }

        public int Hp { get => hp; set => hp = value; }
        public bool HasHealing { get => hasHealing; set => hasHealing = value; }
        public Gun Gun { get => gun; set => gun = value; }
        public bool IsDead { get => isDead; set => isDead = value; }

        internal void UseCrimsonAsh()
        {
            hp = 15;
            hasHealing = false;
        }

        internal int TakeDamage(int goonDmg)
        {
            hp -= goonDmg;
            
            if (hp <= 0) {
                IsDead = true;
            }

            // for right now, just return the goonDmg dealt, but this
            // num can be modified with damage reduction and the like
            return goonDmg; 
        }
    }
}