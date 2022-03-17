using CollabClothing.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasData(new Brand() { Id = "01", NameBrand = "Adidas", Info = "Brand Adidas", Images = "adidas.png", Slug = "/adidas" },
                new Brand() { Id = "02", NameBrand = "Nike", Info = "Brand Nike", Images = "nike.png", Slug = "/nike" });

            modelBuilder.Entity<Category>().HasData(new Category() { Id = "10001", NameCategory="Men", ParentId="null", Icon="men", Level=1, IsShowWeb=true, Slug="/men"},
                                                    new Category() { Id = "10002", NameCategory = "Women", ParentId = "null", Icon = "women", Level = 1, IsShowWeb = true, Slug = "/women" },
                                                    new Category() { Id = "20001", NameCategory = "T-Shirt", ParentId = "10001", Icon = "men", Level = 2, IsShowWeb = true, Slug = "/men/t-shirt" },
                                                    new Category() { Id = "20002", NameCategory = "T-Shirt", ParentId = "10002", Icon = "women", Level = 2, IsShowWeb = true, Slug = "/women/t-shirt" });

            modelBuilder.Entity<Product>().HasData(new Product() { Id="01", ProductName="Ao thun nam ", PriceCurrent=300000, PriceOld=450000, SaleOff=20, 
                                                                    BrandId="01", SoldOut=false,  Installment=0, Description="", Slug="/" },
                                                    new Product()
                                                    {
                                                        Id = "02",
                                                        ProductName = "Ao thun nam 2",
                                                        PriceCurrent = 400000,
                                                        PriceOld = 450000,
                                                        SaleOff = 20,
                                                        BrandId = "02",
                                                        SoldOut = true,
                                                        Installment = 0,
                                                        Description = "",
                                                        Slug = "/"
                                                    },
                                                    new Product()
                                                    {
                                                        Id = "03",
                                                        ProductName = "Ao thun nu",
                                                        PriceCurrent = 200000,
                                                        PriceOld = 350000,
                                                        SaleOff = 20,
                                                        BrandId = "01",
                                                        SoldOut = false,
                                                        Installment = 0,
                                                        Description = "",
                                                        Slug = "/"
                                                    },
                                                    new Product()
                                                    {
                                                        Id = "04",
                                                        ProductName = "Ao thun nu 1",
                                                        PriceCurrent = 200000,
                                                        PriceOld = 320000,
                                                        SaleOff = 20,
                                                        BrandId = "02",
                                                        SoldOut = false,
                                                        Installment = 0,
                                                        Description = "",
                                                        Slug = "/"
                                                    });
            modelBuilder.Entity<ProductMapCategory>().HasData(new ProductMapCategory() { ProductId="01", CategoryId="20001"},
                                                                new ProductMapCategory() { ProductId = "02", CategoryId = "20002" });
        }
    }
}
