// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class PlayerGameControls : InputActionAssetReference
{
    public PlayerGameControls()
    {
    }
    public PlayerGameControls(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Gameplay
        m_Gameplay = asset.GetActionMap("Gameplay");
        m_Gameplay_StrifeLeft = m_Gameplay.GetAction("StrifeLeft");
        m_Gameplay_StrifeRight = m_Gameplay.GetAction("StrifeRight");
        m_Gameplay_Move = m_Gameplay.GetAction("Move");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Gameplay = null;
        m_Gameplay_StrifeLeft = null;
        m_Gameplay_StrifeRight = null;
        m_Gameplay_Move = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Gameplay
    private InputActionMap m_Gameplay;
    private InputAction m_Gameplay_StrifeLeft;
    private InputAction m_Gameplay_StrifeRight;
    private InputAction m_Gameplay_Move;
    public struct GameplayActions
    {
        private PlayerGameControls m_Wrapper;
        public GameplayActions(PlayerGameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @StrifeLeft { get { return m_Wrapper.m_Gameplay_StrifeLeft; } }
        public InputAction @StrifeRight { get { return m_Wrapper.m_Gameplay_StrifeRight; } }
        public InputAction @Move { get { return m_Wrapper.m_Gameplay_Move; } }
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
    }
    public GameplayActions @Gameplay
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new GameplayActions(this);
        }
    }
}
