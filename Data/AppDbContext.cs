﻿using Microsoft.EntityFrameworkCore;
using WebAppi.Models;

namespace WebAppi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Shoe> Shoes { get; set; } 
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}