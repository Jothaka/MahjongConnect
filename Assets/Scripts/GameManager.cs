public class GameManager 
{
    public int LevelID;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
    private static GameManager instance;

    private GameManager()
    { }
}