﻿namespace Sotex.Api.Model
{
    public class SideDish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MenuItem MenuItem { get; set; }
        public Guid MenuItemId { get; set; }
        public decimal Price { get; set; }
    }
}