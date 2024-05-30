using UnityEngine;
using UnityEngine.Events;

public class Switch_2Events : MonoBehaviour
{
    public UnityEvent Event1, Event2;
    bool state = false;

    public bool StartAsSwitched
    {
        get => state;

        set => state = value;
    }


    public void SwitchEvent()
    {
        if (state)
        {
            Event1?.Invoke();
        }
        else
        {
            Event2?.Invoke();
        }

        state = !state;
    }

    public void SwitchEvent(int index)
    {
        if (index == 1)
        {
            state = true;
            SwitchEvent();
        }
        else if (index == 2)
        {
            state = false;
            SwitchEvent();
        }
    }
}
