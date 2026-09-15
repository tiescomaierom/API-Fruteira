using System;

namespace API_Fruteira.MODELS
{
    public class Venda
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
