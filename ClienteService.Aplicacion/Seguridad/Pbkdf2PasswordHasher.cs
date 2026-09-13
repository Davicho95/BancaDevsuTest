using System.Security.Cryptography;

namespace ClienteService.Aplicacion.Seguridad;

// PBKDF2 con lo que trae el BCL, sin meter una libreria de terceros solo para esto.
// Formato guardado: "{iteraciones}.{saltBase64}.{hashBase64}", para poder subir el costo
// mas adelante sin invalidar los hashes que ya existen.
public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int Iteraciones = 100_000;
    private const int TamanoSalt = 16;
    private const int TamanoHash = 32;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(TamanoSalt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);
        return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hashAlmacenado)
    {
        var partes = hashAlmacenado.Split('.', 3);
        if (partes.Length != 3 || !int.TryParse(partes[0], out var iteraciones))
            return false;

        var salt = Convert.FromBase64String(partes[1]);
        var hashEsperado = Convert.FromBase64String(partes[2]);
        var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(password, salt, iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

        return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
    }
}
