[System.Serializable]
public class DataPlayerExample
{
    public int Score  { get; set; }
    public string Name { get; set; }
        
    public DataPlayerExample(int score, string name)
    {
        Score = score;
        Name = name;
    }
}