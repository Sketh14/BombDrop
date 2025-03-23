namespace BombDrop.Utils
{
    public class HelperFunctions
    {
        public static string GenerateRandomHash6Bytes()
        {
            byte[] _randomBytes = new byte[6];
            // using (var rng = new RNGCryptoServiceProvider())            
            //     rng.GetBytes(bytes);

            System.Random randomByteGenerator = new System.Random(System.DateTime.Now.Day + System.DateTime.Now.Hour
                                                 + System.DateTime.Now.Minute + System.DateTime.Now.Second
                                                 + System.DateTime.Now.Millisecond);

            randomByteGenerator.NextBytes(_randomBytes);

            // and if you need it as a string...
            // _generatedHash = System.BitConverter.ToString(_randomBytes);
            return System.BitConverter.ToString(_randomBytes).Replace("-", "");

            // or maybe...
            // return System.BitConverter.ToString(_randomBytes).Replace("-", "").ToLower();
        }
    }
}
