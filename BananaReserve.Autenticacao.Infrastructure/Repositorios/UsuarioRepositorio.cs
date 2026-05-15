using BananaReserve.Autenticacao.Domain.Entidades;
using BananaReserve.Autenticacao.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace BananaReserve.Autenticacao.Infrastructure.Repositorios;

public class UsuarioRepositorio(DefaultContext context) : IUsuarioRepositorio
{
    private readonly DefaultContext _context = context;

    public async Task<Usuario> CriarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return usuario;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await ObterPorIdAsync(id, cancellationToken);
        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
