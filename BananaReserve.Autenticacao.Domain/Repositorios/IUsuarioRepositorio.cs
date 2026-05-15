using BananaReserve.Autenticacao.Domain.Entidades;

namespace BananaReserve.Autenticacao.Domain.Repositorios;

public interface IUsuarioRepositorio
{
    Task<Usuario> CriarAsync(Usuario usuario, CancellationToken cancellationToken = default);

    Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default);
}
