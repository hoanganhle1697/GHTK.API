using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHTK.Repository.Abstractions.Entity;

public class Order
{
    public string Id { get; set; }

    public string PartnerId { get; set; }

    public string PickName { get; set; }

    public string PickAddress { get; set; }

    public string PickProvince { get; set; }

    public string PickDistrict { get; set; }

    public string PickWard { get; set; }

    public string PickTel { get; set; }

    public string Tel { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string Province { get; set; }

    public string District { get; set; }

    public string Ward { get; set; }

    public string Hamlet { get; set; }

    public int IsFreeship { get; set; }

    public DateTimeOffset PickDate { get; set; }

    public long PickMoney { get; set; }

    public string Note { get; set; }

    public long Value { get; set; }

    public string Transport { get; set; }

    public string PickOption { get; set; }

    public string DeliverOption { get; set; }
    public List<Product> Products { get; set; } = [];
    public string TrackingId { get; set; }
}
