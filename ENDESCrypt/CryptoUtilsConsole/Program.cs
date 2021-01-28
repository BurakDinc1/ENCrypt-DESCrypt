using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoUtilsConsole
{
    public enum CryptoOperation
    {
        ENCRYPT,
        DECRYPT
    };

    class DESDemos
    {

        public static byte[] DESCrypto(CryptoOperation cryptoOperation, DESCryptoServiceProvider des, byte[] message)
        {

            using (var memStream = new MemoryStream())
            {
                CryptoStream cryptoStream = null;

                if (cryptoOperation == CryptoOperation.ENCRYPT)
                    cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                else if (cryptoOperation == CryptoOperation.DECRYPT)
                    cryptoStream = new CryptoStream(memStream, des.CreateDecryptor(), CryptoStreamMode.Write);

                if (cryptoStream == null)
                    return null;

                cryptoStream.Write(message, 0, message.Length);
                cryptoStream.FlushFinalBlock();
                return memStream.ToArray();
            }

        }

        public static byte[] GenerateRandomByteArray(int size)
        {
            var random = new Random();
            byte[] byteArray = new byte[size];
            random.NextBytes(byteArray);
            return byteArray;
        }

        public static byte[] GenerateIv()
        {
            byte[] IV = GenerateRandomByteArray(8);
            return IV;
        }

        public static byte[] GenerateKey()
        {
            byte[] key = GenerateRandomByteArray(8);
            return key;
        }

        public static void LaunchDemo(string metin)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.GenerateIV();
                des.GenerateKey();
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                byte[] encrypted = DESCrypto(CryptoOperation.ENCRYPT, des, Encoding.UTF8.GetBytes(metin));
                Console.WriteLine(Environment.NewLine + "Encrypted Text :" + BitConverter.ToString(encrypted).Replace("-", ""));
                byte[] decrypted = DESCrypto(CryptoOperation.DECRYPT, des, encrypted);
                Console.WriteLine(Environment.NewLine + "Decrypted Text :" + Encoding.UTF8.GetString(decrypted));
            }
            Console.ReadLine();
        }    
    }

    class Program
    {
        static void Main(string[] args) // -- Ana program burası. Çalıştırdığında ilk buranın içi çalışır.-- 
        {
            Console.Write("Şifrelenecek Metni Giriniz: ");
            string metin = Console.ReadLine(); // (1.) kullanıcıdan şifrelemek için metin girmesi istenir                     

            DESDemos.LaunchDemo(metin); //(2.) AESDemos sınıfından LaunchDemo fonksiyonu çağrılır.

        }
    }
}