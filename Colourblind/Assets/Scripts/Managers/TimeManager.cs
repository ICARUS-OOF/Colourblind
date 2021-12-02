using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colourblind.Managers
{
    public static class TimeManager
    {
        public static float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }
    }
}