namespace CC.Common.Models
{
    public class EmailMessage
    {
        /// <summary>
        /// Email where to send
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Subject of the notification
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Message that will be send within notification
        /// </summary>
        public string? Message { get; set; }

    }
}
