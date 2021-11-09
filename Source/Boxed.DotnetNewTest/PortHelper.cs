namespace Boxed.DotnetNewTest;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// Port helper methods.
/// </summary>
internal static class PortHelper
{
    /// <summary>
    /// Gets a free TCP port.
    /// </summary>
    /// <returns>The free TCP port number.</returns>
    public static int GetFreeTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
