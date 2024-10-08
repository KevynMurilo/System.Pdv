﻿using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class ItemPedido
{
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    [JsonIgnore]
    public Pedido Pedido { get; set; }

    [JsonIgnore]
    public Guid ProdutoId { get; set; }
    public Produto Produto { get; set; }

    public int Quantidade { get; set; }
    public string Observacoes { get; set; }

    public ICollection<ItemAdicional> Adicionais { get; set; }
}
