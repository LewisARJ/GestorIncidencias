using System.Text.RegularExpressions;
using GestorIncidencias.Application.Contract;
using GestorIncidencias.Application.Core;
using GestorIncidencias.Application.Dtos;
using GestorIncidencias.Domain.Entities;
using GestorIncidencias.Infrastructure.Interfaces;

namespace GestorIncidencias.Application.Services;

public class UsuarioService : IUsuarioService
{
    private static readonly Regex CorreoRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private readonly IUsuarioRepository _usuarios;
    private readonly IIncidenciaRepository _incidencias;

    public UsuarioService(IUsuarioRepository usuarios, IIncidenciaRepository incidencias)
    {
        _usuarios = usuarios;
        _incidencias = incidencias;
    }

    public ServiceResult<IEnumerable<UsuarioDto>> GetAllUsuarios()
    {
        var lista = _usuarios.GetAll().Select(ToDto).ToList();
        return ServiceResult<IEnumerable<UsuarioDto>>.SuccessResponse(lista);
    }

    public ServiceResult<UsuarioDto> GetUsuarioById(int id)
    {
        var usuario = _usuarios.GetById(id);
        if (usuario == null)
            return ServiceResult<UsuarioDto>.ErrorResponse("El usuario no existe.", 404);
        return ServiceResult<UsuarioDto>.SuccessResponse(ToDto(usuario));
    }

    public ServiceResult<UsuarioDto> CreateUsuario(CreateUsuarioDto dto)
    {
        var error = Validar(dto);
        if (error != null)
            return ServiceResult<UsuarioDto>.ErrorResponse(error);

        if (_usuarios.GetByCorreo(dto.Correo.Trim()) != null)
            return ServiceResult<UsuarioDto>.ErrorResponse("Ya existe un usuario con ese correo.", 409);

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Departamento = dto.Departamento.Trim(),
            Correo = dto.Correo.Trim()
        };
        _usuarios.Add(usuario);
        return ServiceResult<UsuarioDto>.SuccessResponse(ToDto(usuario), "Usuario creado correctamente.", 201);
    }

    public ServiceResult<UsuarioDto> UpdateUsuario(int id, CreateUsuarioDto dto)
    {
        var usuario = _usuarios.GetById(id);
        if (usuario == null)
            return ServiceResult<UsuarioDto>.ErrorResponse("El usuario no existe.", 404);

        var error = Validar(dto);
        if (error != null)
            return ServiceResult<UsuarioDto>.ErrorResponse(error);

        var otro = _usuarios.GetByCorreo(dto.Correo.Trim());
        if (otro != null && otro.Id != id)
            return ServiceResult<UsuarioDto>.ErrorResponse("Ya existe un usuario con ese correo.", 409);

        usuario.Nombre = dto.Nombre.Trim();
        usuario.Departamento = dto.Departamento.Trim();
        usuario.Correo = dto.Correo.Trim();
        _usuarios.Update(usuario);
        return ServiceResult<UsuarioDto>.SuccessResponse(ToDto(usuario), "Usuario actualizado correctamente.");
    }

    public ServiceResult<bool> DeleteUsuario(int id)
    {
        var usuario = _usuarios.GetById(id);
        if (usuario == null)
            return ServiceResult<bool>.ErrorResponse("El usuario no existe.", 404);

        if (_incidencias.GetAll().Any(i => i.UsuarioId == id))
            return ServiceResult<bool>.ErrorResponse("El usuario tiene incidencias registradas y no se puede eliminar.", 409);

        _usuarios.Remove(usuario);
        return ServiceResult<bool>.SuccessResponse(true, "Usuario eliminado correctamente.");
    }

    private static string? Validar(CreateUsuarioDto dto)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            errores.Add("El nombre es obligatorio.");
        else if (dto.Nombre.Trim().Length < 2 || dto.Nombre.Trim().Length > 100)
            errores.Add("El nombre debe tener entre 2 y 100 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Departamento))
            errores.Add("El departamento es obligatorio.");
        else if (dto.Departamento.Trim().Length > 100)
            errores.Add("El departamento no puede tener más de 100 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.Correo))
            errores.Add("El correo es obligatorio.");
        else if (dto.Correo.Trim().Length > 150)
            errores.Add("El correo no puede tener más de 150 caracteres.");
        else if (!CorreoRegex.IsMatch(dto.Correo.Trim()))
            errores.Add("El correo no tiene un formato válido.");

        return errores.Count == 0 ? null : string.Join(" ", errores);
    }

    private static UsuarioDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Departamento = u.Departamento,
        Correo = u.Correo
    };
}
