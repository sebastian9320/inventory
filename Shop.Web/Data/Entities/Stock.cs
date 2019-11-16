using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class Stock : IEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ProductId { get; set; }
        
        public int WarehouseId { get; set; }

        public int quantity { get; set; }

        public Product Product { get; set; }
        
    }
}
