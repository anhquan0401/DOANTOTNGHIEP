namespace WebApplication1.ViewModels
{
    public class CheckoutPageVM
    {
        public List<CartVM> Carts { get; set; } = new List<CartVM>();
        public IEnumerable<VoucherUserVM> VoucherUsers { get; set; } = new List<VoucherUserVM>();
        public CheckoutVM CheckoutVM { get; set; } = new CheckoutVM();
        public int[] SelectedVoucherUsers { get; set; } = Array.Empty<int>();
    }
}
