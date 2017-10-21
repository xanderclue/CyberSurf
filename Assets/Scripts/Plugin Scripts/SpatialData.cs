using System.Collections.Generic;
using Phidgets;
using Phidgets.Events;
using Spatial_full;
using System;

public class SpatialData
{
    public Spatial device;

    public SpatialData()
    {
        try
        {
            device = new Spatial();
            Open();
        }
        catch { }
    }

    // Spatial (accel,gyro,compass phidget) Members
    public double pitchAngleStart = -1000.0; // store the starting pitch angle for reference
    public double pitchAngle = 0.0;
    public double rollAngle = 0.0;
    private double[] lastMsCount = { 0.0, 0.0, 0.0 };
    private bool[] lastMsCountGood = { false, false, false };
    private double[] gyroHeading = { 0.0, 0.0, 0.0 }; //degrees
    private List<double[]> compassBearingFilter = new List<double[]>();
    private int compassBearingFilterSize = 10;
    private double lastBearing = 0.0;

    private const double ambientMagneticField = 0.57142; //Calgary
    private const double ambientGravity = 1.0; //in G's
    public float fixedDeltaTime = 0.0f;

    public void Open()
    {
        if (null != device)
        {
            device.open();

            device.Attach += new AttachEventHandler(spatial_Attach);
            device.SpatialData += new SpatialDataEventHandler(spatial_SpatialData);
        }
    }

    public void Close()
    {
        if (device != null && device.Attached)
        {
            //When the application is being terminated, close the Phidget
            device.Attach -= new AttachEventHandler(spatial_Attach);
            device.SpatialData -= new SpatialDataEventHandler(spatial_SpatialData);

            device.close();
        }
    }

    //spatial attach event handler
    private void spatial_Attach(object sender, AttachEventArgs e)
    {
        Spatial attached = (Spatial)sender;

        attached.DataRate = 8;

        lastMsCountGood[0] = false;
        lastMsCountGood[1] = false;
        lastMsCountGood[2] = false;
        gyroHeading[0] = 0.0;
        gyroHeading[1] = 0.0;
        gyroHeading[2] = 0.0;
    }

    void spatial_SpatialData(object sender, SpatialDataEventArgs e)
    {
        if (device.gyroAxes.Count > 0)
        {
            calculateGyroHeading(e.spatialData, 0); //x axis
            calculateGyroHeading(e.spatialData, 1); //y axis
            calculateGyroHeading(e.spatialData, 2); //z axis
        }

        //Even when there is a compass chip, sometimes there won't be valid data in the event.
        if (device.compassAxes.Count > 0 && e.spatialData[0].MagneticField.Length > 0)
            try { calculateCompassBearing(); } catch { }
    }

    //This integrates gyro angular rate into heading over time
    void calculateGyroHeading(SpatialEventData[] data, int index)
    {
        for (int i = 0; i < data.Length; ++i)
        {
            if (lastMsCountGood[index])
                gyroHeading[index] += fixedDeltaTime * data[i].AngularRate[index];
            lastMsCount[index] = data[i].Timestamp.TotalMilliseconds;
            lastMsCountGood[index] = true;
        }
    }
    private const double _2pi = 2.0 * Math.PI;
    void calculateCompassBearing()
    {
        //find the tilt of the board wrt gravity
        SpVector3 gravity = SpVector3.Normalize(
            new SpVector3(
            device.accelerometerAxes[0].Acceleration,
            device.accelerometerAxes[2].Acceleration,
            device.accelerometerAxes[1].Acceleration)
        );

        //double pitchAngle = (double)Mathf.Asin((float)gravity.X);
        //double rollAngle = (double)Mathf.Asin((float)gravity.Z);
        pitchAngle = Math.Asin(gravity.Z);
        rollAngle = Math.Asin(gravity.X);

        //The board is up-side down
        if (gravity.Y < 0.0)
        {
            pitchAngle = -pitchAngle;
            rollAngle = -rollAngle;
        }

        //Construct a rotation matrix for rotating vectors measured in the body frame, into the earth frame
        //this is done by using the angles between the board and the gravity vector.
        SpVector3 correctedData = Matrix3x3.Multiply(
            new SpVector3(device.compassAxes[0].MagneticField, device.compassAxes[2].MagneticField, -device.compassAxes[1].MagneticField), Matrix3x3.Multiply(
                new Matrix3x3(Math.Cos(pitchAngle), -Math.Sin(pitchAngle), 0.0, Math.Sin(pitchAngle), Math.Cos(pitchAngle), 0.0, 0.0, 0.0, 1.0),
                new Matrix3x3(1.0, 0.0, 0.0, 0.0, Math.Cos(rollAngle), -Math.Sin(rollAngle), 0.0, Math.Sin(rollAngle), Math.Cos(rollAngle))));

        //These represent the x and y components of the magnetic field vector in the earth frame
        double Xh = -correctedData.X;
        double Yh = -correctedData.Z;

        //we use the computed X-Y to find a magnetic North bearing in the earth frame
        try
        {
            double bearing = 0.0;
            if (Xh < 0.0)
                bearing = Math.PI - Math.Atan(Yh / Xh);
            else if (Xh > 0.0 && Yh < 0.0)
                bearing = -Math.Atan(Yh / Xh);
            else if (Xh > 0.0 && Yh > 0.0)
                bearing = _2pi - Math.Atan(Yh / Xh);
            else if (0.0 == Xh && Yh < 0.0)
                bearing = Math.PI * 0.5;
            else if (0.0 == Xh && Yh > 0.0)
                bearing = Math.PI * 1.5;

            //The board is up-side down
            if (gravity.Y < 0.0)
                bearing = Math.Abs(bearing - _2pi);

            //passing the 0 <-> 360 point, need to make sure the filter never contains both values near 0 and values near 360 at the same time.
            if (Math.Abs(bearing - lastBearing) > 2.0) //2 radians == ~115 degrees
                if (bearing > lastBearing)
                    foreach (double[] stuff in compassBearingFilter)
                        stuff[0] += _2pi;
                else
                    foreach (double[] stuff in compassBearingFilter)
                        stuff[0] -= _2pi;

            compassBearingFilter.Add(new double[] { bearing, pitchAngle, rollAngle });
            if (compassBearingFilter.Count > compassBearingFilterSize)
                compassBearingFilter.RemoveAt(0);

            bearing = pitchAngle = rollAngle = 0.0;
            foreach (double[] stuff in compassBearingFilter)
            {
                bearing += stuff[0];
                pitchAngle += stuff[1];
                rollAngle += stuff[2];
            }
            bearing /= compassBearingFilter.Count;
            pitchAngle /= compassBearingFilter.Count;
            rollAngle /= compassBearingFilter.Count;

            lastBearing = bearing;

            if (-1000.0 == pitchAngleStart)
                pitchAngleStart = pitchAngle;
        }
        catch { }
    }
}