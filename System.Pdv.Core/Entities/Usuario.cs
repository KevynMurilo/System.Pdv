﻿using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Usuario
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public string Nome { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
}
