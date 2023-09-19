public class State
{
    protected MarineObject marine;
    protected StateMachine stateMachine;

    public State(MarineObject marine, StateMachine stateMachine)
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
