using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Com.Ericmas001.Rpi.Gpio;
using Com.Ericmas001.Rpi.Gpio.Abstractions;
using Com.Ericmas001.Rpi.Gpio.Enums;



public class counter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUi;
    [SerializeField] TextMeshProUGUI gamePad;
    [SerializeField] TextMeshProUGUI joystickName;

    public int number;
    public int initialCounter;
    private bool connectedJoyStick = false;


    private int gpioPinNumber = 3; // Número do pino GPIO conectado ao switch

    IGpioController gpioController;
    IGpioPin pin;

    void Start()
    {
        initialCounter = 0;
        StartCoroutine(CheckForControllers());

        // Inicializa o pino GPIO
        pin = gpioController.OpenPin(GpioEnum.Gpio02);
        pin.SetDriveMode(GpioPinDriveModeEnum.Input);
    }

    IEnumerator CheckForControllers()
    {
        while (true)
        {
            var controllers = Input.GetJoystickNames();

            if (!connectedJoyStick && controllers.Length > 0)
            {
                Debug.Log("Joystick(s) Connected:");
                for (int i = 0; i < controllers.Length; i++)
                {
                    joystickName.text = "Joystick " + (i + 1) + ": " + controllers[i];
                }
                connectedJoyStick = true;
                gamePad.text = "Connected";
            }
            else if (connectedJoyStick && controllers.Length == 0)
            {
                connectedJoyStick = false;
                gamePad.text = "Disconnected";
            }

            yield return new WaitForSeconds(1f);
        }
    }


    private void Update()
    {
        Sum();
    }

    private void Sum () {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            GpioPinValueEnum pinValue = pin.Read();
            // Verifica se o estado do pino é baixo (LOW)
            if (pinValue == GpioPinValueEnum.Low)
            {
                Debug.Log("Botão pressionado!");
            }

            if (number < initialCounter)
            {
                number = initialCounter;
                textUi.text = number.ToString();
            }
            else
            {
                number++;
                textUi.text = number.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Jump"))
        {
            if (number <= initialCounter) {
                number = initialCounter;

            } else
            {
                number--;
                textUi.text = number.ToString();
            }
                
        }
    }
}
