using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGameController
{
    public interface ICustomAirController
    {
        #region AIR SIMULATION PROPERTIES
        bool InFlight { get; set; }
        float InFlightSpeed { get; set; }
        float InFlightAcceleration { get; set; }
        Vector3 FlightVelocity { get; set; }
        float FlightHorizontalRotation { get; set; }
        float FlightVerticalRotation { get; set; }
        #endregion

        #region AIR SIMULATION METHODS
        void EnteringFlightState();
        void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed);
        void UpdateFlightHeightPosition(bool ascendingFlight, bool descendingFlight);
        #endregion
    }
}
