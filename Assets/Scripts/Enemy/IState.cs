public interface IState
{
    void Enter(); // що зробити один раз при вході
    void Tick();  // що робити щокадру
    void Exit();  // що прибрати за собою при виході
}
