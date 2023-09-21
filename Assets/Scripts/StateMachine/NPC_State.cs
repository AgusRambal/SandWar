public class NPC_State
{
    protected NPC_Marine marine;
    protected StateMachine stateMachine;

    public NPC_State(NPC_Marine marine, StateMachine stateMachine)
    {
        this.marine = marine;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void FixedState() { }
    public virtual void AnimationTriggerEvent() { }
}
