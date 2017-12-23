using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class TimePaceChanger : MonoBehaviour
    {
        public void ChangePaceOfTime(float newPace)
        {
            Time.timeScale = newPace;
        }
    }
}
