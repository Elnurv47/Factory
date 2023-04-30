public struct Product
{
    public ItemSO Item { get; private set; }
    public int Amount { get; set; }

    public Product(ItemSO item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}
