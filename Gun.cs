namespace EnterTheLoop
{
    public class Gun
    {
        private String name;
        private int dieRolled;
        private bool oneHanded;

        public Gun(string name, int dieRolled, bool oneHanded)
        {
            this.name = name;
            this.dieRolled = dieRolled;
            this.oneHanded = oneHanded;
        }

        internal int[] rollAttack(int hitThreshold)
        {
            int[] directAndPartialHits = new int[] {0, 0};

            Random r = new Random();
            for (int i = 0; i < dieRolled; i++) {
                int rand = r.Next(1, 7);
                // Console.WriteLine($"Character rolled a {rand}, hitThreshold is {hitThreshold}" );
                if (rand >= hitThreshold) {
                    directAndPartialHits[0]+=1;
                } else {
                    directAndPartialHits[1]+=1;
                }
            }

            return directAndPartialHits;
        }
    }
}