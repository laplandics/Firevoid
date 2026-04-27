using R3;

namespace Boot
{
    public class GameBoot
    {
        public void Boot(out Subject<Unit> onExit)
        {
            var exitSubject = new Subject<Unit>();
            
            onExit = exitSubject;
        }
    }
}