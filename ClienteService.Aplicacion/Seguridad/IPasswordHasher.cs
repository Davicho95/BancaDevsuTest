namespace ClienteService.Aplicacion.Seguridad;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hashAlmacenado);
}
