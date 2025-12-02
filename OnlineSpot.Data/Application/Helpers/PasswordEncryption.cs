using System.Security.Cryptography;
using System.Text;

namespace OnlineSpot.Data.Application.Helpers
{
    public class PasswordEncryption
    {
        public static string ComputeSha256Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    // 2 dígitos hexadecimales
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
