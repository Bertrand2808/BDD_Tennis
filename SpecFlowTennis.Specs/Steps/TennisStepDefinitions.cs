using Xunit;
namespace SpecFlowTennis.Specs.Steps;

[Binding]
public sealed class TennisStepDefinitions
{

    private readonly ScenarioContext _scenarioContext;
    private Tennis _tennis;
    private Tennis.EtatMatch _etatMatch;
    

    public TennisStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [Given("Le match commence entre (.*) et (.*)")]
    public void GivenLeMatchCommenceEntre(String joueurA, String joueurB)
    {
        _tennis = new Tennis(joueurA, joueurB);
        _tennis.CommencerMatch();
    }
    
    [Given("Le set commence")]
    public void GivenLeSetCommence()
    {
        _tennis.CommencerSet();
    }
    
    [Given("Le jeu commence")]
    public void GivenLeJeuCommence()
    {
        _tennis.CommencerJeu();
    }
    
    [Given("L'échange commence")]
    public void GivenLEchangeCommence()
    {
        _tennis.CommencerEchange();
    }
    
    [Given("Le joueur (.*) marque un point")]
    public void GivenLeJoueurAMarqueUnPoint(String joueur)
    {
        _tennis.marquerPoint(joueur, 15);
    }
    
    [When("L'échange devrait être terminé")]
    public void WhenLEchangeDevraitEtreTermine()
    {
        _tennis.ChangeEtatMatch(Tennis.EtatMatch.EchangeTermine);
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
        var actualScores = _tennis.GetScore();
        foreach (var player in expectedScores.Keys)
        {
            Assert.Equal(expectedScores[player].Score, actualScores[player].Score);
            Assert.Equal(expectedScores[player].Sets, actualScores[player].Sets);
        }
    }
    
    [Then("Le jeu devrait être \"(.*)\"")]
    public void ThenLeJeuDevraitEtre(String etat)
    {
        var expectedState = (Tennis.EtatMatch) Enum.Parse(typeof(Tennis.EtatMatch), etat);
        Assert.Equal(expectedState, _tennis.GetEtatMatch());
    }

    
}