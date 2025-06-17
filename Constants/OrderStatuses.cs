namespace EcommerceAPI.Constants
{
    public static class OrderStatuses
    {
        public const string Pending = "Pending";
        public const string PaymentPending = "PaymentPending";
        public const string PaymentProcessing = "PaymentProcessing";
        public const string PaymentFailed = "PaymentFailed";
        public const string Processing = "Processing";
        public const string Shipped = "Shipped";
        public const string Delivered = "Delivered";
        public const string Cancelled = "Cancelled";

        public static List<string> GetAllStatuses()
        {
            return new List<string>
               {
                   Pending,
                   PaymentPending,
                   PaymentProcessing,
                   PaymentFailed,
                   Processing,
                   Shipped,
                   Delivered,
                   Cancelled
               };
        }
    }

}
