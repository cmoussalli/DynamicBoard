using System.Text;

namespace DynamicBoard.Application.DomainServices
{
    public class EncryptionHelper
    {
        private static readonly string encryptionKey = Storage.EncryptionKey;// "DynamicBoard_Feburary4th2024";
        public static string Encrypt(string plainText )
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] encryptedData = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                encryptedData[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            string encryptedText = BitConverter.ToString(encryptedData).Replace("-", "");
            return encryptedText;
        }
        public static string Decrypt(string encryptedText )
        {
            byte[] data = new byte[encryptedText.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(encryptedText.Substring(i * 2, 2), 16);
            }
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] decryptedData = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                decryptedData[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            string decryptedText = Encoding.UTF8.GetString(decryptedData);
            return decryptedText;
        }
    }
}

