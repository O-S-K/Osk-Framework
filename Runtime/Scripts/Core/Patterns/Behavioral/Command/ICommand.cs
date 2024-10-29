namespace OSK
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}