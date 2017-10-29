using Phidgets;
using Phidgets.Events;
public class MotorData
{
    private MotorControl motorControl = null;
    public MotorControl MotorDevice { get { return motorControl; } }
    public MotorData()
    {
        try
        {
            motorControl = new MotorControl();
            Open();
        }
        catch { }
    }
    public void Open()
    {
        if (motorControl != null)
            if (!motorControl.Attached)
            {
                motorControl.Attach += new AttachEventHandler(motorControl_Attach);
                motorControl.Detach += new DetachEventHandler(motorControl_Detach);
                motorControl.Error += new ErrorEventHandler(motorControl_Error);
                motorControl.open(); //310019
            }
            else
                UnityEngine.Debug.Log("already open");
        else
            UnityEngine.Debug.LogWarning("Could not find the phidgets motor controller or drivers");
    }
    public void Close()
    {
        if (null != motorControl)
        {
            motorControl.Attach -= motorControl_Attach;
            motorControl.Detach -= motorControl_Detach;
            motorControl.Error -= motorControl_Error;
        }
        if (motorControl?.Attached ?? false)
        {
            for (int i = 0; i < motorControl.motors.Count; ++i)
                motorControl.motors[i].Velocity = 0.0f;
            try
            {
                motorControl.close();
                motorControl = null;
            }
            catch (PhidgetException pe)
            {
                UnityEngine.Debug.LogWarning("!!!!!!!!!!!" + pe.Description);
            }
        }
    }
    private void motorControl_Attach(object sender, AttachEventArgs e)
    {
        for (int i = 0; i < motorControl.motors.Count; ++i)
        {
            motorControl.motors[i].Acceleration = 6250.0f;
            motorControl.motors[i].Velocity = 0.0f;
        }
    }
    private void motorControl_Detach(object sender, DetachEventArgs e)
    {
        motorControl = null;
    }
    private void motorControl_Error(object sender, ErrorEventArgs e)
    {
        UnityEngine.Debug.LogWarning("Phidgets display error: " + e.ToString());
    }
}