using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;
using System.Globalization;
using TMPro;

public class ConnectArduino : MonoBehaviour
{
    private BluetoothHelper helper;
    public string deviceName = "HC-05";
    private string received_message;
    public float RotationValue { get; private set; } = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            BluetoothHelper.BLE = false;
            helper = BluetoothHelper.GetInstance(deviceName);
            helper.OnConnected += OnConnected;
            helper.OnConnectionFailed += OnConnectionFailed;
            helper.OnDataReceived += OnMessageReceived; //read the data
            helper.setLengthBasedStream();

            


    // Debuglog: connected and paired devices:
    LinkedList<BluetoothDevice> ds = helper.getPairedDevicesList();
            foreach (BluetoothDevice d in ds)
            {
                Debug.Log($"{d.DeviceName} {d.DeviceAddress}");
            }

            // Connect to selected device:
            Connect();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Update()
    {
        /*
        //Synchronous method to receive messages
        if (helper != null)
            if (helper.Available)
            {
                received_message = helper.Read();

                Debug.Log(received_message);
            }*/
    }

    void OnMessageReceived(BluetoothHelper helper)
    {
        //StartCoroutine(blinkSphere());
        received_message = helper.Read();
        //     Debug.Log("Msg:" + received_message);

        string[] vecOrientation = received_message.Split(',');
        //     char[] data = received_message.ToCharArray();
        //        Debug.Log(data);

        try
        {
            float rotation = float.Parse(vecOrientation[0], CultureInfo.InvariantCulture);

            
            RotationValue = Mathf.Clamp01(rotation);


            Debug.Log("rotation:" + rotation);
            // go.transform.eulerAngles = new Vector3(r, h*-1.0f, p);
            
        }
        catch
        {
            Debug.Log("Data Error!");
        }
    }

    void OnConnected(BluetoothHelper helper)
    {
        try
        {
            helper.StartListening();
            Debug.Log("Connected to: " + deviceName);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
        Debug.Log("Failed to connect");
    }

    public void Connect()
    {
        helper.Connect();
    }


    public void Disconnect()
    {
        helper.Disconnect();
    }

    public void sendData(string d)
    {
        if (helper.isConnected())
            helper.SendData(d);
    }

    void OnDestroy()
    {
        if (helper != null)
            helper.Disconnect();
    }
}
