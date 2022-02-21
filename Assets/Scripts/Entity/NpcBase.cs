using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NpcBase : MonoBehaviour
{

    AttributeSystem aSystem;

    private void Start()
    {
        aSystem = GetComponent<AttributeSystem>();
        HealthAttribute h = new HealthAttribute();
        h.Name = "health";
        h.ChangeValue(100);
        aSystem.AddAttribute(h);
    }

    private void OnDestroy()
    {
        if(aSystem.GetAttributeValue("health") <= 0)
        UiManager.Instance.ChangeCurrency( UnityEngine.Random.Range(0f, 10f));
    }

}
