using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ObjectEngine.UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Product product = new Product() { Url = "http://www.baidu.com", CreateTime = DateTime.Now };
            List<Product> productList = new List<Product>();
            for (int i = 0; i < 15100; i++)
            {
                Product p = new Product();
                p.Name = $"产品{i}";
                p.Url = $"http://www.baidu.com{i}";
                p.Remark = $"备注{i}";
                p.Index = i + 1;
                p.Price = i + 3.5m;
                p.CreateTime = DateTime.Now;
                productList.Add(p);
            }

            User user = new User() { Name = "张三", Age = 18, Product = productList, ProductSigle = product };
            ObjectEngine ObjectEngine = new ObjectEngine();
            ObjectEngine.SetData(user);
            var value = ObjectEngine.GetValue("data.Name");

            var value1 = ObjectEngine.GetValue("data.ProductSigle.Url");

            var value2 = ObjectEngine.GetValue("data.Product[1].Name");
        }

        public class User
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public Product ProductSigle { get; set; }

            public List<Product> Product { get; set; }

            public List<User> UserList { get; set; }
        }

        public class Product
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }

            public string Url { get; set; }

            public DateTime CreateTime { get; set; }

            public string Remark { get; set; }
        }
    }
}
