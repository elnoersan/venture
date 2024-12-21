using System;

// An interface that defines the contract for file handling operations
public interface IFileHandler
{
    // Method to load game data from a file
    public GameData Load();

    // Method to save game data to a file
    public void Save(GameData data);

    // Method to encrypt or decrypt data using a simple XOR algorithm
    public String EncryptDecrypt(string data);
}