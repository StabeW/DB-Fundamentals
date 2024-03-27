using ProductShop.Data;
using Newtonsoft;
using Newtonsoft.Json;
using ProductShop.Models;
using Newtonsoft.Json.Serialization;

public class StartUp
{
    public static void Main(string[] args)
    {
        var context = new ProductShopContext();
        var jsonPath = File.ReadAllText(@"C:\\Users\\stani\\Documents\\FreeLancer\\DB-Fundamentals\\CSharp Database Advanced\\JavaScript Object Notation - JSON\\ProductShop\\Datasets\\categories-products.json");
        Console.WriteLine(GetUsersWithProducts(context));
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        var usersToImport = JsonConvert.DeserializeObject<User[]>(inputJson);

        var usersToAdd = new List<User>();

        foreach (var user in usersToImport)
        {
            if (string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrEmpty(user.LastName))
            {
                continue;
            }

            usersToAdd.Add(user);
        }

        context.Users.AddRange(usersToAdd);
        context.SaveChanges();

        return $"Successfully imported {usersToAdd.Count}";
    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        var productToImport = JsonConvert.DeserializeObject<Product[]>(inputJson);

        var productToAdd = new List<Product>();

        foreach (var product in productToImport)
        {
            if (string.IsNullOrWhiteSpace(product.Name) ||
                string.IsNullOrEmpty(product.Name))
            {
                continue;
            }

            productToAdd.Add(product);
        }

        context.Products.AddRange(productToAdd);
        context.SaveChanges();

        return $"Successfully imported {productToAdd.Count}";
    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        var categoriesToImport = JsonConvert.DeserializeObject<Category[]>(inputJson);

        var categoriesToAdd = new List<Category>();

        foreach (var category in categoriesToImport)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                continue;
            }
            categoriesToAdd.Add(category);
        }
        context.Categories.AddRange(categoriesToAdd);
        context.SaveChanges();

        return $"Successfully imported {categoriesToAdd.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        var validCategoryId = new HashSet<int>(
                context
                .Categories
                .Select(c => c.Id));

        var validProductId = new HashSet<int>(
            context
            .Products
            .Select(p => p.Id));

        var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

        var validCategoriesProducts = new List<CategoryProduct>();

        foreach (var item in categoriesProducts)
        {
            bool isValid = validCategoryId.Contains(item.CategoryId)
                        && validProductId.Contains(item.ProductId);

            if (isValid)
            {
                validCategoriesProducts.Add(item);
            }
        }

        context.CategoriesProducts.AddRange(validCategoriesProducts);
        context.SaveChanges();

        return $"Successfully imported {validCategoriesProducts.Count}";
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        var products = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .Select(p => new
            {
                name = p.Name,
                price = p.Price,
                seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
            })
            .OrderBy(p => p.price)
            .ToList();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        var products = context.Users
            .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold
                .Select(ps => new
                {
                    name = ps.Name,
                    price = ps.Price,
                    buyerFirstName = ps.Buyer.FirstName,
                    buyerLastName = ps.Buyer.LastName
                })
                .ToList()
            })
            .OrderBy(u => u.lastName)
            .ThenBy(u => u.firstName)
            .ToList();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categoriesByProductCount = context
                 .Categories
                 .OrderByDescending(c => c.CategoryProducts.Count)
                 .Select(c => new
                 {
                     category = c.Name,
                     productsCount = c.CategoryProducts.Count,
                     averagePrice = $"{c.CategoryProducts.Average(x => x.Product.Price):F2}",
                     totalRevenue = $"{c.CategoryProducts.Sum(x => x.Product.Price)}"
                 })
                 .ToList();

        string json = JsonConvert.SerializeObject(categoriesByProductCount, new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },

            Formatting = Formatting.Indented
        });

        return json;
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var usersProducts = context
                .Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                        .Count(ps => ps.Buyer != null),
                        products = u.ProductsSold
                        .Where(ps => ps.Buyer != null)
                        .Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price
                        })
                        .ToList()
                    }
                })
                .ToList();

        var result = new
        {
            usersCount = usersProducts.Count,
            users = usersProducts
        };

        return JsonConvert.SerializeObject(result, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        });
    }
}