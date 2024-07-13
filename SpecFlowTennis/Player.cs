namespace SpecFlowTennis
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; private set; }
        public int[] Sets { get; private set; }
        public int CurrentSet { get; set; }
        public bool Avantage { get; set; }

        public Player(string name)
        {
            Name = name;
            Score = 0;
            Sets = new int[3];
            CurrentSet = 0;
            Avantage = false;
        }

        public void ScorePoint(Player adversaire)
        {
            if (Score == 50)
            {
                Score += 10; // On passe à 60 synonyme de victoire du jeu
                Avantage = false;
                return;
            }
            if (Score == 40 && adversaire.Score == 40)
            {
                Avantage = true;
                Score = 50; // Représenter l'avantage avec 50
            }
            else if (Score == 30)
            {
                Score += 10;
            }
            else if (Score == 40 && adversaire.Score != 40)
            {
                // Gagner le jeu
            }
            else
            {
                Score += 15;
            }
        }

        public void ScoreTieBreak(Player adversaire)
        {
            Score += 1;
        }


        public void ResetScore()
        {
            Score = 0;
            Avantage = false;
        }

        public void GagnerJeu()
        {
            Sets[CurrentSet] += 1;
        }

        public void GagnerSet()
        {
            CurrentSet += 1;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void SetSets(int[] sets)
        {
            for (int i = 0; i < sets.Length; i++)
            {
                Sets[i] = sets[i];
            }
        }
        
        public bool IsAvantage()
        {
            return Score == 50;
        }
    }
}