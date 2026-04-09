namespace RetailShop.DTO
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
