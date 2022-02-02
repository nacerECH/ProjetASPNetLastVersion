using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectAspNETv2.Models
{
    public class MinimizeProduct
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string category { get; set; }
        public string imagePath { get; set; }
        public int sellerId { get; set; }
        public string sellerName { get; set; }
        public DateTime created_at { get; set; }

    }
}