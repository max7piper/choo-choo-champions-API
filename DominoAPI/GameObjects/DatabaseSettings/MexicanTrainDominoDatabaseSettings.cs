namespace DominoAPI.GameObjects
{
    public class MexicanTrainDominoDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DominoCollectionName { get; set; } = null!;
    }
}