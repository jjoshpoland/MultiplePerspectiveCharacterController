using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Cinemachine;

[UnitTitle("Set Cinemachine Camera Distance"), UnitCategory("Cinemachine")]
public class UpdateCinemachineFollowDistance : Unit
{
    [DoNotSerialize] // No need to serialize ports.
    [PortLabelHidden]
    public ControlInput inputTrigger; //Adding the ControlInput port variable

    [DoNotSerialize] // No need to serialize ports.
    [PortLabelHidden]
    public ControlOutput outputTrigger;//Adding the ControlOutput port variable.

    [DoNotSerialize]
    public ValueInput CinemachineObject;

    [DoNotSerialize]
    public ValueInput Zoom;

    [DoNotSerialize]
    public ValueInput MaxZoom;

    [DoNotSerialize]
    public ValueOutput NewZoom;

    GameObject camera;
    float zoom;
    float maxZoom;
    float newZoom;

    protected override void Definition()
    {
        //Making the ControlInput port visible, setting its key and running the anonymous action method to pass the flow to the outputTrigger port.
        inputTrigger = ControlInput("inputTrigger", (flow) => {

            camera = flow.GetValue<GameObject>(CinemachineObject);
            zoom = flow.GetValue<float>(Zoom);
            maxZoom = flow.GetValue<float>(MaxZoom);
            
            Cinemachine3rdPersonFollow bodyComponent = camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent(CinemachineCore.Stage.Body) as Cinemachine3rdPersonFollow;
            bodyComponent.CameraDistance = Mathf.Clamp(bodyComponent.CameraDistance +  zoom, 0, maxZoom);
            newZoom = bodyComponent.CameraDistance;
            return outputTrigger; });
        //Making the ControlOutput port visible and setting its key.
        outputTrigger = ControlOutput("outputTrigger");

        CinemachineObject = ValueInput<GameObject>("VCM", null);
        Zoom = ValueInput<float>("Zoom", 0f);
        MaxZoom = ValueInput<float>("MaxZoom", 10f);
        NewZoom = ValueOutput<float>("NewZoom", (flow) => newZoom);

        Requirement(CinemachineObject, inputTrigger);

        Requirement(Zoom, inputTrigger);

        Succession(inputTrigger, outputTrigger);

        Assignment(inputTrigger, NewZoom);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
