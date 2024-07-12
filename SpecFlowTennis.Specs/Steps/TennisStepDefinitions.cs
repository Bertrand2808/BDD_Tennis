using Xunit;
namespace SpecFlowTennis.Specs.Steps;

[Binding]
public sealed class TennisStepDefinitions
{

    private readonly ScenarioContext _scenarioContext;
    private Tennis _tennis;

    public TennisStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    /*************************************************************************
     *                                                                       *
     *                       Given Steps                                     *
     *                                                                       *
     *************************************************************************/
    [Given("Le match commence entre (.*) et (.*)")]
    public void GivenLeMatchCommenceEntreEt(string joueurA, string joueurB)
    {
        _tennis = new Tennis(joueurA, joueurB);
    }
    
    
    [Given("Le set est (.*)")]
    public void GivenLeSetEst(string etatSet)
    {
        Assert.Equal(etatSet, _tennis.EtatDuSet.ToString());
    }
    
    [Given("Le jeu est (.*)")]
    public void GivenLeJeuEst(string etatJeu)
    {
        Assert.Equal(etatJeu, _tennis.EtatDuJeu.ToString());
    }
    
    [Given("Le score est de :")]
    public void GivenLeScoreEstDe(Table table)
    {
        var scores = new Dictionary<string, PlayerScore>();
        foreach (var row in table.Rows)
        {
            var player = row["Joueur"];
            var score = new PlayerScore
            {
                Score = int.Parse(row["Score"]),
                Sets = new int[]
                {
                    int.Parse(row["Set 1"]),
                    int.Parse(row["Set 2"]),
                    int.Parse(row["Set 3"]),
                }
            };
            scores.Add(player, score);
        }

        _tennis.SetScores(scores);
    }
    
    [Given("Le set en cours est le set (.*)")]
    public void GivenLeSetEnCoursEstLeSet(int set)
    {
        _tennis.PlayerA.CurrentSet = set - 1;
        _tennis.PlayerB.CurrentSet = set - 1;
    }
    
    /*************************************************************************
     *                                                                       *
     *                       When Steps                                     *
     *                                                                       *
     *************************************************************************/
    
    
    
    [When("Joueur (.*) marque un point")]
    public void WhenJoueurMarqueUnPoint(string joueur)
    {
        if (joueur == _tennis.PlayerA.Name)
        {
            _tennis.MarquerPoint(_tennis.PlayerA);
        }
        else
        {
            _tennis.MarquerPoint(_tennis.PlayerB);
        }
    }
    
    /*************************************************************************
     *                                                                       *
     *                       Then Steps                                     *
     *                                                                       *
     *************************************************************************/
    [Then("Le jeu devrait être (.*)")]
    public void ThenLeJeuEst(string etatJeu)
    {
        Assert.Equal(etatJeu, _tennis.EtatDuJeu.ToString());
    }
    
    [Then("Le match devrait être (.*)")]
    public void ThenLeMatchDoitEtre(string etatMatch)
    {
        Assert.Equal(etatMatch, _tennis.EtatDuMatch.ToString());
    }
    
    [Then("Le set devrait être (.*)")]
    public void ThenLeSetDoitEtre(string etatSet)
    {
        Assert.Equal(etatSet, _tennis.EtatDuSet.ToString());
    }
    
    [Then("le score devrait être :")]
    public void ThenLeScoreDevraitEtre(Table table)
    {
        var expectedScores = new Dictionary<String, PlayerScore>();
        foreach (var row in table.Rows)
        {
            var player = row["Joueur"];
            var exprectedPoints = new PlayerScore
            {
                Score = int.Parse(row["Score"]),
                Sets = new int[]
                {
                    int.Parse(row["Set 1"]),
                    int.Parse(row["Set 2"]),
                    int.Parse(row["Set 3"]),
                }
            };
            expectedScores.Add(player, exprectedPoints);
        }
        var actualScores = _tennis.GetScores();
        foreach (var player in expectedScores.Keys)
        {
            Assert.Equal(expectedScores[player].Score, actualScores[player].Score);
            Assert.Equal(expectedScores[player].Sets, actualScores[player].Sets);
        }
    }
    
    [Then(@"le resultat du jeu devrait être ""(.*)""")]
    public void ThenLeResultatDuJeuDevraitEtre(string resultatAttendu)
    {
        var resultatActuel = _tennis.GetResultatJeu();
        Assert.Equal(resultatAttendu, resultatActuel);
    }
    
    [Then(@"le resultat du set devrait être ""(.*)""")]
    public void ThenLeResultatDuSetDevraitEtre(string resultatAttendu)
    {
        var resultatActuel = _tennis.GetResultatSet();
        Assert.Equal(resultatAttendu, resultatActuel);
    }
    
    [Then(@"le resultat du match devrait être ""(.*)""")]
    public void ThenLeResultatDuMatchDevraitEtre(string resultatAttendu)
    {
        var resultatActuel = _tennis.GetResultatMatch();
        Assert.Equal(resultatAttendu, resultatActuel);
    }
    
    
    
}