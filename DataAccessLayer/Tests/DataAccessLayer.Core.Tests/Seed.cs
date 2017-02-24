﻿using System;
using System.Collections.Generic;
using DataAccessLayer.Core.Interfaces.Repositories;
using DataAccessLayer.Core.Interfaces.UoW;
using DataAccessLayer.Core.Tests.TestModels;

namespace DataAccessLayer.Core.Tests
{
    public static class Seed
    {
        public static void SeedData(TestDbContext context)
        {
            ProductProducer producer = new ProductProducer()
            {
                Address ="4 steet NYC",
                Name = "Volvo"
            };

            context.ProductProducers.Add(producer);

            Product product = new Product()
            {
                Price = 2.2m,
                ProducerId = producer.Id,
                Type= "Car"
            };

            context.Products.Add(product);
            
            Order order = new Order()
            {
                Products = new List<Product>(),
                PlaceOrderTimeStamp = DateTime.Now
            };
            order.Products.Add(product);
            context.Orders.Add(order);
            Client client = new Client()
            {
                Name = "Jan",
                PhoneNumer = "123123123",
                Surname = "Kowalski",
                Orders = new List<Order>()
            };
            client.Orders.Add(order);
            context.Clients.Add(client);

            Client client2 = new Client()
            {
                Name = "Roman",
                PhoneNumer = "123123123",
                Surname = "",
                Orders = new List<Order>()
            };
            client2.Orders.Add(order);
            context.Clients.Add(client2);

            context.SaveChanges();
        }
    }
}
