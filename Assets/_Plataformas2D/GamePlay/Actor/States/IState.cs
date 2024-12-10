public interface IState
{
    public void OnEnter();
    public void UpdateState();
    public void FixedUpdateState();
    public void OnExit();
}