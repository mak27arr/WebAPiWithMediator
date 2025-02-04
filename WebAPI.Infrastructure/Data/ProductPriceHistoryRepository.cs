﻿using System;
using WebAPI.Core.Models;
using WebAPI.Core.Repository;

namespace WebAPI.Infrastructure.Data
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
