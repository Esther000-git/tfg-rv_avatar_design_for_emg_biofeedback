using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;    // ← aquí
using UnityEngine;
using UnityEngine.UI;                   // si también usas Text
using System.Globalization;



public class TCPServer : MonoBehaviour
{
    private TcpListener server;
    private Thread serverThread;
    private ConcurrentQueue<string> receivedMessages = new ConcurrentQueue<string>();

    private bool isRunning = false;
    private int port = 5500; // Puerto del servidor
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StartServer();
    }
    void Update(){
        // Procesar mensajes recibidos
        while (receivedMessages.TryDequeue(out string message))
        {   
            Debug.Log("Mensaje recibido: " + message);
            float[] channelValues = ProcessData(message);
            if (channelValues != null && AvatarMain.Instance != null)
            {
                AvatarMain.Instance.UpdateMuscleActivation(channelValues);
            }
        }
    }
    private float[] ProcessData(string data)
    {
        string[] values = data.Split(',');
        if (values.Length != 8)
        {
            Debug.LogWarning($"Se esperaban 8 canales, pero se recibieron {values.Length}");
            return null;
        }

        float[] channelValues = new float[8];
        for (int i = 0; i < 8; i++)
        {
            string s = values[i].Trim();
            Debug.Log($"Canal {i + 1}: '{s}'");

            // Parsear usando InvariantCulture, donde el punto es decimal
            if (!float.TryParse(
                    s,
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture,
                    out channelValues[i]
                ))
            {
                Debug.LogWarning($"No se pudo parsear el canal {i + 1}: '{s}'");
                return null;
            }

            // Ahora sí estará entre 0.0 y 1.0 si tus datos los cumplen
            channelValues[i] = Mathf.Clamp(channelValues[i], 0f, 1f);
            Debug.Log($"Canal {i + 1} clamped: {channelValues[i]}");
        }

        Debug.Log("Valores de los canales: " + string.Join(", ", channelValues));
        return channelValues;
    }
    private void StartServer(){
        if (isRunning) return;
        isRunning = true;               //si el servidor ya está corriendo no lo inicies de nuevo
        serverThread = new Thread(() =>
        {
            try {
                server = new TcpListener(IPAddress.Any, port);             // Crea un nuevo TcpListener en el puerto especificado
                server.Start();                                             
                Debug.Log("Servidor TCP iniciado en el puerto " + port);
                while (isRunning)
                {
                    if (!server.Pending())
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    TcpClient client = server.AcceptTcpClient();
                    Debug.Log("Cliente conectado");
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true; // Asegura que el hilo de cliente se cierra.
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error en el servidor TCP: " + e.Message);
            }
        });
        serverThread.IsBackground = true;
        serverThread.Start();
    }
    private void HandleClient (TcpClient client){
        Debug.Log("Cliente conectado");

        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        while (client.Connected){
            try{
                if ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    receivedMessages.Enqueue(data);  // <-- Meter en la cola
                    Debug.Log("Mensaje recibido y encolado: " + data);

                    byte[] response = Encoding.ASCII.GetBytes("Recibido: " + data);
                    stream.Write(response, 0, response.Length);
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Error en la conexión con el cliente: " + e.Message);
                break;
            }
        }
        client.Close();
        Debug.Log("Cliente desconectado");
    }
    public void OnAplicationQuit()
    {
        isRunning = false;
        server?.Stop();
        if (serverThread != null && serverThread.IsAlive)
    {
        serverThread.Join();  // Espera a que el hilo termine correctamente
    }
        Debug.Log("Servidor TCP detenido");
    }

    
}
