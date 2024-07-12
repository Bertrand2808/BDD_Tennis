namespace SpecFlowTennis;

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
    
    public void ScorePoint()
    {
        if (Avantage)
        {
            
        }
        if (Score == 30)
        {
            Score += 10;
        }
        else if (Score == 40)
        {
            if (Score == 40)
            {
                Avantage = true;
            }
        }
        else
        {
            Score += 15;
        }
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
    
}