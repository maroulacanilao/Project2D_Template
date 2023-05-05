using System.Collections;

public abstract class BattleControllerState
{
    protected BattleCharacterController Controller;
    protected BattleManager BattleManager;

    public BattleControllerState(BattleCharacterController controller_)
    {
        Controller = controller_;
        BattleManager = BattleManager.Instance;
    }

    public abstract IEnumerator EnterState();
    public abstract IEnumerator StateLogic();
    public abstract IEnumerator ExitLogic();
}