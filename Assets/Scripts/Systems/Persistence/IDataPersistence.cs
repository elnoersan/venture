// An interface that defines the contract for data persistence in the game
public interface IDataPersistence
{
    // Method to load data into the implementing class
    void LoadData(GameData data);

    // Method to save data from the implementing class
    void SaveData(GameData data);
}