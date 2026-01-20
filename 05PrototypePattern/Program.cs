using Mapster;

Console.WriteLine("Prototype Pattern");

#region Initialize
Product masaustuBilgisayar = new()
{
    Id = 1,
    Name = "Masaüstü Bilgisayar",
    Price = 100000,
    Stock = 10
};
#endregion

#region Old Version
//Product laptop = (Product)masaustuBilgisayar.Clone();
//laptop.Name = "Laptop";
#endregion

#region Mapping Version
Product laptop = masaustuBilgisayar.Adapt<Product>();
laptop.Name = "Laptop";
#endregion

#region Initialize
class Product : ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public object Clone()
    {
        return new Product()
        {
            Id = 0,
            Name = Name,
            Price = Price,
            Stock = Stock
        };
    }
}
#endregion