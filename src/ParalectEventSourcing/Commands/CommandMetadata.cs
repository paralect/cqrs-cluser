namespace ParalectEventSourcing.Commands
{
    using System;

    /// <summary>
    /// Command Metadata
    /// </summary>
    public class CommandMetadata : ICommandMetadata
    {
        /// <summary>
        /// Gets or sets unique Id of Command
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// Gets or sets user Id of user who initiate this command
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets assembly qualified CLR Type name of Command Type
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets time when command was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets SignalR Connection Id
        /// </summary>
        public string ConnectionId { get; set; }
    }
}