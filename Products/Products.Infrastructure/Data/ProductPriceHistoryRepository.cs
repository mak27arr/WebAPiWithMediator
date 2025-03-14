﻿using System;
using Products.Infrastructure.Models;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Infrastructure.Data
{
    public class ProductPriceHistoryRepository : IProductPriceHistoryRepository
    {
        private readonly DataContext _context;

        public ProductPriceHistoryRepository(DataContext context) => _context = context;

        public async Task AddAsync(ProductPriceHistory history)
        {
            await _context.ProductPriceHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }
    }
}
