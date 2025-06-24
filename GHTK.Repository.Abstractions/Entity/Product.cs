using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHTK.Repository.Abstractions.Entity;

public class Product
{
    public string Name { get; set; }

    public double Weight { get; set; }

    public long Quantity { get; set; }

    public long ProductCode { get; set; }
}
