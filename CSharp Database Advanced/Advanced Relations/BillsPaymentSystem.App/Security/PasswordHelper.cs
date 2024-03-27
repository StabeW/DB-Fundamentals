using System.Security.Cryptography;

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Create the Rfc2898DeriveBytes object and get the hash value
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        // Combine the salt and password bytes for storage
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Convert the combined salt+hash bytes to Base64 for storage
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string hashedPassword, string userEnteredPassword)
    {
        // Convert the Base64 string back to bytes
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the stored hashBytes
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Compute the hash of the entered password
        var pbkdf2 = new Rfc2898DeriveBytes(userEnteredPassword, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        // Compare the computed hash with the stored hash
        for (int i = 0; i < 15; i++)
        {
            if (hashBytes[i + 10] != hash[i])
                return false;
        }
        return true;
    }
}
