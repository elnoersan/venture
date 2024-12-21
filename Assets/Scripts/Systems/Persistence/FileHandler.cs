using UnityEngine;
using System;
using System.IO;

// A file handler class that implements the IFileHandler interface
public class FileHandler : IFileHandler
{
    private string path = ""; // The directory path where the file will be saved
    private string filename = ""; // The name of the file
    private string encryptionSeed = "waldheim"; // The seed used for simple XOR encryption

    // Constructor to initialize the file handler with a path and filename
    public FileHandler(string path, string filename)
    {
        this.path = path;
        this.filename = filename;
    }

    // Load game data from the file
    public GameData Load()
    {
        string fullPath = Path.Combine(path, filename); // Combine the path and filename to get the full file path

        GameData loadedData = null; // Variable to store the loaded game data

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = ""; // Variable to store the raw data read from the file

                // Open the file and read its contents
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd(); // Read the entire file
                    }
                }

                // Decrypt the loaded data
                dataToLoad = EncryptDecrypt(dataToLoad);

                // Deserialize the JSON data into a GameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                // Log an error if something goes wrong during loading
                Debug.Log("Error when trying to load data from file: " + fullPath + "\n " + e);
            }
        }

        return loadedData; // Return the loaded game data
    }

    // Save game data to the file
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(path, filename); // Combine the path and filename to get the full file path

        try
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the GameData object to JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Encrypt the JSON data
            dataToStore = EncryptDecrypt(dataToStore);

            // Write the encrypted data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            // Log an error if something goes wrong during saving
            Debug.Log("Error when trying to save data to file: " + fullPath + "\n " + e);
        }
    }

    // Simple XOR encryption/decryption method
    public String EncryptDecrypt(string data)
    {
        string modifiedData = ""; // Variable to store the encrypted/decrypted data

        // Iterate through each character in the data
        for (int i = 0; i < data.Length; i++)
        {
            // XOR the character with the corresponding character in the encryption seed
            modifiedData += (char)(data[i] ^ encryptionSeed[i % encryptionSeed.Length]);
        }

        return modifiedData; // Return the modified (encrypted/decrypted) data
    }
}