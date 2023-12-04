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
        #endregion

        #region AIR SIMULATION METHODS
        void EnteringAirState();
        void IdleAirState();
        void UpdateFlightPosition(Vector3 inputDirection, float movementSpeed);
        void UpdateAirHeightPosition();
        #endregion
    }
}
