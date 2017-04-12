using UnityEngine;
using System.Collections;
using WiimoteApi;
using WiimoteApi.Util;


public class wiidetect : MonoBehaviour
{
    private Wiimote wiimote;

    private Vector3 wmpOffset = Vector3.zero;
    float x;
    float y;
    float z;
    float ir_x;
    float ir_y;
    float ir_x_new;
    float ir_y_new;
	float ir_exper_x;
	float player_position_x;
	float player_position_y;
	float player_position_z;
    private float speed = 20.0f;
    public ReadOnlyMatrix<int> ir { get { return _ir_readonly; } }
    private ReadOnlyMatrix<int> _ir_readonly;
    private int[,] _ir;
    bool onGround = true;
	GameObject temp;


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
            remote.SetupIRCamera(IRDataType.FULL);
            wiimote = remote;

        }

    }

    void calibrate_accel()
    {
        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();
        } while (ret > 0);



        float[] data = wiimote.Accel.GetCalibratedAccelData();

        x = data[0];
        y = data[1];
        z = data[2];
        wiimote.SendStatusInfoRequest();


    }

    void Start()
    {

        InitialiseWiimote();
        calibrate_accel();
		temp = GameObject.Find("player");





    }

    // Update is called once per frame
    void Update()
    {
        update_ir();

		Debug.Log (temp.transform.position);






    }
    void newAccel()
    {

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();
        } while (ret > 0);




        float[] data = wiimote.Accel.GetCalibratedAccelData();

        float new_x = data[0];
        float new_y = data[1];
        float new_z = data[2];

        wiimote.SendStatusInfoRequest();

        if (new_x < 1.181818)
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;

        }
        if (new_y > y)
        {
            y = new_y;
        }

        if (new_z > z)
        {
            z = new_z;
        }



    }

    void update_ir()
    {
        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();
        } while (ret > 0);

        float[] pointer = wiimote.Ir.GetPointingPosition();
        ir_x = pointer[0];
        ir_y = pointer[1];
        ir_x_new = ir_x * 100.0f;
        ir_y_new = ir_y * 100.0f;
		player_position_x = temp.transform.position.x;
		player_position_y = temp.transform.position.y;
		Debug.Log (player_position_x);
		Debug.Log (player_position_y);
		Debug.Log (player_position_z);

        
		if (ir_x == -1.0) {
			

		} 

		else 
		{
			transform.position = new Vector3(player_position_x, player_position_y, player_position_z);
		}


    }

    void OnCollisionEnter(Collision collisionInfo)
    {
		temp.transform.position = new Vector3 (0, 0, 0);
		Debug.Log ("This is colliding");
    }


    void OnCollisionExit(Collision collisionInfo)
    {
        Debug.Log("not colliding anymore");
    }

}