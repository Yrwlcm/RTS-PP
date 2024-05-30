using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scipts
{
    public class Action : MonoBehaviour
    {
        public void OnClick()
        {
            ActionFrame.instance.StartSpawnTimer(name);
        }
    }
}

