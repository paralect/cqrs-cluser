namespace WebApi
{
    using System.Collections.Concurrent;

    public class CommandConnectionsDictionary
    {
        private readonly ConcurrentDictionary<string, string> _commandConnections = new ConcurrentDictionary<string, string>();

        public void AddCommandConnection(string commandId, string connectionId)
        {
            _commandConnections.TryAdd(commandId, connectionId);
        }

        public string GetAndRemoveCommandConnection(string commandId)
        {
            string connectionId;
            _commandConnections.TryRemove(commandId, out connectionId);

            return connectionId;
        }
    }
}
