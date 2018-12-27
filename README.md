# ObjectEngine
对象引擎，以路径形式访问对象属性,例data.Product[1].Name
用例：
### 用例

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
   ###
