using System.Security.Cryptography;

namespace GameShopAPI.Helpers
{
    public class Encryptor
    {
        private const int Saltsize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public Encryptor() { }

        public string EncryptPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       Saltsize,
                       Iterations,
                       HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Iterations}.{salt}.{key}";
            }
        }

        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
                throw new FormatException("Unexpected hash format. Should be formatted as {iterations}.{salt}.{key}");

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needUpgrade = iterations != Iterations;

            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       Saltsize,
                       Iterations,
                       HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);
                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needUpgrade);
            }
        }
    }
}
