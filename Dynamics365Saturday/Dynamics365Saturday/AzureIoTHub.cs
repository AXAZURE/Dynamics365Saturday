using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using AXZDynamicsSaturday;

class AzureIoTHub
{
    private static void CreateClient()
    {
        if (deviceClient == null)
        {
            // create Azure IoT Hub client from embedded connection string
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }
    }

    static DeviceClient deviceClient = null;

    const string deviceConnectionString = AXZSecrets.deviceConnectionString;
        
    public static async Task SendDeviceToCloudMessageAsync()
    {
        CreateClient();
#if WINDOWS_UWP
        var str = "{\"deviceId\":\"axazure\",\"messageId\":1,\"text\":\"Hello, Cloud from a UWP C# app!\"}";
#else
        var str = "{\"deviceId\":\"axazure-rpi\",\"messageId\":1,\"text\":\"Hello, Cloud from a C# app!\"}";
#endif
        var message = new Message(Encoding.UTF8.GetBytes(str));
     
        try
        {
            await deviceClient.SendEventAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static async Task<string> ReceiveCloudToDeviceMessageAsync()
    {
        CreateClient();

        while (true)
        {
            var receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
                return messageData;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
    
    //JATOM - 27/03/2018 - Enviar peso a Azure IoT Hub
    public static async Task<string> SendWeigh(string weigh, string matricula, string operationId, string appointmentId)
    {
        CreateClient();

        var str = "{\"deviceId\":\"axazure\",\"operationId\":\"" + operationId + "\",\"appointmentId\":\"" + appointmentId + "\",\"matricula\":" + matricula + ",\"peso\":\"" + weigh + "\"}";

        var message = new Message(Encoding.ASCII.GetBytes(str));

        await deviceClient.SendEventAsync(message);

        string result = await ReceiveAzureData();

        return result;
    }

    public static async Task<string> ReceiveAzureData()
    {
        CreateClient();

        while (true)
        {
            Console.WriteLine("Esperando mensaje...");
            var receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
                return await Task.FromResult<string>(messageData);
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
