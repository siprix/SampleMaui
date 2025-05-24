namespace Siprix;


public interface ICallNotifService
{
    void Create(ICoreService core);
    void ToggleForegroundMode();
}


public class StubCallNotifService : ICallNotifService
{
    public void Create(ICoreService core) { }
    public void ToggleForegroundMode() { }
}