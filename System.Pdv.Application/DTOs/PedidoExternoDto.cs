﻿using System.ComponentModel.DataAnnotations;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Application.DTOs;

public class PedidoExternoDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres.")]
    public string NomeCliente { get; set; }

    [Required(ErrorMessage = "O telefone do cliente é obrigatório.")]
    [Phone(ErrorMessage = "O telefone informado não é válido.")]
    public string TelefoneCliente { get; set; }

    [Required(ErrorMessage = "O ID do garçom é obrigatório.")]
    public Guid GarcomId { get; set; }

    [JsonIgnore]
    public TipoPedido TipoPedido { get; set; } = TipoPedido.Externo;

    [Required(ErrorMessage = "O ID do método de pagamento é obrigatório.")]
    public Guid MetodoPagamentoId { get; set; }

    [Required(ErrorMessage = "O ID do status do pedido é obrigatório.")]
    public Guid StatusPedidoId { get; set; }

    [Required(ErrorMessage = "Os itens do pedido são obrigatórios.")]
    [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
    public IList<ItemPedidoDto> Itens { get; set; }
}
