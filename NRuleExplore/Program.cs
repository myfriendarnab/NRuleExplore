﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuleAppCore;
using RuleAppCore.Domain;
using SimpleRuleEngine;

namespace NRuleExplore
{
    class Program
    {
        static void Main(string[] args)
        {
            var svcProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IRuleEngine,RuleEngine>()
                .AddTransient<IRuleService, MyRuleService>()
                .BuildServiceProvider();

            svcProvider.GetService<ILoggerFactory>();

            using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogInformation("Starting service");


                var myRuleSvc = svcProvider.GetService<IRuleService>();

                var orderItems = new List<OrderItem> {
                    new OrderItem("A",5),
                    new OrderItem("B", 2),
                    new OrderItem("C",1),
                    new OrderItem("D",1)
                };
                var orders = new Orders(orderItems);

                myRuleSvc.RunRuleService(orders);
                var promotionalPrice = myRuleSvc.GenerateInvoice(orders);
                Console.WriteLine($"Invoice value after applying promotions - {promotionalPrice}");
            }

            Console.ReadLine();
        }
    }
}
