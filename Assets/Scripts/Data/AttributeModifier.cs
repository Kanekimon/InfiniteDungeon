using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AttributeModifier
{

    private ModifierType modifierType;
    private float value;


    public ModifierType ModifierType { get { return modifierType; } set { modifierType = value; } }

    public float Value { get { return value; } set { this.value = value; } }


    public AttributeModifier(ModifierType ty, float value)
    {
        modifierType = ty;
        this.value = value;
    }

}
