using UnityEngine;

public class VehicleInput : MonoBehaviour
{
    //private float thrust;
    //private float steer;				
    //private bool isDrifting;

    public float Thrust { get; protected set; }
    public float Steer { get; protected set; }
    public bool IsDrifting { get; protected set; }

    public bool stuntingSameAsDrifting = true;

    public bool StuntingSameAsDrifting
    {
        get { return stuntingSameAsDrifting; }
    }

    private bool isStunting;
    public bool IsStunting
    {
        get
        {
            if (StuntingSameAsDrifting)
            {
                return IsDrifting;
            }
            else
            {
                return isStunting;
            }
        }
        protected set
        {
            if (StuntingSameAsDrifting)
            {
                IsDrifting = value;
            }
            else
            {
                isStunting = value;
            }
        }
    }
}