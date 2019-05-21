/* This is a simple example to show the steps and one possible way of
 * automatically scanning for and connecting to a device to receive
 * notification data from the device.
 */

using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTest : MonoBehaviour
{
	public string DeviceName = "Bluefruit52";
	public string ServiceUUID = "27B7";
	public string SubscribeCharacteristic = "28C8";
	public string WriteCharacteristic = "28C9";
    public string StripCharacteristic = "28d1";
    public string DeviceMacAddress = "E5:2F:4A:11:A1:78";   //Station 0: "EA:5D:84:7A:37:ED"
                                                            //Station 1: "EB:97:ED:A2:E2:FC"
                                                            //Station 2: "E5:2F:4A:11:A1:78"
                                                            //Station 3: "F7:CC:85:CF:71:27"
                                                            //Station 4: ""
                                                            //Station 5: ""

    private string message = "Senior Max";
    private string previousString = "test";

    private bool sendMessage = false;
    public static bool updateUser = true;
    private bool resetUser = false;

    public static string bluetoothMessage_weight = "";
    public static float previousWeight = 0.0f;
    public static bool updateWeight = false;

    public GameObject inputText;

    enum States
	{
		None,
		Scan,
		ScanRSSI,
		Connect,
		Subscribe,
		Unsubscribe,
		Disconnect,
	}

	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private string _deviceAddress;
	private bool _foundSubscribeID = false;
	private bool _foundWriteID = false;
    private bool _foundStripID = false;
    private byte[] _dataBytes = null;
	private bool _rssiOnly = false;
	private int _rssi = 0;

	void Reset ()
	{
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_deviceAddress = null;
		_foundSubscribeID = false;
        _foundWriteID = false;
        _foundStripID = false;
        _dataBytes = null;
		_rssi = 0;
	}

	void SetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

	void StartProcess ()
	{
		Reset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			
			SetState (States.Scan, 0.1f);
			
		}, (error) => {
			
			BluetoothLEHardwareInterface.Log ("Error during initialize: " + error);
		});
	}

	// Use this for initialization
	void Start ()
	{
        StartProcess ();
	}

    public void sendMessagetoDevice()
    {
        sendMessage = true;
    }

    // Update is called once per frame
    void Update ()
	{
        if (GameController.login && updateUser)
        {
            SendByte(GameController.tmp_Name);
            updateUser = false;
            resetUser = true;
        }
        if(!GameController.login && updateUser)
        {
            if (resetUser)
            {
                SendByte("Niemand");
                resetUser = false;
            }
        }

		if (_timeout > 0f)
		{
			_timeout -= Time.deltaTime;
			if (_timeout <= 0f)
			{
				_timeout = 0f;
                Debug.Log("Searching...");
				switch (_state)
				{
				case States.None:
					break;

				case States.Scan:
					BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

                        // if your device does not advertise the rssi and manufacturer specific data
                        // then you must use this callback because the next callback only gets called
                        // if you have manufacturer specific data

                        Debug.Log("Device found: " + address + " and adaptive name: " + name);

                        if (!_rssiOnly)
						{
                            Debug.Log("Identify...");

                            //TODO for Station which is to much add: || address.Contains(DeviceMacAddress) || address.Contains("EB:97:ED:A2:E2:FC") || address.Contains("F7:CC:85:CF:71:27")
                            if (name.Contains (DeviceName) || address.Contains(DeviceMacAddress) || address.Contains("EB:97:ED:A2:E2:FC") || address.Contains("F7:CC:85:CF:71:27") || address.Contains("EA:5D:84:7A:37:ED"))
                            {
								BluetoothLEHardwareInterface.StopScan ();
                                Debug.Log("Connected to " + name + " " + address + "!");
                                // found a device with the name we want
                                // this example does not deal with finding more than one
                                _deviceAddress = address;
                                SetState (States.Connect, 0.5f);
							}
						}

					}, (address, name, rssi, bytes) => {

						// use this one if the device responses with manufacturer specific data and the rssi

						if (name.Contains (DeviceName) || address.Contains(DeviceMacAddress))
						{
							if (_rssiOnly)
							{
								_rssi = rssi;
							}
							else
							{
								BluetoothLEHardwareInterface.StopScan ();
								
								// found a device with the name we want
								// this example does not deal with finding more than one
								_deviceAddress = address;
								SetState (States.Connect, 0.5f);
							}
						}

					}, _rssiOnly); // this last setting allows RFduino to send RSSI without having manufacturer data

					if (_rssiOnly)
						SetState (States.ScanRSSI, 0.5f);
					break;

				case States.ScanRSSI:
					break;

				case States.Connect:
					// set these flags
					_foundSubscribeID = false;
					_foundWriteID = false;
                    _foundStripID = false;
                        Debug.Log("Looking for UUID's....");
					// note that the first parameter is the address, not the name. I have not fixed this because
					// of backwards compatiblity.
					// also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
					// the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
					// large enough that it will be finished enumerating before you try to subscribe or do any other operations.
					BluetoothLEHardwareInterface.ConnectToPeripheral (_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {

                        //Debug.Log("Device Service UUID: " + serviceUUID);
                        //Debug.Log("Our UUID: " + ServiceUUID);

                        if (IsEqual (serviceUUID, FullUUID(ServiceUUID)))
						{
                            Debug.Log("Service UUID found! " + characteristicUUID);

							_foundSubscribeID = _foundSubscribeID || IsEqual (characteristicUUID, FullUUID(SubscribeCharacteristic));
							_foundWriteID = _foundWriteID || IsEqual (characteristicUUID, FullUUID(WriteCharacteristic));
                            _foundStripID = _foundStripID || IsEqual(characteristicUUID, FullUUID(StripCharacteristic));

                            Debug.Log("frs UUID: " + _foundSubscribeID);
                            Debug.Log("Display UUID: " + _foundWriteID);
                            Debug.Log("Strip UUID: " + _foundStripID);

                            // if we have found both characteristics that we are waiting for
                            // set the state. make sure there is enough timeout that if the
                            // device is still enumerating other characteristics it finishes
                            // before we try to subscribe
                            if (_foundSubscribeID && _foundWriteID && _foundStripID) //& _foundStripID
							{
                                Debug.Log("UUID's ready to use...");
                                BroadcastReceiver.connected = true;
                                _connected = true;
                                SetState (States.Subscribe, 2f);
							}
						}
					});
					break;

				case States.Subscribe:
					BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_deviceAddress, ServiceUUID, SubscribeCharacteristic, null, (address, characteristicUUID, bytes) => {

						// we don't have a great way to set the state other than waiting until we actually got
						// some data back. For this demo with the rfduino that means pressing the button
						// on the rfduino at least once before the GUI will update.
						_state = States.None;

						// we received some data from the device
						_dataBytes = bytes;
					});
					break;

				case States.Unsubscribe:
					BluetoothLEHardwareInterface.UnSubscribeCharacteristic (_deviceAddress, ServiceUUID, SubscribeCharacteristic, null);
					SetState (States.Disconnect, 4f);
					break;

				case States.Disconnect:
					if (_connected)
					{
						BluetoothLEHardwareInterface.DisconnectPeripheral (_deviceAddress, (address) => {
							BluetoothLEHardwareInterface.DeInitialize (() => {

                                BroadcastReceiver.connected = false;
								_connected = false;
								_state = States.None;
							});
						});
					}
					else
					{
						BluetoothLEHardwareInterface.DeInitialize (() => {
							
							_state = States.None;
						});
					}
					break;
				}
			}
		}
	}

	private bool ledON = false;
	public void OnLED ()
	{
		ledON = !ledON;
		if (ledON)
		{
			SendByte ((byte)0x01);
		}
		else
		{
			SendByte ((byte)0x00);
		}
	}
	
	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
	}
	
	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);
		
		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}

    private void SendByte(string message)
    {
        byte[] data = Encoding.Default.GetBytes(message);

        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, WriteCharacteristic, data, data.Length, true, (characteristicUUID) => {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    void SendByte (byte value)
	{
		byte[] data = new byte[] { value };

        BluetoothLEHardwareInterface.WriteCharacteristic (_deviceAddress, ServiceUUID, WriteCharacteristic, data, data.Length, true, (characteristicUUID) => {
            
			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
	}

    void SendStripe(string stripID)
    {
        byte[] data = Encoding.Default.GetBytes(stripID);

        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, StripCharacteristic, data, data.Length, true, (characteristicUUID) => {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    void OnGUI ()
	{
		GUI.skin.textArea.fontSize = 32;
		GUI.skin.button.fontSize = 32;
		GUI.skin.toggle.fontSize = 32;
		GUI.skin.label.fontSize = 32;

		if (_connected)
		{

			if (_state == States.None)
			{
                /*
				if (GUI.Button (new Rect (10, 10, Screen.width - 10, 100), "Disconnect"))
					SetState (States.Unsubscribe, 1f);

                //if (GUI.Button(new Rect(10, 210, Screen.width - 10, 100), "Write Value"))
                    //SendStripe("3 7 9 10");//or "wheel" //SendByte(message); //OnLED ();
                    */

                if (_dataBytes != null && !updateWeight)
				{
                    string message = "";
                    bluetoothMessage_weight = "";
                    string data = Encoding.Default.GetString(_dataBytes);
                    if (!previousString.Equals(data))
                    {
                        previousString = data;
                        Debug.Log(data);
                        message += data;
                        bluetoothMessage_weight = message;
                        updateWeight = true;
                        BroadcastReceiver.hasDrunk = true;
                    }

                    //foreach (var b in _dataBytes)
                    // data += Encoding.Default.GetString(_dataBytes);
                    //data += b.ToString ("X") + " ";

                    //GUI.TextArea (new Rect (10, 400, Screen.width - 10, 300), data);
				}
			}
			else if (_state == States.Subscribe && _timeout == 0f)
			{
				//GUI.TextArea (new Rect (50, 100, Screen.width - 100, Screen.height - 200), "Press the button on the RFduino");
			}
		}
		else if (_state == States.ScanRSSI)
		{
			//if (GUI.Button (new Rect (10, 10, Screen.width - 10, 100), "Stop Scanning"))
			{
				//BluetoothLEHardwareInterface.StopScan ();
				//SetState (States.Disconnect, 0.5f);
			}
			
			//if (_rssi != 0)
				//GUI.Label (new Rect (10, 300, Screen.width - 10, 50), string.Format ("RSSI: {0}", _rssi));
		}
		else if (_state == States.None)
		{
			//if (GUI.Button (new Rect (10, 10, Screen.width - 10, 100), "Connect"))
				//StartProcess ();

			//_rssiOnly = GUI.Toggle (new Rect (10, 200, Screen.width - 10, 50), _rssiOnly, "Just Show RSSI");
		}
	}
}
