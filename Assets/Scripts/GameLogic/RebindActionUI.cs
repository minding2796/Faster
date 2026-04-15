using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace GameLogic
{
    public class RebindActionUI : MonoBehaviour
    {
        [SerializeField] private InputActionReference currentAction;
        // 특정 키(Composite의 자식)를 바인딩할 때 필요한 인덱스 (기본 0)
        [SerializeField] private int bindingIndex; 
        [SerializeField] private TextMeshProUGUI bindingDisplayNameText;
        [SerializeField] private GameObject selectedMarkObject;
        [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

        public InputActionRebindingExtensions.RebindingOperation RebindingOperation;
        public string oldPath;

        private void OnEnable()
        {
            if (currentAction != null)
                ShowBindText();
        }

        public void StartRebinding()
        {
            if (currentAction == null) return;

            // 1. 액션 비활성화 (필수)
            currentAction.action.Disable();
            selectedMarkObject.SetActive(false);
            oldPath = currentAction.action.bindings[bindingIndex].effectivePath;

            RebindingOperation = currentAction.action.PerformInteractiveRebinding(bindingIndex)
                .WithTargetBinding(bindingIndex)
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(_ => RebindCleanUp())
                .OnComplete(_ => {
                    RebindCleanUp();
                    
                    foreach (var o in KeyBindingMaster.Instance.rebinds)
                    {
                        if (o == this) continue;
                        var other = o.currentAction.action.bindings[o.bindingIndex].effectivePath;
                        var current = currentAction.action.bindings[bindingIndex].effectivePath;
                        if (other != current) continue;
                        o.currentAction.action.ApplyBindingOverride(oldPath);
                        o.ShowBindText();
                    }

                    ShowBindText();
                    GameStatics.KeyBinding = Settings.Instance.inputActionAsset.SaveBindingOverridesAsJson();
                    SaveDataManager.SaveSettings();
                })
                .Start();
        }

        private void RebindCleanUp()
        {
            RebindingOperation.Dispose();
            currentAction.action.Enable();
            selectedMarkObject.SetActive(true);
        }

        private void ShowBindText()
        {
            // bindingIndex를 전달해야 정확한 키 이름이 나옵니다.
            oldPath = currentAction.action.bindings[bindingIndex].effectivePath;
            bindingDisplayNameText.text = currentAction.action.GetBindingDisplayString(bindingIndex, displayStringOptions);
        }
    }
}
