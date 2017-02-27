using UnityEngine;
using System.Collections;
using WiimoteApi;



public class wiidetect : MonoBehaviour
{
    private Wiimote wiimote;
    
    private Vector3 wmpOffset = Vector3.zero;

    // Use this for initialization

    void InitialiseWiimote()
    {

        Debug.Log("This code runs");
        WiimoteManager.FindWiimotes(); // Poll native bluetooth drivers to find Wiimotes
        foreach (Wiimote remote in WiimoteManager.Wiimotes)
        {
            Debug.Log("We found a mote");
            remote.SendPlayerLED(true, false, false, true);

            remote.SendStatusInfoRequest();
            remote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
            wiimote = remote;
        }
        
    }
    void Start()
    {

        InitialiseWiimote();
       



    }

    // Update is called once per frame
    void Update()
    {
        
        int ret;
           do
           {
            ret = wiimote.ReadWiimoteData();
            } while (ret > 0);


        
        float[] data = wiimote.Accel.GetCalibratedAccelData();
        
            float x = data[0];
            float y = data[1];
            float z = data[2];
            wiimote.SendStatusInfoRequest();


        x -= 0.5f;
            y -= 0.5f;
        Debug.Log(x);
        Debug.Log(y);
        Debug.Log(z);



    }

}