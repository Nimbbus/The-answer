using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class GameInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }

    public GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""version"": 1,
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""8f0e74eb-6369-402e-9963-6a4c53c41c6a"",
            ""actions"": [
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b29bc8ee-b327-4284-af64-6480ac9f445b"",
                    ""expectedControlType"": ""Vector2"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7c13051e-2d6e-49ca-83c7-15902f2779b0"",
                    ""expectedControlType"": ""Button""
                },
                {
                    ""name"": ""scrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bf7473e7-931b-4817-b69b-12d4bddbb42b"",
                    ""expectedControlType"": ""Vector2"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""submit"",
                    ""type"": ""Button"",
                    ""id"": ""8c5345f5-6c20-4780-bc47-cee721d287f8""
                },
                {
                    ""name"": ""cancel"",
                    ""type"": ""Button"",
                    ""id"": ""45b29210-c24c-4d27-9cfb-f42f7a14e88c""
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d1c9b3f1-1234-4567-890a-bcdef1234567"",
                    ""expectedControlType"": ""Vector2"",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""id"": ""327c4dd0-a8de-4fc4-b8cc-ea95cfa039ec"",
                    ""path"": ""<Mouse>/position"",
                    ""action"": ""Point""
                },
                {
                    ""id"": ""80fbb9c1-e336-41de-b9f8-00a81bf7c32a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""action"": ""click""
                },
                {
                    ""id"": ""8a5dc984-7932-4d99-bbc5-7655ceab2706"",
                    ""path"": ""<Mouse>/scroll"",
                    ""action"": ""scrollWheel""
                },
                {
                    ""id"": ""f6c59b47-516f-4d5d-9f8e-54d47c895b25"",
                    ""path"": ""<Keyboard>/enter"",
                    ""action"": ""submit""
                },
                {
                    ""id"": ""5e489b69-a43a-41a7-ace9-3c40ef475d07"",
                    ""path"": ""<Keyboard>/escape"",
                    ""action"": ""cancel""
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e2f9a4b2-2345-5678-901b-cdef23456789"",
                    ""path"": ""2DVector"",
                    ""action"": ""Navigate"",
                    ""isComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678901"",
                    ""path"": ""<Keyboard>/w"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678902"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678903"",
                    ""path"": ""<Keyboard>/s"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678904"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678905"",
                    ""path"": ""<Keyboard>/a"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678906"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678907"",
                    ""path"": ""<Keyboard>/d"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f3a0b5c3-3456-6789-012c-def345678908"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""action"": ""Navigate"",
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_click = m_UI.FindAction("click", throwIfNotFound: true);
        m_UI_scrollWheel = m_UI.FindAction("scrollWheel", throwIfNotFound: true);
        m_UI_submit = m_UI.FindAction("submit", throwIfNotFound: true);
        m_UI_cancel = m_UI.FindAction("cancel", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
    }

    ~GameInputActions()
    {
        UnityEngine.Debug.Assert(!m_UI.enabled, "GameInputActions.UI.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action) => asset.Contains(action);
    public IEnumerator<InputAction> GetEnumerator() => asset.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public void Enable() => asset.Enable();
    public void Disable() => asset.Disable();
    public IEnumerable<InputBinding> bindings => asset.bindings;
    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false) => asset.FindAction(actionNameOrId, throwIfNotFound);
    public int FindBinding(InputBinding bindingMask, out InputAction action) => asset.FindBinding(bindingMask, out action);

    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_click;
    private readonly InputAction m_UI_scrollWheel;
    private readonly InputAction m_UI_submit;
    private readonly InputAction m_UI_cancel;
    private readonly InputAction m_UI_Navigate;

    public struct UIActions
    {
        private GameInputActions m_Wrapper;
        public UIActions(GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction Point => m_Wrapper.m_UI_Point;
        public InputAction click => m_Wrapper.m_UI_click;
        public InputAction scrollWheel => m_Wrapper.m_UI_scrollWheel;
        public InputAction submit => m_Wrapper.m_UI_submit;
        public InputAction cancel => m_Wrapper.m_UI_cancel;
        public InputAction Navigate => m_Wrapper.m_UI_Navigate;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }

        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            Point.started += instance.OnPoint;
            Point.performed += instance.OnPoint;
            Point.canceled += instance.OnPoint;
            click.started += instance.OnClick;
            click.performed += instance.OnClick;
            click.canceled += instance.OnClick;
            scrollWheel.started += instance.OnScrollWheel;
            scrollWheel.performed += instance.OnScrollWheel;
            scrollWheel.canceled += instance.OnScrollWheel;
            submit.started += instance.OnSubmit;
            submit.performed += instance.OnSubmit;
            submit.canceled += instance.OnSubmit;
            cancel.started += instance.OnCancel;
            cancel.performed += instance.OnCancel;
            cancel.canceled += instance.OnCancel;
            Navigate.started += instance.OnNavigate;
            Navigate.performed += instance.OnNavigate;
            Navigate.canceled += instance.OnNavigate;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            Point.started -= instance.OnPoint;
            Point.performed -= instance.OnPoint;
            Point.canceled -= instance.OnPoint;
            click.started -= instance.OnClick;
            click.performed -= instance.OnClick;
            click.canceled -= instance.OnClick;
            scrollWheel.started -= instance.OnScrollWheel;
            scrollWheel.performed -= instance.OnScrollWheel;
            scrollWheel.canceled -= instance.OnScrollWheel;
            submit.started -= instance.OnSubmit;
            submit.performed -= instance.OnSubmit;
            submit.canceled -= instance.OnSubmit;
            cancel.started -= instance.OnCancel;
            cancel.performed -= instance.OnCancel;
            cancel.canceled -= instance.OnCancel;
            Navigate.started -= instance.OnNavigate;
            Navigate.performed -= instance.OnNavigate;
            Navigate.canceled -= instance.OnNavigate;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }

    public UIActions UI => new UIActions(this);

    public interface IUIActions
    {
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
    }
}
