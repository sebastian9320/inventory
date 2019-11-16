using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class Warehouse : IEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Surface { get; set; }
    }
}
