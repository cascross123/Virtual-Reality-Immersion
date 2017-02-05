using UnityEngine;
using System.Collections;
using WiimoteApi;



public class wiidetect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("This code runs");
        WiimoteManager.FindWiimotes(); // Poll native bluetooth drivers to find Wiimotes
        foreach (Wiimote remote in WiimoteManager.Wiimotes)
        {
            Debug.Log("We found a mote");
            remote.SendPlayerLED(true, false, false, true);
            remote.RumbleOn = true; // Enabled Rumble
            remote.SendStatusInfoRequest();
            remote.SendDataReportMode(InputDataType.REPORT_BUTTONS);
            int ret;
            do
            {
                ret = remote.ReadWiimoteData();
            } while (ret > 0);
            Debug.Log(ret);

        }
        if (!WiimoteManager.FindWiimotes())
        {
            Debug.Log("Sorry it doesn't look like I can find the motes, Maybe check the batteries!");

        }

    }

    // Update is called once per frame
    void Update()
    {


    }



}
