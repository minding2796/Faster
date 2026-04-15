using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace GameLogic
{
    public class KeyBindingMaster : SingleMono<KeyBindingMaster>
    {
        public GameObject[] lines;
        public RebindActionUI[] rebinds;

        public void OnL1(InputValue value) => lines[0].SetActive((int)value.Get<float>() == 1);
        public void OnL2(InputValue value) => lines[1].SetActive((int)value.Get<float>() == 1);
        public void OnL3(InputValue value) => lines[2].SetActive((int)value.Get<float>() == 1);
        public void OnL4(InputValue value) => lines[3].SetActive((int)value.Get<float>() == 1);

        public void CloseModal()
        {
            gameObject.SetActive(false);
            Settings.Instance.uiInput.enabled = true;
        }
        
        public void OnPause(InputValue value) => CloseModal();
        
        public void OnCancel(InputValue value)
        {
            foreach (var rebindActionUI in rebinds)
            {
                var o = rebindActionUI.RebindingOperation;
                if (o is { completed: false }) o.Cancel();
            }
        }
    }
}
