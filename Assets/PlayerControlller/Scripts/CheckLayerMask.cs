using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[UnitCategory("Utility")]
public class CheckLayerMask : Unit
{

    [DoNotSerialize]
    public ValueInput LayerToCheck;
    [DoNotSerialize]
    public ValueInput LayerMask;
    [DoNotSerialize]
    public ValueOutput Result;

    LayerMask mask;
    int layerToCheck;
    bool result;

    protected override void Definition()
    {

        LayerToCheck = ValueInput<int>("Layer", 0);
        LayerMask = ValueInput<LayerMask>("LayerMask", 0);
        Result = ValueOutput<bool>("", (flow) => {
            mask = flow.GetValue<LayerMask>(LayerMask);
            layerToCheck = flow.GetValue<int>(LayerToCheck);

            result = ((mask.value & (1 << layerToCheck)) > 0);
            return result;
        });

        Requirement(LayerToCheck, Result);
        Requirement(LayerMask, Result);
    }


    
}
